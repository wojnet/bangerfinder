import Image from 'next/image';
import { FC, MouseEventHandler } from 'react';

interface IButtonProps {
  text: string,
  onClick?: MouseEventHandler<HTMLDivElement>,
  isBigContrast?: boolean,
}

const Button: FC<IButtonProps> = ({
  text,
  onClick,
  isBigContrast,
}) => {
  return (
    <div
      onClick={onClick || undefined}
      className={`${isBigContrast ? "bg-zinc-900 text-white" : "bg-transparent text-zinc-900 shadow-[0_4px_0_#27272a]"} flex items-center gap-2 p-[6px] rounded-2xl shadow-[0_0_0_black] mb-5 outline-2 outline-zinc-900 ${isBigContrast ? "hover:outline-zinc-900 hover:bg-transparent hover:text-zinc-900 hover:shadow-[0_4px_0_black]" : "hover:outline-white hover:bg-zinc-900 hover:text-white hover:shadow"} transition cursor-pointer select-none`}
    >
      <p className="font-josefin-sans mx-[6px]">
        {text}
      </p>
    </div>
  );
}

export default Button;