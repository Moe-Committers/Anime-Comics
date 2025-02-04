import { useEffect, useState } from "react";
import { useRouter } from "next/router";
import { useAuthStore } from "@/store/auth.store";
import { Navbar } from "../Navbar/Navbar";

export default function withAuth(Component, options = {}) {
  const { requiresAuth = true } = options; 

  return function ProtectedRoute(props) {
    const { isAuthenticated, isLoading, checkAuth } = useAuthStore();
    const [isChecking, setIsChecking] = useState(true);
    const router = useRouter();

    useEffect(() => {
      const init = async () => {
        await checkAuth();
        setIsChecking(false);
      };

      init();
    }, [checkAuth]);

    if (isChecking || isLoading) {
      return (
        <div className="min-h-screen flex items-center justify-center bg-[#1f1f1f]">
          <div className="w-8 h-8 border-4 border-blue-500 border-t-transparent rounded-full animate-spin" />
        </div>
      );
    }

    if (requiresAuth && !isAuthenticated) {
      router.push("/login");
      return null; 
    }

    return (
      <div className="min-h-screen bg-[#1f1f1f]">
        <Navbar />
        <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-6">
          <Component {...props} />
        </main>
      </div>
    );
  };
}
