using Newtonsoft.Json;
using RS.JwtDemo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RS.JwtDemo.Client
{
    /// <summary>
    /// Provides methods to interact with a remote API.
    /// </summary>
    public class ApiClient
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient"/> class with the specified base URL.
        /// </summary>
        /// <param name="baseUrl">The base URL of the API.</param>
        public ApiClient(string baseUrl)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl);
        }

        /// <summary>
        /// Sends a POST request to the specified endpoint with the provided model and authorization token.
        /// </summary>
        /// <typeparam name="T1">The type of the model to be sent in the request body.</typeparam>
        /// <typeparam name="T2">The type of the response expected from the API.</typeparam>
        /// <param name="model">The model to be sent in the request body.</param>
        /// <param name="endpoint">The API endpoint to send the request to.</param>
        /// <param name="token">The authorization token to be included in the request headers.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response from the API.</returns>
        public async Task<T2> Post<T1, T2>(T1 model, string endpoint, string token)
        {
            // Set the authorization header with the provided token
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Send a POST request with the model as JSON
            HttpResponseMessage response = await _client.PostAsJsonAsync(endpoint, model);

            // Read the response content as a string
            string responseContent = await response.Content.ReadAsStringAsync();

            // Deserialize the response content to the expected type
            T2 result = JsonConvert.DeserializeObject<T2>(responseContent);
            return result;
        }
    }
}
