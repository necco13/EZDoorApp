using Org.Json;
using System.Net.Http;
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
        string token = await MakeGetRequest("https://ezdoor.azurewebsites.net/api/token");
        int cifra  = JsonSerializer.Deserialize<Token>(token).cifra;
        int cas = JsonSerializer.Deserialize<Token>(token).cas;
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Code.Text = cifra.ToString();
            Time.Text = cas.ToString();
        });
    }

    private static async Task<string> MakeGetRequest(string url)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
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