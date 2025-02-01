using EventosPro.Models;
using EventosPro.Services.Interfaces;
using EventosPro.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventosPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICookieService _cookieService;
        private readonly IEmailService _emailService;
        private readonly IPasswordService _passwordService;
        private readonly IPasswordResetService _passwordResetService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ICookieService cookieService, IEmailService emailService, IPasswordService passwordService, IPasswordResetService passwordResetService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _cookieService = cookieService;
            _emailService = emailService;
            _passwordService = passwordService;
            _passwordResetService = passwordResetService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (model.Password != model.ConfirmPassword)
                    return BadRequest("Passwords do not match.");

                if (!_passwordService.ValidatePasswordStrength(model.Password))
                    return BadRequest("Password does not meet minimum security requirements.");

                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email
                };

                await _userService.AddUserAsync(user, model.Password);
                await _emailService.SendEmailConfirmationAsync(user.Email, user.ConfirmationToken, user.Name, "");

                return Ok(new { message = "Registration completed successfully. Please check your email to confirm your account." });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "User registration error");
                return StatusCode(500, "An error occurred while processing your registration.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userService.GetUserByEmailAsync(model.Email);
                if (user == null)
                    return Unauthorized("Invalid username or password.");

                if (!await _userService.ValidateCredentialsAsync(model.Email, model.Password))
                    return Unauthorized("Invalid username or password.");

                if (!user.IsConfirmed)
                    return BadRequest("Please confirm your email before logging in.");

                await _cookieService.SignInWithCookieAsync(user, model.RememberMe);

                return Ok(new { message = "Login successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "User login error");
                return StatusCode(500, "An error occurred while processing your login.");
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _cookieService.SignOutFromCookieAsync(HttpContext);
                return Ok(new { message = "Logout completed successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "User logout error");
                return StatusCode(500, "An error occurred while processing your logout.");
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _passwordResetService.InitiatePasswordResetAsync(model.Email);
                return Ok(new { message = "If the email exists in our database, you will receive instructions to reset your password." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing password reset request");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (model.NewPassword != model.ConfirmNewPassword)
                    return BadRequest("Passwords do not match.");

                await _passwordResetService.ResetPasswordAsync(model.Email, model.Token, model.NewPassword);
                return Ok(new { message = "Password reset successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when resetting password");
                return StatusCode(500, "An error occurred while resetting your password.");
            }
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _userService.ConfirmEmailAsync(model.Email, model.Token);
                return Ok(new { message = "Email confirmed successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming email");
                return StatusCode(500, "An error occurred while confirming your email.");
            }
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (model.NewPassword != model.ConfirmNewPassword)
                    return BadRequest("Passwords do not match.");

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var success = await _passwordService.UpdatePasswordAsync(userId, model.CurrentPassword, model.NewPassword);

                if (!success)
                    return BadRequest("Incorrect current password.");

                return Ok(new { message = "Password changed successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password");
                return StatusCode(500, "An error occurred while changing your password.");
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserProfileViewModel>> GetProfile()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var user = await _userService.GetUserByIdAsync(userId);

                if (user == null)
                    return NotFound();

                var profile = new UserProfileViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    IsConfirmed = user.IsConfirmed,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt
                };

                return Ok(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile");
                return StatusCode(500, "An error occurred while retrieving your profile.");
            }
        }
    }
}
