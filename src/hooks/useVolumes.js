import { useState, useEffect } from "react";
import { volumeService } from "@/services/volume.service";

export function useVolumes(bookId, params) {
  const [data, setData] = useState([]);
  const [pagination, setPagination] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchVolumes();
  }, [bookId, params]);

  const fetchVolumes = async () => {
    if (!bookId) return;

    try {
      setLoading(true);
      const response = await volumeService.getBookVolumes(bookId, params);

      if (!response.success) {
        throw new Error(response.message);
      }

      setData(response.data);
      setPagination(response.data.paginate);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return {
    volumes: data.data,
    pagination,
    loading,
    error,
    refetch: fetchVolumes,
  };
}
