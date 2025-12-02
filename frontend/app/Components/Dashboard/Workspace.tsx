import { FC } from 'react';
import SpotifyIcon from "@assets/Icons/spotify-color-svgrepo-com.svg";
import IconButton from '../UI/Buttons/IconButton';
import { TSong } from '@/app/Types/Song';
import RecommendationCard from './Recommendations/RecommendationCard/RecommendationCard';

interface WorkspaceProps {
  onGetRecommendations: () => void,
  recommendations: TSong[],
}

const Workspace: FC<WorkspaceProps> = ({
  onGetRecommendations,
  recommendations,
}) => {
  return (
    <div className="w-full flex flex-col items-center gap-5">
      <IconButton
        src={SpotifyIcon}
        alt="Icon"
        iconHeight={24}
        text="Get new recommendation"
        onClick={onGetRecommendations}
        isBigContrast
      />
      <section>
        { recommendations.map((song, i) => 
          <RecommendationCard
            key={i}
            song={song}
          />
        ) }
      </section>
    </div>
  );
}

export default Workspace;