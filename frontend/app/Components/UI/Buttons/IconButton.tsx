import Image from 'next/image';
import { FC, MouseEventHandler } from 'react';

interface IconButtonProps {
  src: any,
  alt: string,
  iconHeight: number,
  text: string,
  onClick?: MouseEventHandler<HTMLDivElement>,
}

const IconButton: FC<IconButtonProps> = ({
  src,
  alt,
  iconHeight,
  text,
  onClick,
}) => {
  return (
    <div
      onClick={onClick || undefined}
      className="bg-zinc-900 text-white flex items-center gap-2 p-[6px] rounded-2xl shadow-[0_0_0_black] mb-5 hover:outline-2 hover:outline-zinc-900 hover:bg-transparent hover:text-zinc-900 hover:shadow-[0_4px_0_black] transition cursor-pointer"
    >
      <Image
        src={src}
        alt={alt}
        height={iconHeight}
      />
      <p className="font-josefin-sans mr-[6px]">
        {text}
      </p>
    </div>
  );
}

export default IconButton;