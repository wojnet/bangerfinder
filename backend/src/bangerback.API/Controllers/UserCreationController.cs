using Backend.bangerback.Core.DTOs;
using Backend.bangerback.Core.Entities;
using Backend.bangerback.Infrastructure.Services;
using Backend.bangerback.Infrastructure.Data;
using FinalAppTemplate.Models.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalAppTemplate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserCreationController : ControllerBase
    {
        private readonly AppDbContext DbAPI;
        private readonly EmailSender EmailSender;
        private readonly IConfiguration Configuration;

        public UserCreationController(AppDbContext dbapi, EmailSender emailSender, IConfiguration configuration)
        {
            DbAPI = dbapi;
            EmailSender = emailSender;
            Configuration = configuration;
        }


        ///  POSTs --------------------------------------


        // POST: api/UserCreation/Logout
        [SessionCheck] // Uses the new Header-based check we wrote
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            // 1. Get the token from the Header (Standard API approach)
            string authHeader = Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();

                // 2. Remove from Database
                var session = await DbAPI.Sessions.FirstOrDefaultAsync(s => s.Token == token);
                if (session != null)
                {
                    DbAPI.Sessions.Remove(session);
                    await DbAPI.SaveChangesAsync();
                }
                return Ok(new { message = "Logged out successfully" });
            }

            return BadRequest(new { message = "No active session found" });
        }


        // POST: api/UserCreation/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            // [ApiController] automatically handles ModelState.IsValid, so we skip that check manually

            // 1. Check Duplicates
            bool usernameExists = await DbAPI.Users.AnyAsync(u => u.Username == model.Username);
            if (usernameExists)
            {
                return BadRequest(new { message = "This username is already taken." });
            }

            bool emailExists = await DbAPI.Users.AnyAsync(u => u.Email == model.Email);
            if (emailExists)
            {
                return BadRequest(new { message = "This email is already registered." });
            }

            // 2. Generate Token
            var token = Guid.NewGuid().ToString();

            // 3. Create User Entity
            var userEntity = new User
            {
                Name = model.Name,
                Username = model.Username,
                Email = model.Email,
                CreatedAt = DateTime.Now,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                IsVerified = false,
                VerificationToken = token
            };

            try
            {
                DbAPI.Users.Add(userEntity);
                await DbAPI.SaveChangesAsync();

                // 4. Send Email
                // CRITICAL: In a real React app, this URL usually points to the FRONTEND (localhost:3000/verify?token=...), 
                // which then calls the API. 
                // However, to keep your current flow working, we point to the API, which will then Redirect to the Frontend.
                var verifyUrl = Url.Action(nameof(VerifyEmail), "UserCreation", new { token = token }, Request.Scheme);

                await EmailSender.SendVerificationEmailAsync(model.Email, verifyUrl);

                return Ok(new { message = "Registration successful. Please check your email." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Registration failed: " + ex.Message });
            }
        }


        // POST: api/UserCreation/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model) // 1. Use DTO and [FromBody]
        {
            // 2. Return BadRequest for validation errors
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 3. Find User (Same logic)
            var user = await DbAPI.Users
                .FirstOrDefaultAsync(u => u.Username == model.LoginInput
                                       || u.Email == model.LoginInput);

            // Security Tip: Standardize error messages to avoid leaking which exists (User vs Password)
            // But for now, I'll keep your logic flow.
            if (user == null)
            {
                return Unauthorized(new { message = "User does not exist." });
            }

            // 4. Verify Password
            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

            if (!isPasswordCorrect)
            {
                return Unauthorized(new { message = "Invalid password." });
            }

            // 5. Verify Email
            if (!user.IsVerified)
            {
                return BadRequest(new { message = "Please verify your email before logging in." });
            }

            // =========================================================
            // KILL OLD SESSIONS (API Adaptation)
            // =========================================================
            // Since we don't have cookies, we can't target a "specific" old browser session easily.
            // OPTION: Clear ALL previous sessions for this user to ensure fresh start.
            // If you don't want this, just delete this block.
            var activeSessions = DbAPI.Sessions.Where(s => s.UserId == user.UserId);
            DbAPI.Sessions.RemoveRange(activeSessions);
            // =========================================================

            // 6. CREATE SESSION (Same Logic)
            var newSession = new Session()
            {
                UserId = user.UserId,
                Token = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddDays(7)
            };

            DbAPI.Sessions.Add(newSession);
            await DbAPI.SaveChangesAsync();

            // 7. THE BIG CHANGE: Return the Token in JSON
            // React will take this "token" and save it in localStorage or memory.
            return Ok(new
            {
                token = newSession.Token,
                expiration = newSession.ExpiresAt,
                message = "Login successful",
                // Send back safe user info so React can show "Welcome, [Name]"
                user = new
                {
                    id = user.UserId,
                    name = user.Name,
                    username = user.Username,
                    email = user.Email
                }
            });
        }


        // POST: api/UserCreation/ResendVerification
        [HttpPost("ResendVerification")]
        public async Task<IActionResult> ResendVerification([FromBody] ResendVerificationDto model)
        {
            // DTO Validation handled automatically
            var user = await DbAPI.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                // Fake success to prevent email enumeration
                return Ok(new { message = "If the email exists, a link has been sent." });
            }

            if (user.IsVerified)
            {
                return BadRequest(new { message = "Account already verified." });
            }

            // Send new token
            try
            {
                var token = Guid.NewGuid().ToString();
                user.VerificationToken = token;
                await DbAPI.SaveChangesAsync();

                var verifyUrl = Url.Action(nameof(VerifyEmail), "UserCreation", new { token = token }, Request.Scheme);
                await EmailSender.SendVerificationEmailAsync(user.Email, verifyUrl);

                return Ok(new { message = "Verification link sent." });
            }
            catch
            {
                return StatusCode(500, new { message = "Could not send email." });
            }
        }


        ///  GETs --------------------------------------


        // GET: api/UserCreation/LoggedUser (The Profile Endpoint)
        [SessionCheck]
        [HttpGet("LoggedUser")]
        public IActionResult LoggedUser()
        {
            // The [SessionCheck] filter puts the User object into HttpContext.Items
            if (HttpContext.Items["CurrentUser"] is User currentUser)
            {
                // Map to DTO to hide PasswordHash
                var userDto = new UserDto
                {
                    Id = currentUser.UserId,
                    Name = currentUser.Name,
                    Username = currentUser.Username,
                    Email = currentUser.Email
                };
                return Ok(userDto);
            }

            return Unauthorized();
        }


        // GET: api/UserCreation/VerifyEmail?token=xyz
        [HttpGet("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            if (string.IsNullOrEmpty(token)) return BadRequest("Token is missing");

            var user = await DbAPI.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);

            if (user == null)
            {
                return BadRequest("Invalid or expired token.");
            }

            user.IsVerified = true;
            user.VerificationToken = null; // Consume token
            await DbAPI.SaveChangesAsync();

            // MAGIC REDIRECT: Send the user to the React Frontend Login page
            // Assuming your React app runs on port 3000
            return Redirect("http://localhost:3000/login?verified=true");
        }

    }
}
