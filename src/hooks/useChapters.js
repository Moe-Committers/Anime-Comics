import { useState, useEffect } from "react";
import { chapterService } from "@/services/chapter.service";

export function useChapters(volumeId, params) {
  const [data, setData] = useState([]);
  const [pagination, setPagination] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchChapters();
  }, [volumeId, params]);

  const fetchChapters = async () => {
    if (!volumeId) return;

    try {
      setLoading(true);
      const response = await chapterService.getVolumeChapters(volumeId, params);

      if (!response.success) {
        throw new Error(response.message);
      }

      setData(response.data.data);
      setPagination(response.data.paginate);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return {
    chapters: data,
    pagination,
    loading,
    error,
    refetch: fetchChapters,
  };
}
