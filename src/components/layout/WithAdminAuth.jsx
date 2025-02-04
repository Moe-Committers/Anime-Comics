import { useAuthStore } from "@/store/auth.store";
import { useRouter } from "next/router";
import { useEffect } from "react";
import { AdminLayout } from "../admin/AdminLayout";

export default function withAdminAuth(Component) {
  return function AdminProtectedRoute(props) {
    const { user, isAuthenticated, isLoading } = useAuthStore();
    const router = useRouter();

    useEffect(() => {
      if (!isLoading && (!isAuthenticated || user?.data?.role !== 0)) {
        router.push("/");
      }
    }, [isLoading, isAuthenticated, user, router]);

    if (isLoading) {
      return <div>Loading...</div>;
    }

    if (!isAuthenticated || user?.data?.role !== 0) {
      return null;
    }

    return (
      <AdminLayout>
          <Component {...props} />
      </AdminLayout>
    );
  };
}
