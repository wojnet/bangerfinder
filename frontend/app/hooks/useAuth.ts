import { useEffect, useState } from "react";
import { User } from "../Types/User";

export const useAuth = () => {
  const [authState, setAuthState] = useState<{
    user: User | null;
    token: string | null;
    loading: boolean;
  }>({
    user: null,
    token: null,
    loading: true,
  });

  useEffect(() => {
    // Wrapping the logic in a timeout moves the state update to the next 
    // event loop tick, preventing the "cascading render" warning
    const timeoutId = setTimeout(() => {
      const t = localStorage.getItem("auth_token");
      const u = localStorage.getItem("auth_user");
      
      let parsedUser: User | null = null;
      if (u) {
        try {
          parsedUser = JSON.parse(u);
        } catch (e) {
          console.error("Failed to parse auth_user", e);
        }
      }

      // Updating a single state object ensures only one re-render cycle occurs
      // and satisfies React's performance recommendations
      setAuthState({
        token: t,
        user: parsedUser,
        loading: false,
      });
    }, 0);

    return () => clearTimeout(timeoutId); // Cleanup timeout on unmount
  }, []);

  return { 
    user: authState.user, 
    token: authState.token, 
    loading: authState.loading 
  };
};