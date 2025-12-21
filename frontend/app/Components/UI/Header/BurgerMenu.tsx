"use client"
import { useRouter } from 'next/navigation';
import { FC, useEffect, useRef, useState } from 'react';

interface BurgerMenuProps {
  
}

const BurgerMenu: FC<BurgerMenuProps> = ({}) => {
  const router = useRouter();
  const [isOpen, setIsOpen] = useState<boolean>(false);
  const menuRef = useRef<HTMLDivElement | null>(null);
  const buttonRef = useRef<HTMLButtonElement | null>(null);

  const handleClickOutside = (e: MouseEvent) => {
    if (
      menuRef.current &&
      !menuRef.current.contains(e.target as Node) &&
      e.target !== buttonRef.current
    ) {
      setIsOpen(false);
    }
  }

  const handleLogout = () => {
    localStorage.removeItem("auth_token");
    localStorage.removeItem("auth_user");
    router.push("/");
  }

  useEffect(() => {
    document.addEventListener("mousedown", handleClickOutside);

    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    }
  }, []);

  return (
    <div className="w-4 h-8">
      <button
        ref={buttonRef}
        className="cursor-pointer select-none text-xl"
        onClick={() => setIsOpen(_ => !_)}
      >
        &#9776;
      </button>
      <div
        ref={menuRef}
        style={{
          transform: isOpen ? "scaleY(1)" : "scaleY(0)",
          opacity: isOpen ? 1 : 0,
        }}
        className="text-sm origin-top w-30 bg-zinc-200 flex flex-col gap-2 relative right-24 top-2 transition duration-300 p-2 border border-zinc-400 shadow-[0_2px_0_#9f9fa9] rounded-2xl"
      >
        <button
          className="cursor-pointer rounded-2xl hover:shadow-md p-1 transition"
        >
          <span>⚙️</span> Settings
        </button>
        <button
          className="cursor-pointer rounded-2xl hover:shadow-md p-1 transition"
          onClick={handleLogout}
        >
          <span>🚪</span> Logout
        </button>
      </div>
    </div>
  );
}

export default BurgerMenu;