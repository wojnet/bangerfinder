"use client"
import React, { FC, useState } from 'react'; // Added React import
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

const LoginForm: FC = () => {
  const router = useRouter();
  const [formData, setFormData] = useState<LoginDto>(initialFormData);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  // Use React.FormEvent or React.BaseSyntheticEvent to avoid 'any'
  const handleLogin = async (e: React.FormEvent | React.KeyboardEvent) => {
    e.preventDefault();
    
    if (isLoading) return;

    try {
      setIsLoading(true);
      const res = await loginUser(formData);

      localStorage.setItem("auth_token", res.token);
      localStorage.setItem("auth_user", JSON.stringify(res.user));

      router.push("/dashboard");
    } catch (err: unknown) { // Use 'unknown' instead of 'any'
      changeFormDataField("password", "");
      if (err instanceof Error) {
        alert(err.message);
      }
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
        onSubmit={handleLogin} // Use onSubmit for better accessibility
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
          onEnterPressed={() => handleLogin({ preventDefault: () => {} } as React.FormEvent)}
        />
        <br />
        <Submit
          label="Login"
          onClick={handleLogin}
          disabled={isLoading}
        />
      </form>
    </>
  );
}

export default LoginForm;