"use client"
import { FC, useState } from 'react';
import Input from '../UI/FormComponents/Input';
import PasswordInput from '../UI/FormComponents/PasswordInput';
import Submit from '../UI/FormComponents/Submit';
import { LoginDto } from '@/app/Types/User';
import { loginUser } from '@/app/lib/api/user';
import { useRouter } from 'next/navigation';
import { AuthRedirectIfAuthenticated } from './AuthRedirect';

const initialFormData: LoginDto = {
  loginInput: "",
  password: "",
}

const LoginForm: FC = ({}) => {
  const router = useRouter();

  const [formData, setFormData] = useState<LoginDto>(initialFormData);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const handleLogin = async (e: any) => {
    let event = e as Event;
    event?.preventDefault?.();
    
    if (isLoading) return;

    try {
      setIsLoading(true);
      const res = await loginUser(formData);

      localStorage.setItem("auth_token", res.token);
      localStorage.setItem("auth_user", JSON.stringify(res.user));

      router.push("/dashboard");

      console.log("Logged in:", res);
    } catch (err: any) {
      changeFormDataField("password", "");
      alert(err.message);
    } finally {
      setIsLoading(false);
    }
  }

  const changeFormDataField = (field: keyof LoginDto, value: string) => {
    setFormData(prev => ({...prev, [field]: value}))
  }

  return (
    <>
      <AuthRedirectIfAuthenticated path="/dashboard" />
      <form
        className="flex flex-col items-center gap-3"
        onKeyDown={(e) => {
          if (e.key === "Enter") {
            handleLogin(e);
          }
        }}
      >
        <Input
          placeholder="email or username"
          onChange={(e) => changeFormDataField("loginInput", e.target.value)}
          disabled={isLoading}
        />
        <PasswordInput
          placeholder="password"
          onChange={(e) => changeFormDataField("password", e.target.value)}
          disabled={isLoading}
        />
        <br />
        <Submit
          label="Login"
          onClick={(e) => {
            handleLogin(e);
          }}
          disabled={isLoading}
        />
      </form>
    </>
  );
}

export default LoginForm;