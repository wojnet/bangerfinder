import { useEffect, useState } from "react";
import { User } from "../Types/User";

export const useAuth = () => {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const t = localStorage.getItem("auth_token");
    const u = localStorage.getItem("auth_user");
    if (t && u) {
      setToken(t);
      setUser(JSON.parse(u));
    }
    setLoading(false);
  }, []);

  return { user, token, loading };
};