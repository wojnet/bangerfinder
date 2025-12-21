import RegisterForm from '@/app/Components/Authentication/RegisterForm';
import Input from '@/app/Components/UI/FormComponents/Input';
import Link from 'next/link';
import { FC } from 'react';

const RegisterPage: FC = ({}) => {
  return (
    <div className="w-full min-h-screen flex flex-col items-center bg-zinc-100 font-sans">
      <header className="w-full max-w-[1000px] h-48 flex flex-col justify-center items-center mx-auto">
        <h1 className="text-zinc-800 font-josefin-sans font-bold text-4xl select-none">
          Register
        </h1>
        <Link
          href="/"
          className="text-zinc-600 font-josefin-sans select-none"
        >
          &#171; BANGERFINDER
        </Link>
      </header>
      <RegisterForm />
      <Link
        className="mt-5 text-zinc-500 text-sm"
        href="/login"
      >
        Login instead
      </Link>
    </div>
  );
}

export default RegisterPage;