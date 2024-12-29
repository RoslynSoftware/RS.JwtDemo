using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RS.JwtDemo.Client;
using RS.JwtDemo.Core.Models;
using System.Diagnostics;

namespace RS.JwtDemo.WebUI.Controllers
{
    /// <summary>
    /// Controller to handle home-related actions and views.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Displays the home page.
        /// </summary>
        /// <returns>The home page view.</returns>
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Handles user login and redirects to the secure page.
        /// </summary>
        /// <returns>A redirect to the secure page.</returns>
        [AllowAnonymous]
        public IActionResult UserLogin()
        {
            // Create an AuthClient instance and login model
            AuthClient client = new AuthClient("https://localhost:7044/api/auth/login");
            LoginModel model = new LoginModel
            {
                Email = "jane.smith@example.com",
                Password = "SecurePass123!",
                Audience = "App1",
                IsRefreshToken = false
            };

            // Perform login and get the token
            string token = client.Login(model).Result;

            // Store the token in the session
            HttpContext.Session.SetString("token", token);

            // Redirect to the secure page
            return RedirectToAction(nameof(SecurePage));
        }

        /// <summary>
        /// Handles admin login and redirects to the secure admin page.
        /// </summary>
        /// <returns>A redirect to the secure admin page.</returns>
        [AllowAnonymous]
        public IActionResult AdminLogin()
        {
            // Create an AuthClient instance and login model
            AuthClient client = new AuthClient("https://localhost:7044/api/auth/login");
            LoginModel model = new LoginModel
            {
                Email = "john.doe@example.com",
                Password = "Password123!",
                Audience = "App1",
                IsRefreshToken = false
            };

            // Perform login and get the token
            string token = client.Login(model).Result;

            // Store the token in the session
            HttpContext.Session.SetString("token", token);

            // Redirect to the secure admin page
            return RedirectToAction(nameof(SecureAdminPage));
        }

        /// <summary>
        /// Displays the secure page for authenticated users.
        /// </summary>
        /// <returns>The secure page view.</returns>
        [Authorize(AuthenticationSchemes = "App1")]
        public IActionResult SecurePage()
        {
            return View();
        }

        /// <summary>
        /// Displays the secure admin page for authenticated users with the Admin role.
        /// </summary>
        /// <returns>The secure admin page view.</returns>
        [Authorize(AuthenticationSchemes = "App1", Roles = "Admin")]
        public IActionResult SecureAdminPage()
        {
            return View();
        }

        /// <summary>
        /// Tests the service by sending a request to the test API endpoint.
        /// </summary>
        /// <returns>The view with the response from the test API.</returns>
        [AllowAnonymous]
        public IActionResult ServiceTest()
        {
            // Create an AuthClient instance and login model
            AuthClient client = new AuthClient("https://localhost:7044/api/auth/login");
            LoginModel model = new LoginModel
            {
                Email = "jane.smith@example.com",
                Password = "SecurePass123!",
                Audience = "App2",
                IsRefreshToken = false
            };

            // Perform login and get the token
            string token = client.Login(model).Result;

            // Create an ApiClient instance and request model
            ApiClient apiClient = new ApiClient("https://localhost:7233/api/test/");
            RequestModel request = new RequestModel
            {
                Data = "Hello from WebUI"
            };

            // Send the request to the test API and get the response
            ResponseModel response = apiClient.Post<RequestModel, ResponseModel>(request, "post", token).Result;

            // Return the view with the response
            return View(response);
        }
    }
}
