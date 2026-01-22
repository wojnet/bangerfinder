using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace bangerback.Core.DTOs
{
    public class LastFmResponse
    {
        [JsonPropertyName("similartracks")]
        public SimilarTracksContainer SimilarTracks { get; set; } // Changed to match Service usage
    }

    public class SimilarTracksContainer
    {
        [JsonPropertyName("track")]
        public List<LastFmTrack> Tracks { get; set; }
    }

    // MOVED OUTSIDE: Now accessible as bangerback.Core.DTOs.LastFmTrack
    public class LastFmTrack
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Mbid { get; set; }
        public LastFmArtist Artist { get; set; }
        public double? Match { get; set; }

        // ADD THIS: Capture the image list from the API
        [JsonPropertyName("image")]
        public List<LastFmImage> Images { get; set; }
    }

    public class LastFmImage
    {
        [JsonPropertyName("#text")] // Maps the specific Last.fm key for the URL
        public string Url { get; set; }
        public string Size { get; set; }
    }

    public class LastFmArtist
    {
        public string Name { get; set; }
    }

    public class LastFmSessionResponse
    {
        [JsonPropertyName("session")]
        public LastFmSession Session { get; set; } = null!;
    }

    public class LastFmSession
    {
        public string Name { get; set; } = null!;
        public string Key { get; set; } = null!;
        public int Subscriber { get; set; }
    }

    public class LastFmTopTracksResponse
    {
        [JsonPropertyName("toptracks")]
        public TopTracksContainer TopTracks { get; set; } = null!;
    }

    public class TopTracksContainer
    {
        [JsonPropertyName("track")]
        public List<LastFmTrack> Tracks { get; set; } = new();
    }

    public class LastFmRecentTracksResponse
    {
        [JsonPropertyName("recenttracks")]
        public RecentTracksContainer RecentTracks { get; set; } = null!;
    }

    public class RecentTracksContainer
    {
        [JsonPropertyName("track")]
        public List<LastFmTrack> Track { get; set; } = new();
    }
}