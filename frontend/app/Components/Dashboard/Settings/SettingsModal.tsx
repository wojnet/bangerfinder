"use client"
import { useState, useEffect } from "react";
import IconButton from "../../UI/Buttons/IconButton";
import { initiateLastFmConnection } from "../../../lib/api/user";

interface SettingsModalProps {
  isOpen: boolean;
  onClose: () => void;
  token: string | null;
}

const SettingsModal = ({ isOpen, onClose, token }: SettingsModalProps) => {
  const [isLinked, setIsLinked] = useState(false);
  const [lastFmUsername, setLastFmUsername] = useState("");

  // 1. Check status whenever modal opens
  useEffect(() => {
    if (isOpen && token) {
      fetch("http://localhost:5000/api/LastFmAuth/status", {
        headers: { 'Authorization': `Bearer ${token}` }
      })
      .then(res => res.json())
      .then(data => {
        setIsLinked(data.isLinked);
        setLastFmUsername(data.lastFmUsername);
      });
    }
  }, [isOpen, token]);

  // 2. DISCONNECT LOGIC
  const handleDisconnect = async () => {
    if (!token) return;
    const res = await fetch("http://localhost:5000/api/LastFmAuth/disconnect", {
      method: "POST",
      headers: { 'Authorization': `Bearer ${token}` }
    });
    
    if (res.ok) {
      setIsLinked(false);
      setLastFmUsername("");
      alert("Account disconnected. You can now link a different one!");
    }
  };

  // 3. CONNECT LOGIC (Calls the URL generator we built)
  const handleConnect = async () => {
    if (!token) return;
    const res = await fetch("http://localhost:5000/api/UserCreation/lastfm-connect-url", {
        headers: { 'Authorization': `Bearer ${token}` }
    });
    const { url } = await res.json();
    window.location.href = url;
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
      <div className="bg-white p-8 rounded-2xl shadow-2xl w-full max-w-md">
        <h2 className="text-2xl font-bold mb-6 font-josefin-sans">Settings</h2>
        
        <div className="space-y-6">
          <section>
            <h3 className="text-sm font-semibold text-zinc-400 uppercase tracking-wider mb-2">Music Profile</h3>
            
            {isLinked ? (
              <div className="flex flex-col gap-3">
                <div className="p-4 bg-zinc-100 rounded-xl flex justify-between items-center">
                  <span>Linked to: <strong>{lastFmUsername}</strong></span>
                  <span className="text-green-500 text-xs font-bold">CONNECTED</span>
                </div>
                {/* DISCONNECT BUTTON */}
                <button 
                  onClick={handleDisconnect}
                  className="w-full py-2 border-2 border-red-500 text-red-500 font-bold rounded-xl hover:bg-red-50 transition-colors"
                >
                  Disconnect Account
                </button>
              </div>
            ) : (
              <div className="flex flex-col gap-3">
                <p className="text-sm text-zinc-500">Connect your Last.fm to start cooking bangers.</p>
                {/* CONNECT BUTTON */}
                <IconButton 
                  text="Connect Last.fm" 
                  onClick={handleConnect}
                  isBigContrast
                />
              </div>
            )}
          </section>
        </div>

        <button onClick={onClose} className="mt-8 w-full py-3 bg-zinc-800 text-white rounded-xl font-bold">
          Close
        </button>
      </div>
    </div>
  );
};

export default SettingsModal;