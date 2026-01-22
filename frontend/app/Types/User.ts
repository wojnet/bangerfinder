export interface User {
  id: number;
  name: string;
  username: string;
  email: string;
}

export interface LoginResponse {
  token: string;
  expiration: string;
  message: string;
  user: User;
}

export interface RegisterDto {
  name: string;
  username: string;
  email: string;
  password: string;
}

export interface LoginDto {
  loginInput: string;
  password: string;
}