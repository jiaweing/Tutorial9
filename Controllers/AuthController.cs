using Web.Models;
using Web.Models.Api;
using Web.Services.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Web.Controllers
{
    public class AuthController : Controller
	{
		private readonly AuthService _authService;
		private readonly RoleService _roleService;
		private readonly ILogger<AuthController> _logger;

		public AuthController(
			AuthService service,
			RoleService roleService,
			ILogger<AuthController> logger
		)
		{
            _authService = service;
			_roleService = roleService;
			_logger = logger;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginRequest model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var response = await _authService.AuthenticateUserAsync(model);
			if (response.IsT0)
			{
				if (await CreateJwtAndSignIn(response.AsT0.Result))
				{
					return RedirectToAction("Dashboard", "Home");
				}
			}

			ModelState.AddModelError(string.Empty, response.AsT1.Message);
			return View(model);
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterRequest model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var response = await _authService.RegisterUserAsync(model);
			if (response.IsT0)
			{
				if (await CreateJwtAndSignIn(response.AsT0.Result))
				{
					return RedirectToAction("Dashboard", "Home");
				}
			}

			ModelState.AddModelError(string.Empty, response.AsT1.Message);
			return View(model);
		}

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }


        private async Task<bool> CreateJwtAndSignIn(User user)
		{
			if (user is null)
			{
				return false;
			}

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture)),
                new Claim("Nickname", user.Name),
                new Claim("UserId", user.UserId),
                new Claim("Email", user.EmailAddress.ToString()),
            };

            // add roles
            var roles = await _roleService.FindUserRolesAsync(user.UserId);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();

			// this can only be executed in controller
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

			return true;
        }
	}
}
