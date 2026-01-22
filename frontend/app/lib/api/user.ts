import { LoginDto, RegisterDto } from "@/app/Types/User";

// Look for where you defined API_URL earlier
const API_URL = "http://localhost:5000/api/UserCreation"; // process.env.NEXT_PUBLIC_API_URL || "http://localhost:5000/api/UserCreation"; - for docker
const LASTFM_URL = "http://localhost:5000/api/LastFmAuth"; 

// 1. Redirect to Last.fm for the Handshake
// app/lib/api/user.ts
export const initiateLastFmConnection = async (token: string) => {
  const res = await fetch("http://localhost:5000/api/UserCreation/lastfm-connect-url", {
    headers: { 'Authorization': `Bearer ${token}` }
  });
  
  const { url } = await res.json();
  // Redirect the user to Last.fm
  window.location.href = url;
};

// 2. Fetch Bangers from the Backend
export const refreshRecommendations = async (token: string, userId: number) => {
  const res = await fetch("http://localhost:5000/api/Recommendation/refresh", {
    method: "POST",
    headers: { 
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    },
    // FIX: Must be an object to match [FromBody] RefreshRequest in C#
    body: JSON.stringify({ userId }) 
  });
  
  if (!res.ok) throw new Error("Failed to cook fresh bangers");
  return await res.json();
};

// 3. Status Check for Settings
export const getLastFmStatus = async (token: string) => {
  const res = await fetch("http://localhost:5000/api/LastFmAuth/status", {
    method: "GET",
    headers: { 'Authorization': `Bearer ${token}` }
  });
  
  if (!res.ok) return { isLinked: false };
  return await res.json();
};

export const registerUser = async (data: RegisterDto) => {
  const res = await fetch(`${API_URL}/Register`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(data),
  });

  const json = await res.json();

  if (!res.ok)
    throw new Error(json.message || "Registration failed");

  return json;
}

export const loginUser = async (data: LoginDto) => {
  const res = await fetch(`${API_URL}/Login`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(data),
  });

  const json = await res.json();

  if (!res.ok)
    throw new Error(json.message || "Logging in failed");

  return json;
} 