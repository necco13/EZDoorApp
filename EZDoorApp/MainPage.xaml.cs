using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Microsoft.Maui.Graphics;

namespace EZDoorApp;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnLoginClicked(object sender, EventArgs e)
	{
        TryLogin();
    }

    private async void TryLogin()
    {
        var requestBody = new
        {
            ime = UsernameEntry.Text,
            geslo = PasswordEntry.Text
        };

        // Serialize the anonymous object to a JSON string
        string jsonBody = JsonSerializer.Serialize(requestBody);

        // Make a POST request with the JSON body
        string resp = await MakePostRequest("https://ezdoor.azurewebsites.net/api/overi", jsonBody);
        Console.WriteLine(resp);
        if (resp.Equals("true"))
        {
            UsernameEntry.Text = "";
            PasswordEntry.Text = "";
            MessageLabel.Text = "";
            await Navigation.PushAsync(new UserPage());
        }
        else
        {
            UsernameEntry.Text = "";
            PasswordEntry.Text = "";
            MessageLabel.Text = "Wrong username or password!";
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

