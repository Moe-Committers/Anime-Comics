import { useState, useEffect } from "react";
import { bookService } from "@/services/book.service";

export function useBooks(params) {
  const [data, setData] = useState([]);
  const [pagination, setPagination] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchBooks();
  }, [params]);

  const fetchBooks = async () => {
    try {
      setLoading(true);
      const response = await bookService.getBooks(params);

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
    books: data,
    pagination,
    loading,
    error,
    refetch: fetchBooks,
  };
}
