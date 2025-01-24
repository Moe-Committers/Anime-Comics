import { useRouter } from "next/router";
import {ChevronLeft} from "lucide-react";
import { usePublishedBooks } from "@/hooks/usePublishedBook";
import { Pagination } from "@/components/common/Pagination";
import { MangaListItem } from "@/components/manga/Manga/MangaListItem";
import { MangaListSkeleton } from "@/components/manga/Manga/MangaListSkeleton";
import Link from "next/link";
import { Navbar } from "@/components/layout/Navbar";

export default function LatestMangaPage() {
  const router = useRouter();

  const currentPage = Number(router.query.page) || 1;

  const { data, pagination, loading, error } = usePublishedBooks({
    page: currentPage,
    pageSize: 20,
    sort: "created",
  });

  const handlePageChange = (page) => {
    router.push({
      pathname: router.pathname,
      query: { ...router.query, page },
    });
  };

  if (error) {
    return null;
  }

  return (
    <div className="min-h-screen bg-[#1f1f1f]">
      <Navbar />
      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-6">
        <div className="max-w-7xl mx-auto py-4 mt-8">
          <div className="flex items-center gap-4">
            <Link href="/" className="text-gray-400 hover:text-white">
              <ChevronLeft className="h-6 w-6" />
            </Link>
            <h1 className="text-xl font-bold text-white">Latest Updates</h1>
          </div>
        </div>

        <div className="max-w-7xl mx-auto py-8">
          <div className="mb-8">
            {loading ? (
              <MangaListSkeleton />
            ) : (
              <div className="space-y-4">
                {data.map((manga) => (
                  <MangaListItem key={manga.id} manga={manga} />
                ))}
              </div>
            )}
          </div>

          {pagination && (
            <Pagination
              currentPage={currentPage}
              totalPages={pagination.totalPage}
              onPageChange={handlePageChange}
            />
          )}
        </div>
      </main>
    </div>
  );
}
