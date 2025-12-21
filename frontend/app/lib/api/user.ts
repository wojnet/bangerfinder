import { LoginDto, RegisterDto } from "@/app/Types/User";

const API_URL = "http://localhost:5174/api/UserCreation";

export const registerUser = async (data: RegisterDto) => {
  const res = await fetch(`${API_URL}/Register`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(data),
  });

  const json = await res.json();

  if (!res.ok)
    throw new Error(json.message || "Registration failed");

  return json;
}

export const loginUser = async (data: LoginDto) => {
  const res = await fetch(`${API_URL}/Login`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(data),
  });

  const json = await res.json();

  if (!res.ok)
    throw new Error(json.message || "Logging in failed");

  return json;
} 