using NanoidDotNet;
using OneOf;
using Serilog;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Web.Database;
using Web.Models;
using Web.Models.Api;
using Web.Models.OneOf;
using Web.Utils;

namespace Web.Services.Auth
{
    public class AuthService
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly EmailUtils _emailUtils;

        private readonly DatabaseContext _db;

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;
        private readonly Serilog.ILogger _logger;
        private readonly IWebHostEnvironment _env;

        public AuthService(
            UserService userService,
            RoleService roleService,
            DatabaseContext db,
            IHttpContextAccessor contextAccessor,
            IConfiguration config,
            IWebHostEnvironment env,
            ILogger<AuthService> logger
        )
        {
            _userService = userService;
            _roleService = roleService;

            _db = db;
            _contextAccessor = contextAccessor;
            _configuration = config;
            _emailUtils = new EmailUtils(env);
            _env = env;

            _logger = Log.ForContext<AuthService>();
        }

        public async Task<OneOf<Success<User>, Error>> AuthenticateUserAsync(LoginRequest request)
        {
            var user = await _userService.FindUserAsync(request.EmailAddress, _userService.GetSha256Hash(request.Password));
            if (user is null)
            {
                _logger.Warning("{method}: Either user does not exist or password is incorrect.", nameof(AuthenticateUserAsync));
                return new Error("The credentials were incorrect. Try again.");
            }

            _logger.Information("{method}: Successfully authenticated user.", nameof(AuthenticateUserAsync));
            return new Success<User>(user);
        }

        public async Task<OneOf<Success<User>, BadRequest>> RegisterUserAsync(RegisterRequest request)
        {
            var user = await _userService.FindUserByEmailAsync(request.EmailAddress);
            if (user != null)
            {
                _logger.Warning("{method}: User already exists.", nameof(RegisterUserAsync));
                return new BadRequest("A user with this email address already exists.");
            }

            if (request.EmailAddress.Contains("+"))
            {
                _logger.Warning("{method}: Email contains a plus sign.", nameof(RegisterUserAsync));
                return new BadRequest("You cannot register with an alias email address.");
            }

            // check if email is in temp email list
            if (_emailUtils.IsEmailInTempList(request.EmailAddress))
            {
                _logger.Warning("{method}: Email is in temp email list.", nameof(RegisterUserAsync));
                return new BadRequest("You cannot register with a temporary email address.");
            }

            string emailUsername = request.EmailAddress.Split('@')[0];
            string name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(emailUsername.Replace('.', ' '));

            var newUser = new User()
            {
                UserId = Nanoid.Generate(),
                EmailAddress = request.EmailAddress,
                Name = name,
                Password = _userService.GetSha256Hash(request.Password),
            };

            var addedUser = await _userService.CreateUserAsync(newUser, 2);

            _logger.Information("{method}: Successfully registered user.", nameof(RegisterUserAsync));
            return new Success<User>(addedUser);
        }

        public async Task<User?> GetAuthenticatedUser(ClaimsPrincipal User)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userId = GetAuthenticatedUserId(User);
                if (userId != null)
                {
                    var id = userId ?? default;
                    return await _userService.FindUserAsync(id);
                }
                return null;
            }
            return null;
        }

        public string? GetAuthenticatedUserId(ClaimsPrincipal User)
        {
            var userId = User.FindFirst("UserId")?.Value;
            return userId;
        }

        public string? GetAuthenticatedUserEmail(ClaimsPrincipal User)
        {
            var userId = User.FindFirst("Email")?.Value;
            return userId;
        }

        public string? GetAuthenticatedUserDisplayName(ClaimsPrincipal User)
        {
            var displayName = User.FindFirst("Nickname")?.Value;
            return displayName;
        }

        public bool IsAdmin(ClaimsPrincipal User)
        {
            return User.IsInRole(CustomRoles.Admin);
        }

        public string GetClaimsFromTokenString(string token, string claims)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var result = jwtToken.Claims.First(claim => claim.Type == claims).Value;
                return result;
            }

            catch (Exception)
            {
                return null;
            }
        }
    }
}
