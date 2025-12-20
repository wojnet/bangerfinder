import Button from '@/app/Components/UI/Buttons/Button';
import { TSong } from '@/app/Types/Song';
import Image from 'next/image';
import { FC } from 'react';

interface RecommendationCardProps {
  song: TSong,
}

const RecommendationCard: FC<RecommendationCardProps> = ({
  song,
}) => {
  return (
    <div className="w-48 font-josefin-sans flex flex-col gap-1">
      <div
        className="w-full flex flex-col items-center border-2 border-zinc-800 rounded-2xl shadow-[0_2px_0_#27272a] px-2 py-4"
      >
        <p className="text-xl font-bold text-zinc-800">{song.title}</p>
        <p className="italic text-zinc-700 mb-1">{song.artist}</p>
        <Image
          className="border-2 border-zinc-800 rounded-2xl shadow-[0_2px_0_#27272a] mb-2 select-none"
          src={song.cover}
          alt={`${song.artist} ${song.title} album cover`}
          width={128}
          height={128}
        />
        <p className="text-sm text-zinc-700">{song.album}</p>
      </div>
      <div className="w-full flex justify-center items-center gap-8 p-2">
        <Button
          text="Add 💚"
          isBigContrast={false}
        />
      </div>
    </div>
  );
}

export default RecommendationCard;