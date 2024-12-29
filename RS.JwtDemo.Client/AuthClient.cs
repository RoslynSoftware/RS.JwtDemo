using RS.JwtDemo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RS.JwtDemo.Client
{
    /// <summary>
    /// Provides methods to handle authentication with a remote API.
    /// </summary>
    public class AuthClient
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthClient"/> class with the specified base URL.
        /// </summary>
        /// <param name="baseUrl">The base URL of the API.</param>
        public AuthClient(string baseUrl)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl);
        }

        /// <summary>
        /// Sends a login request to the API with the provided login model.
        /// </summary>
        /// <param name="model">The login model containing the credentials.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the JWT token if the login is successful; otherwise, an empty string.</returns>
        public async Task<string> Login(LoginModel model)
        {
            // Send a POST request with the login model as JSON
            HttpResponseMessage response = await _client.PostAsJsonAsync("", model);

            // Check if the response indicates success
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a JsonElement
                JsonElement content = await response.Content.ReadFromJsonAsync<JsonElement>();

                // Try to get the "token" property from the JSON response
                if (content.TryGetProperty("token", out var jsonElement))
                {
                     return jsonElement.GetString();
                }
            }

            // Return an empty string if the login is not successful
            return string.Empty;
        }
    }
}
