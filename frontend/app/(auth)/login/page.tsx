import LoginForm from '@/app/Components/Authentication/LoginForm';
import Link from 'next/link';
import { FC } from 'react';

const LoginPage: FC = ({}) => {
  return (
    <div className="w-full min-h-screen flex flex-col items-center bg-zinc-100 font-sans">
      <header className="w-full max-w-[1000px] h-48 flex flex-col justify-center items-center mx-auto">
        <h1 className="text-zinc-800 font-josefin-sans font-bold text-4xl select-none">
          Login
        </h1>
        <Link
          href="/"
          className="text-zinc-600 font-josefin-sans select-none"
        >
          &#171; BANGERFINDER
        </Link>
      </header>
      <LoginForm />
      <Link
        className="mt-5 text-zinc-500 text-sm"
        href="/register"
      >
        Register instead
      </Link>
    </div>
  );
}

export default LoginPage;