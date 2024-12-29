using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RS.JwtDemo.WebUI
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
            builder.Services.AddControllersWithViews();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Configure JWT authentication
            builder.Services.AddAuthentication(options =>
            {
                // Set the default authentication scheme to JWT Bearer
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // Set the default challenge scheme to JWT Bearer
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer("App1", options =>
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
                   ValidAudience = jwtSettings["App1Audience"],

                   // Validate the token's lifetime
                   ValidateLifetime = true,
               };
           });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // Enable session management
            app.UseSession();

            // Add custom middleware to add JWT token to request headers
            app.UseJwt();

            app.UseAuthentication();
            app.UseAuthorization();

            // Map static assets and default controller route
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
