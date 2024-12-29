using RS.JwtDemo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RS.JwtDemo.Core.Demo
{
    /// <summary>
    /// Provides methods to fetch test data from a remote source.
    /// </summary>
    public class TestData
    {
        // URL of the remote JSON data source
        private readonly string url = @"https://raw.githubusercontent.com/RoslynSoftware/TestData/refs/heads/main/users.json";
        private HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestData"/> class.
        /// </summary>
        public TestData()
        {
            _client = new HttpClient();
        }

        /// <summary>
        /// Asynchronously fetches test data from the remote source.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="User"/> objects.</returns>
        public async Task<List<User>> GetTestData()
        {
            // Fetch JSON content from the remote URL
            string jsonContent = await _client.GetStringAsync(url);

            // Deserialize the JSON content to a list of User objects
            List<User> users = JsonSerializer.Deserialize<List<User>>(jsonContent);
            return users;
        }
    }
}
