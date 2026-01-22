import Image from "next/image";
import LandingHeader from "./Components/Landing/LandingHeader/LandingHeader";
import ImageSkeleton from "@assets/Icons/image-skeleton.svg";
import LandingImageCard from "./Components/Landing/LandingImageCard/LandingImageCard";
import PenguinGif from "@assets/Images/penguin.gif";
import Link from "next/link";
import IconButton from "./Components/UI/Buttons/IconButton";
import AtIcon from "./Components/UI/Icons/AtIcon";
import LoginIcon from "./Components/UI/Icons/LoginIcon";
import { AuthRedirectIfAuthenticated } from "./Components/Authentication/AuthRedirect";

export default function Home() {
  return (
    <div className="w-full min-h-screen flex flex-col items-center bg-zinc-100 font-sans pb-20">
      <LandingHeader />
      <AuthRedirectIfAuthenticated path="/dashboard" />
      
      <div className="h-[450px] flex flex-col items-center gap-5">
        <section className="flex gap-5">
          <Link href="/register">
            <IconButton text="Register" icon={<AtIcon />} />
          </Link>
          <Link href="/login">
            <IconButton text="Log in" icon={<LoginIcon />} />
          </Link>
        </section>
        
        <p className="text-center max-w-[350px] font-josefin-sans text-zinc-700">
          💚 Dive into the deep rabbithole of undiscovered music!
        </p>
        
        {/* Next.js <Image> handles objects fine */}
        <Image
          className="rounded-2xl opacity-70 shadow-xl"
          src={PenguinGif}
          alt="Penguin listening to music"
          height={200}
        />
      </div>

      <div className="w-full max-w-[1000px] flex flex-col md:flex-row md:justify-center gap-10 mx-auto px-5">
        {/* FIX: Use .src because LandingImageCard expects a string */}
        <LandingImageCard
          src={ImageSkeleton.src} 
          alt="Last.fm Integration"
          width={200}
          title="Connect Last.fm"
          description="Sync your listening history instantly."
        />
        <LandingImageCard
          src={ImageSkeleton.src}
          alt="Taste Scanner"
          width={200}
          title="Scan Your Taste"
          description="Our engine dives deep into your scrobbles."
        />
        
        <LandingImageCard
          src={ImageSkeleton.src}
          alt="Discovery"
          width={200}
          title="Get New Bangers!"
          description="We cook fresh tracks daily based on your energy."
        />
      </div>
    </div>
  );
}