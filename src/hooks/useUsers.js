import { useState, useEffect } from "react";
import { userService } from "@/services/user.service";

export function useUsers(params) {
  const [data, setData] = useState([]);
  const [pagination, setPagination] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchUsers();
  }, [params]);

  const fetchUsers = async () => {
    try {
      setLoading(true);
      const response = await userService.getUsers(params);

      if (!response.success) {
        throw new Error(response.message);
      }

      setData(response.data);
      setPagination(response.paginate);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return {
    users: data,
    pagination,
    loading,
    error,
    refetch: fetchUsers,
  };
}
