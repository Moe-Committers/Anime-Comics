import { useState, useEffect } from "react";
import { categoryService } from "@/services/category.service";

export function useCategories(params) {
  const [data, setData] = useState([]);
  const [pagination, setPagination] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchCategories();
  }, [params]);

  const fetchCategories = async () => {
    try {
      setLoading(true);
      const response = await categoryService.getCategories(params);

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
    categories: data,
    pagination,
    loading,
    error,
    refetch: fetchCategories,
  };
}
