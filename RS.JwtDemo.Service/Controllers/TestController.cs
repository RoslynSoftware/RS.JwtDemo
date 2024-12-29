using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RS.JwtDemo.Core.Models;

namespace RS.JwtDemo.Service.Controllers
{
    /// <summary>
    /// Controller to handle test-related actions.
    /// </summary>
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// Handles POST requests to the "post" endpoint.
        /// </summary>
        /// <param name="model">The request model containing the data.</param>
        /// <returns>An <see cref="IActionResult"/> containing the response message.</returns>
        [HttpPost("post")]
        [Route("post")]
        [Authorize(AuthenticationSchemes = "App2")]
        public IActionResult Post(RequestModel model)
        {
            // Return a success message with the provided data
            return Ok(new ResponseModel
            {
                Message = $"Success {model.Data}"
            });
        }
    }
}
