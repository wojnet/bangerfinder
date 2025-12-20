"use client"
import { useState } from "react";
import Workspace from "../Components/Dashboard/Workspace";
import Header from "../Components/UI/Header/Header";
import { TSong } from "../Types/Song";

const Dashboard = () => {
  const [recommendations, setRecommendations] = useState<TSong[]>([]);

  const getRecommendations = () => {
    const recommendations: TSong[] = [
      {
        songId: 0,
        spotifyId: 123465,
        title: "No Idea",
        artist: "Don Toliver",
        album: "Heaven or Hell",
        cover: "https://upload.wikimedia.org/wikipedia/en/a/a0/Don_Toliver_-_Heaven_or_Hell.png"
      }
    ];

    setRecommendations(prev => [...prev, ...recommendations]);
  }

  return (
    <>
      <Header />
      <Workspace
        onGetRecommendations={getRecommendations}
        recommendations={recommendations}
      />
    </>
  );
}

export default Dashboard;