"use client"
import { FC, useEffect } from "react";
import { useAuth } from "@/app/hooks/useAuth";
import { useRouter } from "next/navigation";

export const AuthRedirectIfNotAuthenticated: FC<{ path: string }> = ({ path }) => {
  const router = useRouter();
  const { user, loading } = useAuth();

  useEffect(() => {
    if (!loading && !user) {
      router.push(path.startsWith("/") ? path : `/${path}`);
    }
  }, [loading, user]);

  return null;
}

export const AuthRedirectIfAuthenticated: FC<{ path: string }> = ({ path }) => {
  const router = useRouter();
  const { user, loading } = useAuth();

  useEffect(() => {
    if (!loading && user) {
      router.push(path.startsWith("/") ? path : `/${path}`);
    }
  }, [loading, user]);

  return null;
}