import { TSong } from "../Types/Song";

// ✅ REAL Spotify API URL
const SPOTIFY_API_BASE = "https://api.spotify.com/v1"; 

const getHeaders = (accessToken: string) => ({
  Authorization: `Bearer ${accessToken}`,
  "Content-Type": "application/json",
});

export const fetchUserTopTracks = async (accessToken: string): Promise<TSong[]> => {
  console.log("🔍 Service: fetchUserTopTracks called"); 

  const url = `${SPOTIFY_API_BASE}/me/top/tracks?limit=10&time_range=medium_term`;
  console.log("🔍 Service: Fetching URL:", url);

  try {
    const response = await fetch(url, { headers: getHeaders(accessToken) });

    if (!response.ok) {
        console.error("❌ Service: Spotify API Error:", response.status, await response.text());
        return [];
    }

    const data = await response.json();
    
    if (!data.items) {
        console.warn("⚠️ Service: 'items' array is missing");
        return [];
    }

    // Map the data and safely extract images
    const mappedSongs = data.items.map((track: any, index: number) => {
      // ✅ Fix for "Stock Photos": 
      // Safely check if album images exist. If not, use empty string (fallback will handle it).
      const imageArr = track.album?.images;
      const coverUrl = (imageArr && imageArr.length > 0) ? imageArr[0].url : "";

      return {
        songId: index,
        spotifyId: track.id,
        title: track.name,
        artist: track.artists.map((a: any) => a.name).join(", "),
        album: track.album.name,
        cover: coverUrl, 
      };
    });

    console.log(`✅ Service: Mapped ${mappedSongs.length} songs`);
    return mappedSongs;

  } catch (error) {
    console.error("❌ Service: Network Error:", error);
    return [];
  }
};

// ... (Keep the rest of the file if needed, or remove getRecommendationsFromSeeds if unused)