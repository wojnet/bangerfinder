"use client"
import { FC, InputHTMLAttributes } from "react";

interface SubmitProps extends InputHTMLAttributes<HTMLInputElement> {
  isBigContrast?: boolean;
  label: string;
  onClick?: (e: any) => void;
}

const Submit: FC<SubmitProps> = ({ isBigContrast, label, onClick, ...props }) => {
  return (
    <input
      className={`${isBigContrast ? "bg-zinc-900 text-white" : "bg-transparent text-zinc-900 shadow-[0_4px_0_#27272a]"} flex items-center gap-2 p-[6px] rounded-2xl shadow-[0_0_0_black] mb-5 outline-2 outline-zinc-900 ${isBigContrast ? "hover:outline-zinc-900 hover:bg-transparent hover:text-zinc-900 hover:shadow-[0_4px_0_black]" : "hover:outline-white hover:bg-zinc-900 hover:text-white hover:shadow"} transition cursor-pointer select-none px-2 disabled:pointer-events-none disabled:animate-pulse`}
      type="submit"
      value={label}
      {...props}
      onClick={(e) => {
        e.preventDefault();
        onClick?.(e);
      }}
    />
  );
};

export default Submit;