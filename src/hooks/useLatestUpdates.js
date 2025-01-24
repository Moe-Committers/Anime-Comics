import { useState, useEffect } from "react";
import { mangaService } from "@/services/manga.service";

export function useLatestUpdates() {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchLatest = async () => {
      try {
        setLoading(true);
        const response = await mangaService.getBooks({
          pulished: true,
          isLatest: true,
          pageSize: 18,
          page: 1,
        });

        if (!response.success) {
          throw new Error(response.message);
        }

        setData(response.data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchLatest();
  }, []);

  return { data, loading, error };
}
