using Backend.bangerback.Infrastructure.Data;
using bangerback.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bangerback.Core.DTOs;

namespace bangerback.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly LastFmService _lastFmService; // Declare the service

        public RecommendationController(AppDbContext context, LastFmService lastFmService)
        {
            _context = context;
            _lastFmService = lastFmService; // Initialize via constructor
        }

        [HttpGet("my-bangers")]
        public async Task<IActionResult> GetMyBangers([FromQuery] int userId)
        {
            var recommendations = await _context.Recommendations
                .Where(r => r.UserId == userId)
                .Include(r => r.Song)
                .OrderByDescending(r => r.GeneratedAt)
                .Select(r => new {
                    SongId = r.Song.SongId,
                    ExternalId = r.Song.ExternalId,
                    Mbid = r.Song.Mbid,
                    Title = r.Song.Title,
                    Artist = r.Song.Artist,
                    Album = r.Song.Album ?? "Unknown Album",
                    // FIX: Use the actual DB column instead of the placeholder!
                    Cover = r.Song.Cover,
                    Score = r.Score
                })
                .ToListAsync();

            return Ok(recommendations);
        }




        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshBangers([FromBody] RefreshRequest request)
        {
            try
            {
                // Use request.UserId
                await _lastFmService.RefreshUserRecommendationsAsync(request.UserId);

                // Fetch new data to return
                // Inside the RefreshBangers method
                var freshBangers = await _context.Recommendations
                    .Where(r => r.UserId == request.UserId)
                    .Include(r => r.Song)
                    .OrderByDescending(r => r.GeneratedAt)
                    .Take(15)
                    .Select(r => new {
                        SongId = r.Song.SongId,
                        ExternalId = r.Song.ExternalId,
                        Title = r.Song.Title,
                        Artist = r.Song.Artist,
                        Score = r.Score,
                        // FIX: Return the actual URL from the database instead of "via.placeholder"
                        Cover = r.Song.Cover
                    })
                    .ToListAsync();

                return Ok(freshBangers);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: {ex.Message}");
                if (ex.InnerException != null) Console.WriteLine($"INNER: {ex.InnerException.Message}");

                return BadRequest(new { message = ex.Message });
            }
        }
    }
}