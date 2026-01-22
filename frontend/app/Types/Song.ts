export type TSong = {
  songId: number;
  externalId: string; 
  mbid?: string;      
  title: string;
  artist: string;
  album: string;
  cover: string;
  score: number; 
};