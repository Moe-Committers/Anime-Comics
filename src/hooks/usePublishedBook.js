import { useState, useEffect } from "react";
import { mangaService } from "@/services/manga.service";

export function usePublishedBooks({ page, pageSize = 10 }) {
  const [data, setData] = useState([]);
  const [pagination, setPagination] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const response = await mangaService.getPublishedBooks({
          pulished: true,
          isLatest: true,
          page,
          pageSize,
        });

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

    fetchData();
  }, [page, pageSize]);

  return { data, pagination, loading, error };
}
