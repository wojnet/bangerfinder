import Image from 'next/image';
import { FC, MouseEventHandler } from 'react';

interface IconButtonProps {
  icon: any,
  text: string,
  onClick?: MouseEventHandler<HTMLDivElement>,
  isBigContrast?: boolean,
}

const IconButton: FC<IconButtonProps> = ({
  icon,
  text,
  onClick,
  isBigContrast,
}) => {
  return (
    <div
      onClick={onClick || undefined}
      className={`${isBigContrast ? "bg-zinc-900 text-white" : "bg-transparent text-zinc-900 shadow-[0_4px_0_#27272a]"} flex items-center gap-2 p-[6px] rounded-2xl shadow-[0_0_0_black] mb-5 outline-2 outline-zinc-900 ${isBigContrast ? "hover:outline-zinc-900 hover:bg-transparent hover:text-zinc-900 hover:shadow-[0_4px_0_black]" : "hover:outline-white hover:bg-zinc-900 hover:text-white hover:shadow"} transition cursor-pointer select-none`}
    >
      { icon }
      <p className="font-josefin-sans mr-[6px]">
        {text}
      </p>
    </div>
  );
}

export default IconButton;