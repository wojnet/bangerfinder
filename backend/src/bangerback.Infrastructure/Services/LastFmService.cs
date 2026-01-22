using Backend.bangerback.Core.Entities;
using Backend.bangerback.Infrastructure.Data;
using bangerback.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;

namespace bangerback.Infrastructure.Services;

public class LastFmService
{
    private readonly AppDbContext _context;
    private readonly HttpClient _http;
    private readonly string _apiKey;
    private readonly string _apiSecret;

    public LastFmService(IConfiguration config, HttpClient http, AppDbContext context)
    {
        _apiKey = config["LastFm:ApiKey"] ?? "";
        _apiSecret = config["LastFm:ApiSecret"] ?? "";

        if (string.IsNullOrEmpty(_apiKey))
        {
            Console.WriteLine("CRITICAL: Last.fm API Key is MISSING from appsettings.json!");
        }
        //Console.WriteLine(_apiKey);
        //Console.WriteLine(_apiSecret);

        _http = http;
        _context = context;
    }

    public async Task SaveSessionAsync(int userId, string token)
    {
        var parameters = new Dictionary<string, string>
    {
        { "api_key", _apiKey },
        { "method", "auth.getSession" },
        { "token", token }
    };

        var sig = GenerateApiSignature(parameters);
        var url = $"http://ws.audioscrobbler.com/2.0/?method=auth.getSession&token={token}&api_key={_apiKey}&api_sig={sig}&format=json";

        var response = await _http.GetFromJsonAsync<LastFmSessionResponse>(url);
        if (response?.Session != null)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.LastFmUsername = response.Session.Name;
                user.LastFmSessionKey = response.Session.Key;
                user.LastUpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }

    private string GenerateApiSignature(Dictionary<string, string> parameters)
    {
        var sortedParams = parameters.OrderBy(p => p.Key);
        var sb = new StringBuilder();
        foreach (var p in sortedParams)
        {
            sb.Append(p.Key).Append(p.Value);
        }
        sb.Append(_apiSecret); // This is the secret from your appsettings.json

        using var md5 = MD5.Create();
        var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
        return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }


    // This is how we get the 'Bangers' (Similar Tracks)
    public async Task RefreshUserRecommendationsAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null || string.IsNullOrEmpty(user.LastFmUsername)) return;

        // REVERTED: Using getTopTracks seed as requested
        var url = $"http://ws.audioscrobbler.com/2.0/?method=user.getTopTracks&user={user.LastFmUsername}&api_key={_apiKey}&limit=5&format=json";

        var response = await _http.GetFromJsonAsync<LastFmTopTracksResponse>(url);
        var tracksToProcess = response?.TopTracks?.Tracks;

        if (tracksToProcess == null || !tracksToProcess.Any())
        {
            var chartUrl = $"http://ws.audioscrobbler.com/2.0/?method=chart.getTopTracks&api_key={_apiKey}&limit=5&format=json";
            var chartResponse = await _http.GetFromJsonAsync<LastFmTopTracksResponse>(chartUrl);
            tracksToProcess = chartResponse?.TopTracks?.Tracks;
        }

        foreach (var track in tracksToProcess)
        {
            // Safety check to prevent the 'stringToEscape' crash
            if (string.IsNullOrEmpty(track.Name) || string.IsNullOrEmpty(track.Artist?.Name)) continue;
            await FetchAndSaveSimilarTracks(userId, track.Name, track.Artist.Name);
        }
    }


    private async Task FetchAndSaveSimilarTracks(int userId, string trackName, string artistName)
    {
        try
        {
            var url = $"http://ws.audioscrobbler.com/2.0/?method=track.getSimilar" +
                      $"&artist={Uri.EscapeDataString(artistName)}&track={Uri.EscapeDataString(trackName)}" +
                      $"&api_key={_apiKey}&limit=5&format=json";

            // RESTORED: Raw JSON logs for your terminal
            var jsonString = await _http.GetStringAsync(url);
            Console.WriteLine($"\n--- DEBUG: RAW JSON FOR '{trackName}' ---");
            Console.WriteLine(jsonString);
            Console.WriteLine("------------------------------------------\n");

            var response = System.Text.Json.JsonSerializer.Deserialize<LastFmResponse>(jsonString, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // FIXED: Using SimilarTracks.Tracks to match your DTO names
            if (response?.SimilarTracks?.Tracks == null || !response.SimilarTracks.Tracks.Any()) return;

            foreach (var lfmTrack in response.SimilarTracks.Tracks)
            {
                var song = await GetOrCreateSong(lfmTrack);
                var exists = await _context.Recommendations.AnyAsync(r => r.UserId == userId && r.SongId == song.SongId);
                if (!exists)
                {
                    _context.Recommendations.Add(new Recommendation
                    {
                        UserId = userId,
                        SongId = song.SongId,
                        GeneratedAt = DateTime.UtcNow,
                        Score = (decimal)(lfmTrack.Match ?? 0) * 100
                    });
                }
            }
            await _context.SaveChangesAsync();
            Console.WriteLine($">>> SUCCESS: Saved bangers for {trackName}");
        }
        catch (Exception ex) { Console.WriteLine($"FATAL ERROR: {ex.Message}"); }
    }

    private async Task<Song> GetOrCreateSong(LastFmTrack lfmTrack)
    {
        var song = await _context.Songs.FirstOrDefaultAsync(s => s.ExternalId == lfmTrack.Url);
        if (song == null)
        {
            // Get the extralarge image URL from the API
            var coverUrl = lfmTrack.Images?.FirstOrDefault(i => i.Size == "extralarge")?.Url
                           ?? "https://via.placeholder.com/300";

            song = new Song
            {
                ExternalId = lfmTrack.Url,
                Mbid = lfmTrack.Mbid,
                Title = lfmTrack.Name,
                Artist = lfmTrack.Artist.Name,
                Album = "Unknown Album",
                Cover = coverUrl // SAVE to the DB column
            };
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();
        }
        return song;
    }
}