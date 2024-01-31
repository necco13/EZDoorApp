using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Microsoft.Maui.Graphics;
using System;

namespace EZDoorApp;

public partial class Register : ContentPage
{

	public Register()
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
        if (PasswordEntry.Text.Equals(RepeatPasswordEntry.Text))
        {
            string resp = await MakePostRequest("https://ezdoor.azurewebsites.net/api/register", jsonBody);
            Console.WriteLine(resp);
            if (resp.Equals("true"))
            {
                await Navigation.PopToRootAsync();
            }
            else
            {
                UsernameEntry.Text = "";
                PasswordEntry.Text = "";
                RepeatPasswordEntry.Text = "";
                MessageLabel.Text = resp;
            }
        }
        else
            MessageLabel.Text = "The passwords don't match!";


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

