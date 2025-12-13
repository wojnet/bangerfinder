using BCrypt.Net;
using FinalAppTemplate.Data;
using FinalAppTemplate.Models;
using FinalAppTemplate.Models.Attributes;
using FinalAppTemplate.Services;
using FinalAppTemplate.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalAppTemplate.Controllers
{
    public class UserCreationController : Controller
    {
        private readonly FinalAppTemplateDbContext DbAPI;
        private readonly EmailSender EmailSender;

        public UserCreationController(FinalAppTemplateDbContext dbapi, EmailSender emailSender)
        {
            DbAPI = dbapi;
            EmailSender = emailSender;
        }



        ///  POSTs --------------------------------------

        [SessionCheck]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // 1. Get the token
            var sessionToken = Request.Cookies["UserSession"];

            if (!string.IsNullOrEmpty(sessionToken))
            {
                // 2. Remove from Database
                var session = await DbAPI.Sessions.FirstOrDefaultAsync(s => s.Token == sessionToken);
                if (session != null)
                {
                    DbAPI.Sessions.Remove(session);
                    await DbAPI.SaveChangesAsync();
                }

                // 3. Remove from Browser
                Response.Cookies.Delete("UserSession");
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationVM model)
        {
            bool usernameExists = await DbAPI.Users.AnyAsync(u => u.Username == model.Username);
            if (usernameExists)
            {
                ModelState.AddModelError("Username", "This username is already taken.");
            }

            // Check if Email exists in DB
            bool emailExists = await DbAPI.Users.AnyAsync(u => u.Email == model.Email);
            if (emailExists)
            {
                ModelState.AddModelError("Email", "This email is already registered.");
            }

            if (ModelState.IsValid)
            {
                // 1. Generate Token
                var token = Guid.NewGuid().ToString();

                // 2. Create User (Verified = false)
                var userEntity = new User
                {
                    Name = model.Name,
                    Username = model.Username,
                    Email = model.Email,
                    CreatedAt = DateTime.Now,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    IsVerified = false,       // Default
                    VerificationToken = token // Save the token
                };

                try
                {

                    var verifyUrl = Url.Action("VerifyEmail", "UserCreation",
                                               new { token = token }, Request.Scheme); 

                    // 4. Send Email
                    await EmailSender.SendVerificationEmailAsync(model.Email, verifyUrl);

                    DbAPI.Users.Add(userEntity);
                    await DbAPI.SaveChangesAsync();

                    // 5. Redirect to an interstitial page (Check Your Email)
                    return RedirectToAction("CheckEmail");
                }
                catch (Exception ex)
                {
                    // Log the error
                    ModelState.AddModelError("", "Registration failed. " + ex.Message);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                // 1. Find user by Email OR Username
                var user = await DbAPI.Users
                    .FirstOrDefaultAsync(u => u.Username == model.LoginInput
                                           || u.Email == model.LoginInput);

                if (user != null)
                {
                    // 2. Verify Password
                    
                    bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

                    if (isPasswordCorrect)
                    {
                        // 3. Verify Email Status
                        if (!user.IsVerified)
                        {
                            ModelState.AddModelError("", "Please verify your email before logging in.");
                            return View(model);
                        }

                        // =========================================================
                        //  KILL THE OLD SESSION (Cleanup)
                        // =========================================================
                        var oldToken = Request.Cookies["UserSession"];
                        if (!string.IsNullOrEmpty(oldToken))
                        {
                            // Find the old session in the DB
                            var oldSession = await DbAPI.Sessions.FirstOrDefaultAsync(s => s.Token == oldToken);
                            if (oldSession != null)
                            {
                                // Delete it so it doesn't clutter the DB
                                DbAPI.Sessions.Remove(oldSession);
                                await DbAPI.SaveChangesAsync();
                            }
                        }

                        // =========================================================
                        // 4. CREATE SESSION (The New Logic)
                        // =========================================================

                        // A. Create the session record for the database
                        var newSession = new Session()
                        {
                            UserId = user.UserId,
                            Token = Guid.NewGuid().ToString(), // Generate unique token
                            CreatedAt = DateTime.Now,
                            ExpiresAt = DateTime.Now.AddDays(7) // Session valid for 7 days
                        };

                        // B. Save to Database
                        DbAPI.Sessions.Add(newSession);
                        await DbAPI.SaveChangesAsync();

                        // C. Give the Token to the user (Cookie)
                        Response.Cookies.Append("UserSession", newSession.Token, new CookieOptions
                        {
                            HttpOnly = true, // Javascript cannot read this (Security)
                            Expires = newSession.ExpiresAt,
                            IsEssential = true
                        });

                        return RedirectToAction("LoggedUser");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Invalid password.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User does not exist.");
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResendVerification(ResendVerificationVM model)
        {
            // 1. Validate Input
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 2. Find the User
            string email = model.Email; // Extract from VM

            // 2. Find the User
            var user = await DbAPI.Users.FirstOrDefaultAsync(u => u.Email == email);

            // 3. IF USER NOT FOUND (Security Best Practice)
            // We do not want to reveal if an email exists or not to hackers.
            // So if the user is null, we usually pretend we sent it.
            if (user == null)
            {
                // Redirect to "CheckEmail" (Fake Success)
                return RedirectToAction("CheckEmail");
            }

            // 4. CHECK IF ALREADY VERIFIED (Your Request)
            if (user.IsVerified)
            {
                // Add a specific error message so the user knows they don't need a token
                ModelState.AddModelError("Email", "This account is already verified. Please log in.");
                return View(); // Return the same view so they see the message
            }

            // 5. If User Exists AND is Not Verified -> Send New Token
            try
            {
                var token = Guid.NewGuid().ToString();
                user.VerificationToken = token;

                await DbAPI.SaveChangesAsync();

                var verifyUrl = Url.Action("VerifyEmail", "UserCreation",
                                           new { token = token }, Request.Scheme);

                await EmailSender.SendVerificationEmailAsync(user.Email, verifyUrl);

                return RedirectToAction("CheckEmail");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Could not send email. Please try again later.");
                return View();
            }
        }

        ///  GETs --------------------------------------

        
        [HttpGet]
        public async Task<IActionResult> MainPage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegistrationSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(); // You need to create a Register.cshtml view for this!
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [SessionCheck]
        [HttpGet]
        public async Task<IActionResult> LoggedUser()
        {
            // 1. Retrieve the user found by the filter
            var currentUser = HttpContext.Items["CurrentUser"] as User;

            return View(currentUser);
        }

        [HttpGet]
        public IActionResult ResendVerification()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CheckEmail()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            // 1. Sanity Check
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login");
            }

            // 2. Find the user with this specific token
            var user = await DbAPI.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);

            if (user == null)
            {
                // Token not found (or already used)
                ViewBag.Message = "Invalid or expired verification token.";
                return View("VerificationError");
            }

            // 3. Verify the User
            user.IsVerified = true;

            // 4. SECURITY: Consume the token
            // Set it to null so this link cannot be used a second time
            user.VerificationToken = null;

            await DbAPI.SaveChangesAsync();

            // 5. Success! Redirect to Login with a success message
            // TempData persists for exactly one redirect
            TempData["SuccessMessage"] = "Email verified successfully! You can now log in.";

            return RedirectToAction("Login");
        }

    }
}
