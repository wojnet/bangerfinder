"use client"
import React, { FC, useState } from 'react'; // Added React import
import Input from '../UI/FormComponents/Input';
import PasswordInput from '../UI/FormComponents/PasswordInput';
import Submit from '../UI/FormComponents/Submit';
import { RegisterDto } from '@/app/Types/User';
import { registerUser } from '@/app/lib/api/user';
import { AuthRedirectIfAuthenticated } from './AuthRedirect';

const initialFormData: RegisterDto = {
  name: "",
  username: "",
  email: "",
  password: "",
}

const RegisterForm: FC = () => {
  const [formData, setFormData] = useState<RegisterDto>(initialFormData);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  // Simplified event handling to satisfy TypeScript and ESLint
  const handleRegister = async (e?: React.FormEvent | React.KeyboardEvent) => {
    e?.preventDefault(); // No more weird 'as Event' casting
    
    if (isLoading) return;

    try {
      setIsLoading(true);
      const res = await registerUser(formData);
      console.log("Registered:", res);
      // Optional: Redirect to login or auto-login here
    } catch (err: unknown) { // Use 'unknown' instead of 'any'
      if (err instanceof Error) {
        alert(err.message);
      }
    } finally {
      setIsLoading(false);
    }
  }

  const changeFormDataField = (field: keyof RegisterDto, value: string) => {
    setFormData(prev => ({...prev, [field]: value}))
  }

  return (
    <>
      <AuthRedirectIfAuthenticated path="/dashboard" />
      <form
        className="flex flex-col items-center gap-3"
        onSubmit={handleRegister} // Best practice: use onSubmit for accessibility
      >
        <Input
          placeholder="name"
          onChange={(e) => changeFormDataField("name", e.target.value)}
          disabled={isLoading}
        />
        <Input
          placeholder="username"
          onChange={(e) => changeFormDataField("username", e.target.value)}
          disabled={isLoading}
        />
        <Input
          placeholder="email"
          onChange={(e) => changeFormDataField("email", e.target.value)}
          disabled={isLoading}
        />
        <PasswordInput
          placeholder="password"
          onChange={(e) => changeFormDataField("password", e.target.value)}
          disabled={isLoading}
        />
        <br />
        <Submit
          label="Register"
          onClick={handleRegister}
          disabled={isLoading}
        />
      </form>
    </>
  );
}

export default RegisterForm;