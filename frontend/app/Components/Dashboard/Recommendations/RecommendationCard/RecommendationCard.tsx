import Button from '@/app/Components/UI/Buttons/Button';
import { TSong } from '@/app/Types/Song';
import Image from 'next/image';
import { FC } from 'react';
// Import a placeholder icon from your assets
import MusicalNoteIcon from "@assets/Icons/musical-note.svg"; 

interface RecommendationCardProps {
  song: TSong,
}

const RecommendationCard: FC<RecommendationCardProps> = ({
  song,
}) => {
  // Logic: Use the song cover if it exists, otherwise use the placeholder
  const hasCover = song.cover && song.cover.length > 0;
  const imageSrc = hasCover ? song.cover : MusicalNoteIcon;

  return (
    <div className="w-48 font-josefin-sans flex flex-col gap-1">
      <div className="w-full flex flex-col items-center border-2 border-zinc-800 rounded-2xl shadow-[0_2px_0_#27272a] px-2 py-4 bg-white">
        <p className="text-xl font-bold text-zinc-800 text-center truncate w-full" title={song.title}>
          {song.title}
        </p>
        <p className="italic text-zinc-700 mb-2 text-sm text-center truncate w-full">
          {song.artist}
        </p>
        
        <div className="relative w-32 h-32 mb-2">
           <Image
             className={`border-2 border-zinc-800 rounded-2xl shadow-[0_2px_0_#27272a] select-none object-cover ${!hasCover ? "p-4 bg-zinc-100" : ""}`}
             src={imageSrc}
             alt={`${song.artist} ${song.title} album cover`}
             fill // Use fill to adapt to the container
             sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw"
           />
        </div>

        <p className="text-xs text-zinc-500 text-center truncate w-full">
          {song.album}
        </p>
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