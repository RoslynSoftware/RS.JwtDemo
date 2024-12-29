using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations.Rules;
using RS.JwtDemo.AuthService.Utils;
using RS.JwtDemo.Core.Demo;
using RS.JwtDemo.Core.Models;

namespace RS.JwtDemo.AuthService.Controllers
{
    /// <summary>
    /// Controller to handle authentication-related actions.
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TestData _testData;
        private readonly TokenGenerator _tokenGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration settings.</param>
        public AuthController(IConfiguration configuration)
        {
            _testData = new TestData();
            _tokenGenerator = new TokenGenerator(configuration);
        }

        /// <summary>
        /// Handles user login and token generation.
        /// </summary>
        /// <param name="model">The login model containing user credentials.</param>
        /// <returns>An <see cref="IActionResult"/> containing the JWT token if successful.</returns>
        [HttpPost("login")]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
                return BadRequest();

            // Fetch the test data and find the user with matching email and password
            User? user = _testData.GetTestData().Result.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);

            // If user is not found, return Unauthorized
            if (user == null)
                return Unauthorized();

            // Generate the JWT token for the authenticated user
            string token = _tokenGenerator.GenerateToken(user, model.Audience, model.IsRefreshToken);

            // Return the token in the response
            return Ok(new { token = token});
        }
    }
}
