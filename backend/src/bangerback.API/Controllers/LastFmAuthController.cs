using Backend.bangerback.Core.Entities;
using Backend.bangerback.Infrastructure.Data;
using bangerback.Infrastructure.Services;
using FinalAppTemplate.Models.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/[controller]")]
public class LastFmAuthController : ControllerBase
{
    private readonly LastFmService _lastFmService;
    private readonly AppDbContext _context; // Declared here...

    // FIX: You must add AppDbContext to the parameters here!
    public LastFmAuthController(LastFmService lastFmService, AppDbContext context)
    {
        _lastFmService = lastFmService;
        _context = context; // ...but it was never assigned here!
    }

    [HttpGet("callback")]
    [SessionCheck]
    public async Task<IActionResult> Callback(string token)
    {
        if (HttpContext.Items["CurrentUser"] is User currentUser)
        {
            await _lastFmService.SaveSessionAsync(currentUser.UserId, token);
            // DO NOT REDIRECT HERE. Return JSON so React knows it's done.
            return Ok(new { message = "Session saved successfully" });
        }

        return Unauthorized("You must be logged in to link Last.fm");
    }

    [HttpGet("status")]
    [SessionCheck]
    public IActionResult GetStatus()
    {
        if (HttpContext.Items["CurrentUser"] is User currentUser)
        {
            // If the key exists in our DB, they are connected
            bool isLinked = !string.IsNullOrEmpty(currentUser.LastFmSessionKey);
            return Ok(new
            {
                isLinked = isLinked,
                username = currentUser.LastFmUsername
            });
        }
        return Unauthorized();
    }

    [HttpPost("disconnect")]
    [SessionCheck]
    public async Task<IActionResult> Disconnect()
    {
        if (HttpContext.Items["CurrentUser"] is User currentUser)
        {
            var user = await _context.Users.FindAsync(currentUser.UserId);
            if (user != null)
            {
                // Wipe the credentials
                user.LastFmUsername = null;
                user.LastFmSessionKey = null;
                user.LastUpdatedAt = DateTime.UtcNow;

                // Optional: Wipe their old recommendations too for a fresh start
                var oldRecs = _context.Recommendations.Where(r => r.UserId == user.UserId);
                _context.Recommendations.RemoveRange(oldRecs);

                await _context.SaveChangesAsync();
                return Ok(new { message = "Disconnected successfully" });
            }
        }
        return Unauthorized();
    }
}