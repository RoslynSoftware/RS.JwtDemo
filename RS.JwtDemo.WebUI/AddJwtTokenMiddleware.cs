using System.Security.Claims;

namespace RS.JwtDemo.WebUI
{
    /// <summary>
    /// Middleware to add JWT token from session to the request headers.
    /// </summary>
    public class AddJwtTokenMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddJwtTokenMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        public AddJwtTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the middleware to add the JWT token to the request headers if it exists in the session.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A task that represents the completion of request processing.</returns>
        public async Task Invoke(HttpContext context)
        {
            // Retrieve the token from the session
            var token = context.Session.GetString("token");

            // If the token is not null or empty, add it to the request headers
            if (!string.IsNullOrEmpty(token))
            {
                context.Request.Headers.Add("Authorization", $"Bearer {token}");
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }

    /// <summary>
    /// Extension methods for adding the <see cref="AddJwtTokenMiddleware"/> to the application pipeline.
    /// </summary>
    public static class AddJwtTokenExtension
    {
        /// <summary>
        /// Adds the <see cref="AddJwtTokenMiddleware"/> to the application's request pipeline.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>The application builder with the middleware added.</returns>
        public static IApplicationBuilder UseJwt(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AddJwtTokenMiddleware>();
        }
    }

}
