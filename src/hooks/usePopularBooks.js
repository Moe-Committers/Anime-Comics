import { useState, useEffect } from "react";
import { mangaService } from "@/services/manga.service";

export function usePopularBooks() {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchPopularBooks = async () => {
      try {
        setLoading(true);
        const response = await mangaService.getPopularBooks();
        setData(response.data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchPopularBooks();
  }, []);

  return { data, loading, error };
}
