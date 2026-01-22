import { TSong } from "../../Types/Song";

interface WorkspaceProps {
  onGetRecommendations: () => void;
  recommendations: TSong[];
  isRefreshing?: boolean;
  isLinked: boolean; // 1. Added this prop
}

const Workspace = ({ onGetRecommendations, recommendations, isRefreshing, isLinked }: WorkspaceProps) => {
  return (
    <main className="flex-1 flex flex-col items-center p-10">
      <div className="flex flex-col items-center gap-4 mb-10">
        <h1 className="text-4xl font-bold font-josefin-sans">Your Fresh Bangers</h1>
        
        {/* 2. Lock the button if not linked */}
        <button 
          onClick={onGetRecommendations}
          disabled={isRefreshing || !isLinked}
          className={`bg-green-500 text-white font-bold py-3 px-8 rounded-full shadow-lg transition-all transform 
            ${(isRefreshing || !isLinked) 
              ? "opacity-50 cursor-not-allowed grayscale" 
              : "hover:bg-green-600 cursor-pointer active:scale-95 hover:scale-105"}`}
        >
          {!isLinked 
            ? "🔗 Connect Last.fm in Settings to Cook" 
            : isRefreshing 
              ? "🍳 Cooking..." 
              : "🔥 Cook New Recommendations"}
        </button>
      </div>

      {recommendations.length === 0 ? (
        <div className="text-zinc-400 italic text-center mt-20">
          {!isLinked 
            ? "Please link your Last.fm account first." 
            : "No bangers found yet. Click the button above to scan your soul."}
        </div>
      ) : (
        <div className="w-full max-w-[1200px] grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-8">
          {recommendations.map((song) => (
            <div key={song.songId} className="bg-white rounded-2xl overflow-hidden shadow-xl hover:shadow-2xl transition-shadow border border-zinc-200 group">
              <div className="relative h-48 w-full overflow-hidden">
                <img 
                  src={song.cover || "https://via.placeholder.com/300"} 
                  alt={song.title}
                  className="w-full h-full object-cover group-hover:scale-110 transition-transform duration-500"
                />
                <div className="absolute top-2 right-2 bg-black/70 text-white text-xs font-bold px-2 py-1 rounded-md">
                   {Math.round(song.score)}% Match
                </div>
              </div>
              <div className="p-5">
                <h3 className="font-bold text-lg truncate mb-1" title={song.title}>{song.title}</h3>
                <p className="text-zinc-500 text-sm mb-4">{song.artist}</p>
                <a href={song.externalId} target="_blank" rel="noopener noreferrer" className="inline-block w-full text-center py-2 bg-zinc-100 hover:bg-zinc-200 text-zinc-800 rounded-lg text-sm font-semibold transition-colors">
                  View on Last.fm
                </a>
              </div>
            </div>
          ))}
        </div>
      )}
    </main>
  );
};

export default Workspace;