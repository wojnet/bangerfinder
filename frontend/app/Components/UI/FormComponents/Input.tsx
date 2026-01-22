"use client"
import { FC, InputHTMLAttributes } from "react";

interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
  onEnterPressed?: () => void;
}

const Input: FC<InputProps> = ({ onEnterPressed, ...props }) => {
  return (
    <div
      className="w-full h-8 flex items-center justify-between gap-2 shadow-[0_0_0_black] focus-within:shadow-[0_2px_0_2px_black] outline-zinc-500 outline rounded-full focus-within:outline-2 focus-within:outline-zinc-900 px-3"
    >
      <input
        className="w-full h-full focus:outline-none"
        {...props}
        onKeyDown={(e) => {
          if (onEnterPressed || e.key === "Enter") {
            e.preventDefault();
            onEnterPressed?.();
          }
        }}
      />
    </div>
  );
};

export default Input;