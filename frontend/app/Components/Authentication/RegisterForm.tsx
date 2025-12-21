"use client"
import { FC, useState } from 'react';
import Input from '../UI/FormComponents/Input';
import PasswordInput from '../UI/FormComponents/PasswordInput';
import Submit from '../UI/FormComponents/Submit';
import { RegisterDto } from '@/app/Types/User';
import { registerUser } from '@/app/lib/api/user';

const initialFormData: RegisterDto = {
  name: "",
  username: "",
  email: "",
  password: "",
}

const RegisterForm: FC = ({}) => {
  const [formData, setFormData] = useState<RegisterDto>(initialFormData);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const handleRegister = async (e: any) => {
    let event = e as Event;
    event?.preventDefault?.();
    
    if (isLoading) return;

    try {
      setIsLoading(true);
      const res = await registerUser(formData);
      console.log("Registered:", res);
    } catch (err: any) {
      alert(err.message);
    } finally {
      setIsLoading(false);
    }
  }

  const changeFormDataField = (field: keyof RegisterDto, value: string) => {
    setFormData(prev => ({...prev, [field]: value}))
  }

  return (
    <form
      className="flex flex-col items-center gap-3"
      onKeyDown={(e) => {
        if (e.key === "Enter") {
          handleRegister(e);
        }
      }}
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
        onClick={(e) => {
          handleRegister(e);
        }}
        disabled={isLoading}
      />
    </form>
  );
}

export default RegisterForm;