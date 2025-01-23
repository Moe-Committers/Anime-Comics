import '@/styles/globals.css'
import { useAuthStore } from '@/store/auth.store'
import { useRouter } from 'next/router'
import { useEffect } from 'react'

const publicRoutes = ['/login', '/register']

export default function App({ Component, pageProps }) {
  const router = useRouter()
  const { checkAuth, isAuthenticated, isLoading } = useAuthStore()

  useEffect(() => {
    checkAuth()
  }, [])

  useEffect(() => {
    if (!isLoading) {
      if (!isAuthenticated && !publicRoutes.includes(router.pathname)) {
        router.push('/login')
      }
      if (isAuthenticated && publicRoutes.includes(router.pathname)) {
        router.push('/')
      }
    }
  }, [isAuthenticated, isLoading, router.pathname])

  if (isLoading) {
    return <div>Loading...</div>
  }

  return <Component {...pageProps} />
}