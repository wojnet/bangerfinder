"use client"
import { FC, InputHTMLAttributes, MouseEventHandler, useRef, useState } from "react";

interface PasswordInputProps extends InputHTMLAttributes<HTMLInputElement> {
  onEnterPressed?: () => void;
}

const PasswordInput: FC<PasswordInputProps> = ({ onEnterPressed, ...props }) => {
  const inputRef = useRef<HTMLDivElement | null>(null);
  const [isHidden, setIsHidden] = useState<boolean>(true);

  const onButtonClick: MouseEventHandler<HTMLButtonElement> = (event) => {
    event.preventDefault();
    setIsHidden(prev => !prev);
  }

  return (
    <div
      className="w-full h-8 flex items-center justify-between gap-2 shadow-[0_0_0_black] focus-within:shadow-[0_2px_0_2px_black] outline-zinc-500 outline rounded-full focus-within:outline-2 focus-within:outline-zinc-900 px-3"
      ref={inputRef}
    >
      <input
        type={ isHidden ? "password" : "text" }
        className="w-full h-full focus:outline-none" {...props}
        onKeyDown={(e) => {
          if (onEnterPressed || e.key === "Enter") {
            e.preventDefault();
            onEnterPressed?.();
          }
        }}
      />
      <button
        className="cursor-pointer text-shadow-[0_0_2px_#0004]"
        onClick={onButtonClick}
      >
        { isHidden ? "🗝️" : "👀" }
      </button>
    </div>
  );
};

export default PasswordInput;