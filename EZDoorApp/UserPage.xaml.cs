using Org.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
namespace EZDoorApp;

public partial class UserPage : ContentPage
{
    private static System.Timers.Timer aTimer;
    public UserPage()
	{
		InitializeComponent();

        aTimer = new System.Timers.Timer();
        aTimer.Interval = 1000;
        aTimer.Elapsed += RefreshData;
        aTimer.AutoReset = true;
        aTimer.Enabled = true;
    }
    private async void RefreshData(Object source, System.Timers.ElapsedEventArgs e)
    {
        // Create an anonymous object with the specified properties
        var requestBody = new
        {
            ime = "nejcnarobe@gmail.com",
            geslo = "!Geslo13"
        };

        // Serialize the anonymous object to a JSON string
        string jsonBody = JsonSerializer.Serialize(requestBody);

        // Make a POST request with the JSON body
        string token = await MakePostRequest("https://ezdoor.azurewebsites.net/api/token", jsonBody);

        // Deserialize the response and update UI
        if (token != null)
        {
            string cleanedToken = token.Trim('"').Replace("\\", ""); ;

            // Deserialize the cleaned response
            Token tokenObject = JsonSerializer.Deserialize<Token>(cleanedToken);

            int cifra = tokenObject.cifra;
            int cas = tokenObject.cas;
            if(cas == -1)
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopToRootAsync();
                });
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Code.Text = cifra.ToString();
                Time.Text = cas.ToString();
            });
        }
    }

    private static async Task<string> MakePostRequest(string url, string jsonBody)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                // Set the content type to JSON
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Create the HTTP content with the JSON body
                HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                // Make the POST request
                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}

public class Token {
    public int cifra { get; set; }
        
    public int cas { get; set; }
}