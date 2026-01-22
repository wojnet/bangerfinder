import { FC, MouseEventHandler, ReactNode } from 'react';

interface IconButtonProps {
  icon?: ReactNode;
  text: string;
  onClick?: MouseEventHandler<HTMLButtonElement>; // Change to ButtonElement
  isBigContrast?: boolean;
  disabled?: boolean;
}

const IconButton: FC<IconButtonProps> = ({ icon, text, onClick, isBigContrast, disabled }) => {
  return (
    <button // Changed from div to button for accessibility and default behavior
      onClick={!disabled ? onClick : undefined}
      disabled={disabled}
      className={`flex items-center gap-2 px-4 py-2 rounded-lg transition-all 
        cursor-pointer active:scale-95 hover:opacity-80
        ${disabled ? "opacity-50 pointer-events-none grayscale cursor-not-allowed" : "cursor-pointer"}
        ${isBigContrast ? "bg-white text-black" : "bg-zinc-800 text-white"}`}
    >
      {icon && <span>{icon}</span>}
      <p className="font-josefin-sans">{text}</p>
    </button>
  );
};

export default IconButton;