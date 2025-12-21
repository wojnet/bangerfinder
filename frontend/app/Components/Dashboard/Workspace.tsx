import { FC, useEffect, useState } from 'react';
import SpotifyIcon from "@assets/Icons/spotify-color-svgrepo-com.svg";
import IconButton from '../UI/Buttons/IconButton';
import { TSong } from '@/app/Types/Song';
import RecommendationCard from './Recommendations/RecommendationCard/RecommendationCard';
import { useAuth } from '@/app/hooks/useAuth';
import { useRouter } from 'next/navigation';

interface WorkspaceProps {
  onGetRecommendations: () => void,
  recommendations: TSong[],
}

const Workspace: FC<WorkspaceProps> = ({
  onGetRecommendations,
  recommendations,
}) => {
  const router = useRouter();
  const { user, token, loading } = useAuth();

  useEffect(() => {
    if (!loading && !user) {
      router.push("/login");
    }
  }, [loading, user]);

  return (
    <div className="w-full flex flex-col items-center gap-5">
      {/* <IconButton
        icon={<SpotifyIcon/>}
        text="Get new recommendation"
        onClick={onGetRecommendations}
        isBigContrast
      /> */}
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