import Image from "next/image";
import LandingHeader from "./Components/Landing/LandingHeader/LandingHeader";
import ImageSkeleton from "@assets/Icons/image-skeleton.svg";
import LandingImageCard from "./Components/Landing/LandingImageCard/LandingImageCard";
import PenguinGif from "@assets/Images/penguin.gif"
import SpotifyIcon from "@assets/Icons/spotify-color-svgrepo-com.svg"
import IconLink from "./Components/Landing/IconLink/IconLink";

export default function Home() {
  return (
    <div className="w-full min-h-screen flex flex-col items-center bg-zinc-100 font-sans">
      <LandingHeader />
      <div className="h-[450px] flex flex-col items-center gap-5">
        <p className="text-center max-w-[350px]">💚 Dive into the deep rabbithole of undiscovered music! Click the button below:</p>
        <IconLink
          src={SpotifyIcon}
          alt="Spotify icon"
          iconHeight={24}
          href="/dashboard"
        />
        <Image
          className="rounded-2xl opacity-70"
          src={PenguinGif}
          alt="Penguin listening to music"
          height={200}
        />
      </div>
      <div className="w-full max-w-[1000px] flex flex-col md:flex-row md:justify-center gap-10 mx-auto">
        <LandingImageCard
          src={ImageSkeleton}
          alt="Image skeleton"
          width={200}
          title="Login with Spotify"
          description="Connect your account and let the app peek into your music soul. Instant access, instant vibes."
        />
        <LandingImageCard
          src={ImageSkeleton}
          alt="Image skeleton"
          width={200}
          title="Upload a playlist"
          description="Drop in your fav mix and let the system scan it like a music detective. No hassle, just pure data magic."
        />
        <LandingImageCard
          src={ImageSkeleton}
          alt="Image skeleton"
          width={200}
          title="Get new bangers!"
          description="We cook fresh tracks based on your taste and energy. Zero filler, only songs you’ll wanna loop to insanity."
        />
      </div>
    </div>
  );
}