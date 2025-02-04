import { pageService } from "@/services/page.service";
import { useCallback, useEffect, useState } from "react";

export function useChapterPages(chapterId) {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(true);
  const [pagination, setPagination] = useState(null);

  const fetchPages = useCallback(
    async (pageNum, resetData = false) => {
      if (!chapterId) return;

      try {
        setLoading(true);
        const response = await pageService.getChapterPages(chapterId, {
          page: pageNum,
          pageSize: 24,
        });

        if (!response.success) {
          throw new Error(response.message);
        }

        const newData = response.data.data;
        const paginateInfo = response.data.paginate;

        setData((prevData) =>
          resetData ? newData : [...prevData, ...newData]
        );

        if (paginateInfo) {
          setPagination(paginateInfo);
          setHasMore(pageNum < paginateInfo.totalPage);
        }
      } catch (err) {
        console.error("Fetch error:", err);
        setError(err.message);
      } finally {
        setLoading(false);
      }
    },
    [chapterId]
  );

  const loadMore = useCallback(() => {
    if (!loading && hasMore) {
      setPage((prev) => prev + 1);
    }
  }, [loading, hasMore]);

  const refetch = useCallback(() => {
    setPage(1);
    fetchPages(1, true);
  }, [fetchPages]);

  useEffect(() => {
    setPage(1);
    setData([]);
    setHasMore(true);
    if (chapterId) {
      fetchPages(1, true);
    }
  }, [chapterId]);

  useEffect(() => {
    if (page > 1) {
      fetchPages(page, false);
    }
  }, [page]);

  return {
    pages: data,
    loading,
    error,
    pagination,
    hasMore,
    loadMore,
    refetch,
  };
}
