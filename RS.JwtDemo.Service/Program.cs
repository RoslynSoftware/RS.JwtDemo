using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RS.JwtDemo.Service
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            IConfigurationSection? jwtSettings = builder.Configuration.GetSection("Jwt");

            // Add services to the container.
            builder.Services.AddControllers();

            // Configure JWT authentication
            builder.Services.AddAuthentication(options =>
            {
                // Set the default authentication scheme to JWT Bearer
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // Set the default challenge scheme to JWT Bearer
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("App2", options =>
            {
                // Require HTTPS metadata for the JWT tokens
                options.RequireHttpsMetadata = true;
                // Save the token in the authentication properties
                options.SaveToken = true;

                // Configure the token validation parameters
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Validate the signing key used to generate the token
                    ValidateIssuerSigningKey = true,
                    // Specify the key used to sign the token
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])),

                    // Validate the issuer of the token
                    ValidateIssuer = true,
                    // Specify the valid issuer
                    ValidIssuer = jwtSettings["Issuer"],

                    // Validate the audience of the token
                    ValidateAudience = true,
                    // Specify the valid audience
                    ValidAudience = jwtSettings["App2Audience"],

                    // Validate the token's lifetime
                    ValidateLifetime = true,
                };
            });

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
