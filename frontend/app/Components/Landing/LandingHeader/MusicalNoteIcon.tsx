import { FC } from 'react';
import Icon from "@assets/Icons/musical-note.svg";
import Image from 'next/image';

interface MusicalNoteIconProps {
  className?: string,
  width: number,
  height: number,
}

const MusicalNoteIcon: FC<MusicalNoteIconProps> = ({
  className,
  width,
  height,
}) => {
  return (
    <Image
      className={className}
      src={Icon}
      alt="Musical note icon"
      width={width}
      height={height}
    />
  );
}

export default MusicalNoteIcon;