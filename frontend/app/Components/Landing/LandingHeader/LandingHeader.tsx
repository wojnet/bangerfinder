import { FC } from 'react';
// import MusicalNoteIcon from './MusicalNoteIcon'; // Uncomment if you have this file

const LandingHeader: FC = () => {
  return (
    <header className="w-full max-w-[1000px] h-48 flex justify-center items-center gap-10 mx-auto">
      <h1 className="text-zinc-800 font-josefin-sans font-bold text-4xl select-none">
        BANGERFINDER
      </h1>
    </header>
  );
}

export default LandingHeader; // MUST HAVE THIS LINE