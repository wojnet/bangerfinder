"use client"
import { FC, useState, useRef, useEffect } from 'react';
import { useRouter } from 'next/navigation';

interface BurgerMenuProps {
  onOpenSettings: () => void; // Prop passed from Header
}

const BurgerMenu: FC<BurgerMenuProps> = ({ onOpenSettings }) => {
  const [isOpen, setIsOpen] = useState(false);
  const menuRef = useRef<HTMLDivElement>(null);
  const router = useRouter();

  // Close menu when clicking outside
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (menuRef.current && !menuRef.current.contains(event.target as Node)) {
        setIsOpen(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  const handleLogout = async () => {
    const token = localStorage.getItem("auth_token");
    try {
      await fetch("http://localhost:5000/api/UserCreation/Logout", {
        method: "POST",
        headers: { 'Authorization': `Bearer ${token}` }
      });
    } finally {
      // Always clear local data and redirect
      localStorage.removeItem("auth_token");
      localStorage.removeItem("auth_user");
      router.push("/login");
    }
  };

  return (
    <div className="relative" ref={menuRef}>
      {/* Burger Icon */}
      <button 
        onClick={() => setIsOpen(!isOpen)}
        className="p-2 hover:bg-zinc-100 rounded-lg transition-colors cursor-pointer"
      >
        <div className="w-6 h-0.5 bg-zinc-800 mb-1"></div>
        <div className="w-6 h-0.5 bg-zinc-800 mb-1"></div>
        <div className="w-6 h-0.5 bg-zinc-800"></div>
      </button>

      {/* Dropdown Menu */}
      {isOpen && (
        <div className="absolute right-0 mt-2 w-48 bg-white border border-zinc-200 rounded-xl shadow-xl z-50 overflow-hidden">
          <button 
            onClick={() => {
              onOpenSettings(); // Trigger modal in Dashboard
              setIsOpen(false);
            }}
            className="w-full flex items-center gap-3 px-4 py-3 hover:bg-zinc-50 text-zinc-700 transition-colors text-left cursor-pointer"
          >
            <span className="text-lg">⚙️</span>
            <span className="font-josefin-sans">Settings</span>
          </button>

          <button 
            onClick={handleLogout}
            className="w-full flex items-center gap-3 px-4 py-3 hover:bg-red-50 text-red-600 transition-colors text-left border-t border-zinc-100 cursor-pointer"
          >
            <span className="text-lg">🚪</span>
            <span className="font-josefin-sans">Logout</span>
          </button>
        </div>
      )}
    </div>
  );
};

export default BurgerMenu;