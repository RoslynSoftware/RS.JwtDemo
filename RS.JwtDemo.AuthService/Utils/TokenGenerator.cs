using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using RS.JwtDemo.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RS.JwtDemo.AuthService.Utils
{
    /// <summary>
    /// Provides methods to generate JWT tokens.
    /// </summary>
    public class TokenGenerator
    {
        private readonly IConfigurationSection _jwtSettings;
        private readonly JwtSecurityTokenHandler _handler;
        private SecurityTokenDescriptor _descriptor;
        private readonly SymmetricSecurityKey _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenGenerator"/> class.
        /// </summary>
        /// <param name="configuration">The configuration settings.</param>
        public TokenGenerator(IConfiguration configuration)
        {
            _jwtSettings = configuration.GetSection("Jwt");
            _handler = new JwtSecurityTokenHandler();
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings["Key"]));
        }

        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <param name="Audience">The audience for the token.</param>
        /// <param name="isRefreshToken">Indicates whether the token is a refresh token.</param>
        /// <returns>The generated JWT token as a string.</returns>
        public string GenerateToken(User user, string Audience, bool? isRefreshToken = false)
        {
            // Configure the token descriptor with claims and other properties
            _descriptor = new SecurityTokenDescriptor()
            {
                // The issuer of the token
                Issuer = _jwtSettings["Issuer"],
                // The audience of the token
                Audience = Audience,
                // The expiration time of the token
                Expires = DateTime.Now.AddMinutes(isRefreshToken == false ? Convert.ToDouble(_jwtSettings["ExpiresInMinutes"]) : Convert.ToDouble(_jwtSettings["RefreshTokenExpiresInMinutes"])),
                // The issued at time of the token
                IssuedAt = DateTime.Now,
                // The signing credentials for the token
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256),
                // The type of the token
                TokenType = "JwtBearer",
                // The claims included in the token
                Subject = new ClaimsIdentity(new Claim[]
                {
                    // Standard JWT claims

                    // The issuer of the token
                    new Claim(JwtRegisteredClaimNames.Iss, _jwtSettings["Issuer"]),
                    // The audience of the token
                    new Claim(JwtRegisteredClaimNames.Aud, Audience),
                    // The subject of the token, usually the user ID
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    // The issued at time of the token, in Unix timestamp format
                    new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                    // The authentication time
                    new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString()),
                    // The unique identifier for the token (JWT ID)
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    // A unique value to prevent replay attacks
                    new Claim(JwtRegisteredClaimNames.Nonce, Guid.NewGuid().ToString()),
                    // The authorized party, usually the application ID
                    new Claim(JwtRegisteredClaimNames.Azp, _jwtSettings["AppId"]),
                    // The authentication context class reference
                    new Claim(JwtRegisteredClaimNames.Acr, "JwtBearer"),
                    // The authentication method reference
                    new Claim(JwtRegisteredClaimNames.Amr, "UserCredentials"),
                    // The expiration time of the token, in Unix timestamp format
                    new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddMinutes(30)).ToUnixTimeSeconds().ToString()),
                    // The not before time, specifying when the token becomes valid
                    new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()),


                    // Custom claims

                    // The user ID
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    // The user's full name
                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                    // The user's email address
                    new Claim(ClaimTypes.Email, user.Email),
                    // The user's role
                    new Claim(ClaimTypes.Role, user.Role),
                    // The user's first name
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    // The user's last name
                    new Claim(ClaimTypes.Surname, user.LastName),
                    // The user's date of birth
                    new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.ToString("dd/MM/yyyy")),
                    // The user's country
                    new Claim(ClaimTypes.Country, user.Country),
                    // The authentication method
                    new Claim(ClaimTypes.Authentication, "UserCredentials"),
                    // The expiration time of the token
                    new Claim(ClaimTypes.Expiration,  DateTime.Now.AddMinutes(isRefreshToken == false ? Convert.ToDouble(_jwtSettings["ExpiresInMinutes"]) : Convert.ToDouble(_jwtSettings["RefreshTokenExpiresInMinutes"])).ToString()),
                    // The authentication method
                    new Claim(ClaimTypes.AuthenticationMethod, "JwtBearer"),
                })
            };

            // Create the JWT token
            JwtSecurityToken token = _handler.CreateJwtSecurityToken(_descriptor);

            // Return the serialized token
            return _handler.WriteToken(token);
        }
    }
}
