"use client"
import { useEffect } from 'react';
import { useRouter, useSearchParams } from 'next/navigation';
import { useAuth } from '../hooks/useAuth';

export default function LastFmCallback() {
    const searchParams = useSearchParams();
    const router = useRouter();
    const { token: appToken } = useAuth();

    useEffect(() => {
        const lastFmToken = searchParams.get('token');
        if (lastFmToken && appToken) {
            fetch(`http://localhost:5000/api/LastFmAuth/callback?token=${lastFmToken}`, {
                headers: { 'Authorization': `Bearer ${appToken}` }
            })
            .then(res => {
                if(res.ok) {
                    // Navigate to dashboard only AFTER the backend confirms success
                    router.push('/dashboard?linked=true');
                } else {
                    console.error("Backend rejected the handshake");
                }
            })
            .catch(err => console.error("Network error:", err));
        }
    }, [searchParams, appToken, router]);

    return <p>Finalizing connection to Last.fm...</p>;
}