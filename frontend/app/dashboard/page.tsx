"use client"
import { useState } from "react";
import Workspace from "../Components/Dashboard/Workspace";
import Header from "../Components/UI/Header/Header";

interface IRecommendation {
  songId: number;
  spotifyId: number;
  title: string;
  artist: string;
  album: string;
  cover: string;
}

const Dashboard = () => {
  const [recommendations, setRecommendations] = useState<IRecommendation[]>([]);

  const getRecommendations = () => {
    const recommendations: IRecommendation[] = [
      {
        songId: 0,
        spotifyId: 123465,
        title: "No Idea",
        artist: "Don Toliver",
        album: "Heaven or Hell",
        cover: "https://upload.wikimedia.org/wikipedia/en/a/a0/Don_Toliver_-_Heaven_or_Hell.png"
      }
    ];

    setRecommendations(prev => [...prev, recommendations]);
  }

  return (
    <>
      <Header />
      <Workspace
        onGetRecommendations={getRecommendations}
      />
    </>
  );
}

export default Dashboard;