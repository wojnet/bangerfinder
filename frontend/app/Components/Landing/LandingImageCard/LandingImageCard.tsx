import Image from 'next/image';
import { FC } from 'react';

interface LandingImageCardProps {
  src: any,
  alt: string,
  width: number,
  title?: string,
  description?: string,
}

const LandingImageCard: FC<LandingImageCardProps> = ({
  src,
  alt,
  width,
  title,
  description,
}) => {
  return (
    <div
      style={{ width: width }}
      className="flex flex-col items-center gap-2"
    >
      <Image
        className="mb-3"
        src={src}
        alt={alt}
        width={width}
      />
      { title &&
        <h2 className="text-xl font-bold font-josefin-sans text-center">
          {title}
        </h2> 
      }
      { description &&
        <p className="text-center text-zinc-700 text-sm">
          {description}
        </p>
      }
    </div>
  );
}

export default LandingImageCard;