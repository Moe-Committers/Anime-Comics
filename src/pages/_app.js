import '@/styles/globals.css';
import { useAuthStore } from '@/store/auth.store';
import { useEffect } from 'react';

export default function App({ Component, pageProps }) {
  const { checkAuth } = useAuthStore();

  useEffect(() => {
    checkAuth();
  }, []);
  return <Component {...pageProps} />;
}