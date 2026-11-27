using System.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TrainSystem.Api.Data;
using TrainSystem.Api.Models;
using TrainSystem.Api.Models.Auth;

namespace TrainSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ApplicationDbContext _dbContext;
    private readonly IWebHostEnvironment _environment;
    private readonly TokenValidationParameters _jwtParameters;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext dbContext,
        IWebHostEnvironment environment,
        IOptionsMonitor<JwtBearerOptions> jwtOptions)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _dbContext = dbContext;
        _environment = environment;
        _jwtParameters = jwtOptions.Get(JwtBearerDefaults.AuthenticationScheme).TokenValidationParameters;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            FullName = request.FullName,
            UserName = request.Email,
            Email = request.Email
        };

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        IdentityResult result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        IdentityResult roleResult = await _userManager.AddToRoleAsync(user, "Passenger");

        if (!roleResult.Succeeded)
        {
            return Problem("Could not assign the Passenger role.");
        }

        await transaction.CommitAsync();
        return StatusCode(StatusCodes.Status201Created, new { user.Id, user.FullName, user.Email });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return Unauthorized();
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            return Unauthorized();
        }

        return Ok(new { accessToken = await CreateTokenAsync(user) });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return Unauthorized();
        }

        IList<string> roles = await _userManager.GetRolesAsync(user);
        return Ok(new { user.Id, user.FullName, user.Email, roles });
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        return await Me();
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request)
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return Unauthorized();
        }

        user.FullName = request.FullName;
        IdentityResult result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { user.Id, user.FullName, user.Email });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return Unauthorized();
        }

        IdentityResult result = await _userManager.ChangePasswordAsync(
            user, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { message = "Password updated." });
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        if (!IsLocalDevelopment())
        {
            return NotFound();
        }

        ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return BadRequest(new { message = "Invalid email." });
        }

        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        return Ok(new { resetToken = token });
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        if (!IsLocalDevelopment())
        {
            return NotFound();
        }

        ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return BadRequest(new { message = "Invalid email or reset token." });
        }

        IdentityResult result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

        if (!result.Succeeded)
        {
            if (result.Errors.Any(error => error.Code == "InvalidToken"))
            {
                return BadRequest(new { message = "Invalid email or reset token." });
            }

            return BadRequest(result.Errors);
        }

        return Ok(new { message = "Password reset successfully." });
    }

    private bool IsLocalDevelopment()
    {
        IPAddress? address = HttpContext.Connection.RemoteIpAddress;
        return _environment.IsDevelopment() && address != null && IPAddress.IsLoopback(address);
    }

    private async Task<string> CreateTokenAsync(ApplicationUser user)
    {
        IList<string> roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim> { new(JwtRegisteredClaimNames.Sub, user.Id) };

        foreach (string role in roles)
        {
            claims.Add(new Claim("role", role));
        }

        var token = new JwtSecurityToken(
            issuer: _jwtParameters.ValidIssuer,
            audience: _jwtParameters.ValidAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(_jwtParameters.IssuerSigningKey, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
