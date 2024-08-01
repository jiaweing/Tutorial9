using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Web.Models
{
    public class UserClaims
    {
        public string Jti { get; set; }
        public string Iat { get; set; }
        public string Nickname { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public UserClaims(ClaimsPrincipal user)
        {
            Jti = user.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            Iat = user.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Iat)?.Value;
            Nickname = user.Claims.FirstOrDefault(c => c.Type == "Nickname")?.Value;
            UserId = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            Email = user.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            Role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        }
    }
}