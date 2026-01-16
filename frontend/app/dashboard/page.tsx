"use client"
import { useState } from "react";
import { signIn, useSession } from "next-auth/react";
import Workspace from "../Components/Dashboard/Workspace";
import Header from "../Components/UI/Header/Header";
import Button from "../Components/UI/Buttons/Button";
import { TSong } from "../Types/Song";
import { fetchUserTopTracks } from "../Spotify/spotify";

const Dashboard = () => {
  const { data: session } = useSession();
  const [songs, setSongs] = useState<TSong[]>([]); // Renamed for clarity
  const [isLoading, setIsLoading] = useState(false);

  const handleGetTopTracks = async () => {
    const token = (session as any)?.accessToken;

    if (!token) {
      alert("Please login first!");
      return;
    }

    setIsLoading(true);

    try {
      // Direct call to get top tracks
      const myTopTracks = await fetchUserTopTracks(token);

      if (myTopTracks.length === 0) {
        alert("No top tracks found.");
      } else {
        setSongs(myTopTracks);
      }

    } catch (error) {
      console.error("Error:", error);
    } finally {
      setIsLoading(false);
    }
  }

  if (!session) {
    return (
      <div className="w-full min-h-screen flex flex-col items-center bg-zinc-100 font-sans">
        <Header />
        <div className="flex-1 flex flex-col items-center justify-center gap-6">
          <p className="text-xl font-bold text-zinc-800">
             Please log in to see your top tracks 🎧
          </p>
          <div onClick={() => signIn("spotify", { callbackUrl: "/dashboard" })}>
            <Button text="Login with Spotify 💚" isBigContrast />
          </div>
        </div>
      </div>
    );
  }

  return (
    <>
      <Header />
      <Workspace
        // We reuse the existing prop, even though the name implies recommendations
        onGetRecommendations={handleGetTopTracks} 
        recommendations={songs}
      />
      {isLoading && (
        <div className="w-full flex justify-center mt-4 mb-10">
           <p className="text-zinc-500 font-bold animate-pulse">
             Loading your favorites... 🎵
           </p>
        </div>
      )}
    </>
  );
}

export default Dashboard;