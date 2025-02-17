using EventosPro.Models;
using EventosPro.Services.Interfaces;
using EventosPro.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace EventosPro.Controllers
{
    /// <summary>
    /// Controller for managing user-related operations such as registration, login, and profile management.
    /// </summary>
    [Route("api/[controller]")]
    [EnableCors("SecurePolicy")] // CORS policy
    //[RequireHttps] // Force HTTPS
    [Produces("application/json")]
    [EnableRateLimiting("fixed")]
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

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="model">The registration data.</param>
        /// <returns>A response indicating the result of the registration.</returns>
        /// <response code="200">Registration completed successfully.</response>
        /// <response code="400">If the model state is invalid or passwords do not match.</response>
        /// <response code="409">If the user already exists.</response>
        /// <response code="500">If an error occurs during registration.</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                await _emailService.SendEmailConfirmationAsync(user.Email, user.ConfirmationToken, user.Name);

                return Ok(new { message = "Registration completed successfully. Please check your email to confirm your account." });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "User registration error: {Message}, StackTrace: {StackTrace}",
                    ex.Message,
                    ex.StackTrace);

                return StatusCode(500, "An error occurred while processing your registration.");
            }
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="model">The login credentials.</param>
        /// <returns>A response indicating the result of the login attempt.</returns>
        /// <response code="200">Login successful.</response>
        /// <response code="400">If the model state is invalid.</response>
        /// <response code="401">If the credentials are invalid.</response>
        /// <response code="500">If an error occurs during login.</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>A response indicating the result of the logout attempt.</returns>
        /// <response code="200">Logout successful.</response>
        /// <response code="500">If an error occurs during logout.</response>
        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Initiates a password reset for a user.
        /// </summary>
        /// <param name="model">The email address of the user.</param>
        /// <returns>A response indicating the result of the password reset request.</returns>
        /// <response code="200">Password reset initiated successfully.</response>
        /// <response code="400">If the model state is invalid.</response>
        /// <response code="500">If an error occurs during the password reset process.</response>
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Resets a user's password.
        /// </summary>
        /// <param name="model">The password reset data.</param>
        /// <returns>A response indicating the result of the password reset.</returns>
        /// <response code="200">Password reset successfully.</response>
        /// <response code="400">If the model state is invalid or passwords do not match.</response>
        /// <response code="500">If an error occurs during the password reset process.</response>
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Confirms a user's email address.
        /// </summary>
        /// <param name="model">The email confirmation data.</param>
        /// <returns>A response indicating the result of the email confirmation.</returns>
        /// <response code="200">Email confirmed successfully.</response>
        /// <response code="400">If the model state is invalid or the token is invalid.</response>
        /// <response code="500">If an error occurs during the email confirmation process.</response>
        [HttpPost("confirm-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Changes the password of the current user.
        /// </summary>
        /// <param name="model">The password change data.</param>
        /// <returns>A response indicating the result of the password change.</returns>
        /// <response code="200">Password changed successfully.</response>
        /// <response code="400">If the model state is invalid or the current password is incorrect.</response>
        /// <response code="500">If an error occurs during the password change process.</response>
        [Authorize]
        [HttpPost("change-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Retrieves the profile of the current user.
        /// </summary>
        /// <returns>The user's profile data.</returns>
        /// <response code="200">Profile retrieved successfully.</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="500">If an error occurs during the profile retrieval process.</response>
        [Authorize]
        [HttpGet("profile")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserProfileViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
