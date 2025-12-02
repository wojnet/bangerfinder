import { FC } from 'react';
import SpotifyIcon from "@assets/Icons/spotify-color-svgrepo-com.svg";
import IconButton from '../UI/Buttons/IconButton';

interface WorkspaceProps {
  onGetRecommendations: () => void,
}

const Workspace: FC<WorkspaceProps> = ({
  onGetRecommendations,
}) => {
  return (
    <div className="w-full flex flex-col items-center">
      <IconButton
        src={SpotifyIcon}
        alt="Icon"
        iconHeight={24}
        text="Get new recommendation"
        onClick={onGetRecommendations}
      />
    </div>
  );
}

export default Workspace;