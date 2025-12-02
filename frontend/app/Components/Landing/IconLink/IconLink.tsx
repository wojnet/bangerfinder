import Image from 'next/image';
import { FC } from 'react';

interface IconLinkProps {
  src: any,
  alt: string,
  iconHeight: number,
  href: string,
  text: string,
}

const IconLink: FC<IconLinkProps> = ({
  src,
  alt,
  iconHeight,
  href,
  text
}) => {
  return (
    <a
      href={href}
      className="bg-zinc-900 text-white flex items-center gap-2 p-[6px] rounded-2xl shadow-[0_0_0_black] mb-5 hover:outline-2 hover:outline-zinc-900 hover:bg-transparent hover:text-zinc-900 hover:shadow-[0_4px_0_black] transition"
    >
      <Image
        src={src}
        alt={alt}
        height={iconHeight}
      />
      <p className="font-josefin-sans mr-[6px]">
        {text}
      </p>
    </a>
  );
}

export default IconLink;