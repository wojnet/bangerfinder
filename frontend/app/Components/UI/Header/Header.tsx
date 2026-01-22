import { FC } from 'react';
import ProfilePicture from './ProfilePicture';
import ProfilePictureSkeleton from "@assets/Icons/profile-picture-skeleton.svg";
import BurgerMenu from './BurgerMenu';

interface HeaderProps {
  onOpenSettings: () => void;
}

const Header: FC<HeaderProps> = ({ onOpenSettings }) => {
  return (
    <header className="w-full max-w-[1000px] flex flex-col items-center mx-auto p-12">
      <section className="select-none sm:hidden">
        <h1 className="text-zinc-800 font-josefin-sans font-bold text-4xl select-none">
          DASHBOARD
        </h1>
        <p className="font-josefin-sans leading-3">BANGERFINDER</p>
      </section>
      
      <div className="w-full h-32 flex justify-between items-center">
        <section className="select-none hidden sm:block">
          <h1 className="text-zinc-800 font-josefin-sans font-bold text-4xl select-none">
            DASHBOARD
          </h1>
          <p className="font-josefin-sans leading-3">BANGERFINDER</p>
        </section>

        <section className="flex grow sm:justify-end items-center gap-5 sm:px-10 md:px-15 lg:px-20">
          <a className="font-josefin-sans select-none cursor-pointer hover:text-green-500 transition-colors">SAVED</a>
          <a className="font-josefin-sans select-none cursor-pointer hover:text-green-500 transition-colors">GITHUB</a>
        </section>

        <section className="flex items-center gap-5">
          {/* FIX: We now pass onOpenSettings into BurgerMenu */}
          <BurgerMenu onOpenSettings={onOpenSettings} />
          <ProfilePicture src={ProfilePictureSkeleton} />
        </section>
      </div>
    </header>
  );
}

export default Header;