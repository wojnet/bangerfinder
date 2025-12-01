import Image from 'next/image';
import { FC } from 'react';

interface ProfilePictureProps {
  src: string,
}

const ProfilePicture: FC<ProfilePictureProps> = ({
  src,
}) => {
  return (
    <Image
      className="rounded-full select-none pointer-events-none"
      src={src}
      alt="Profile picture"
      width={40}
      height={40}
    />
  );
}

export default ProfilePicture;