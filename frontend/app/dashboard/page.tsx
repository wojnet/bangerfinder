"use client"
import { useState, useEffect } from "react";
import Workspace from "../Components/Dashboard/Workspace";
import Header from "../Components/UI/Header/Header";
import SettingsModal from "../Components/Dashboard/Settings/SettingsModal"; // Ensure path is correct
import { TSong } from "../Types/Song";
import { refreshRecommendations, getLastFmStatus } from "../lib/api/user";
import { useAuth } from "../hooks/useAuth";

const Dashboard = () => {
  const [recommendations, setRecommendations] = useState<TSong[]>([]);
  const [isRefreshing, setIsRefreshing] = useState(false);
  const [isSettingsOpen, setIsSettingsOpen] = useState(false); // Controls the modal
  const [isLinked, setIsLinked] = useState(false);
  const { user, token } = useAuth();

  // Check connection status
  useEffect(() => {
    if (token) {
      getLastFmStatus(token).then(data => setIsLinked(data.isLinked));
    }
  }, [token, isSettingsOpen]);

  const getRecommendations = async () => {
    if (!token || !user) return;
    try {
      setIsRefreshing(true);
      const data = await refreshRecommendations(token, user.id);
      setRecommendations(data);
    } catch (err) {
      console.error(err);
    } finally {
      setIsRefreshing(false);
    }
  };

  return (
    <>
      {/* 3. Pass the setter to the Header */}
      <Header onOpenSettings={() => setIsSettingsOpen(true)} />
      
      <Workspace
        onGetRecommendations={getRecommendations}
        recommendations={recommendations}
        isRefreshing={isRefreshing}
        isLinked={isLinked}
      />

      {/* 4. Render the Modal */}
      <SettingsModal 
        isOpen={isSettingsOpen} 
        onClose={() => setIsSettingsOpen(false)} 
        token={token}
      />
    </>
  );
}

export default Dashboard;