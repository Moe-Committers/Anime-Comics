import { ChapterListSkeleton } from "@/components/chapter/ChapterListSkeleton";
import { ChapterModal } from "@/components/chapter/ChapterModal";
import { Pagination } from "@/components/common/Pagination";
import withAdminAuth from "@/components/layout/WithAdminAuth";
import {
  ArrowUpDown,
  Book,
  Files,
  Pencil,
  PlusCircle,
  Search,
  Trash2,
} from "lucide-react";
import { useState } from "react";
import { toast, Toaster } from "sonner";
import { useRouter } from "next/router";
import Link from "next/link";
import { useChapters } from "@/hooks/useChapters";
import { chapterService } from "@/services/chapter.service";

function AdminChaptersPage() {
  const router = useRouter();
  const { volumeId, bookId } = router.query;

  const [queryParams, setQueryParams] = useState({
    page: 1,
    pageSize: 10,
    search: "",
    sort: "chapNo",
    isAscending: true,
  });

  const { chapters, pagination, loading, refetch } = useChapters(
    volumeId,
    queryParams
  );
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingChapter, setEditingChapter] = useState(null);

  const handleSearch = (term) => {
    setQueryParams((prev) => ({
      ...prev,
      search: term,
      page: 1,
    }));
  };

  const handleSort = (field) => {
    setQueryParams((prev) => ({
      ...prev,
      sort: field,
      isAscending: prev.sort === field ? !prev.isAscending : true,
    }));
  };

  const handlePageChange = (page) => {
    setQueryParams((prev) => ({
      ...prev,
      page,
    }));
  };

  const handleDelete = async (chapterId) => {
    if (!confirm("Are you sure you want to delete this chapter?")) return;

    try {
      const response = await chapterService.deleteChapter(volumeId, chapterId);
      if (!response.success) {
        throw new Error(response.message);
      }
      toast.success("Chapter deleted successfully");
      refetch();
    } catch (error) {
      toast.error(error.message || "Failed to delete chapter");
    }
  };

  return (
    <div className="max-w-7xl mx-auto py-4 mt-8">
      <Toaster/>
      <div className="flex justify-between items-center mb-6">
        <div className="flex items-center gap-2">
          <Link href="/admin/books" className="text-gray-400 hover:text-white">
            <Book className="h-5 w-5" />
          </Link>
          <span className="text-gray-400">/</span>
          <Link
            href={`/admin/books/${bookId}/volumes`}
            className="text-gray-400 hover:text-white"
          >
            <Files className="h-5 w-5" />
          </Link>
          <span className="text-gray-400">/</span>
          <h1 className="text-2xl font-bold text-white">Manage Chapters</h1>
        </div>
        <div className="flex gap-4">
          <div className="relative">
            <input
              type="text"
              value={queryParams.search}
              onChange={(e) => handleSearch(e.target.value)}
              placeholder="Search chapters..."
              className="w-64 px-4 py-2 pr-10 rounded-lg bg-gray-700 text-white placeholder-gray-400 focus:outline-none"
            />
            <Search className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 h-5 w-5" />
          </div>

          <button
            onClick={() => {
              setEditingChapter(null);
              setIsModalOpen(true);
            }}
            className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
          >
            <PlusCircle className="h-5 w-5" />
            Add Chapter
          </button>
        </div>
      </div>

      <div className="bg-gray-800 rounded-lg overflow-hidden">
        <table className="w-full">
          <thead>
            <tr className="bg-gray-700">
              <th className="px-6 py-3 text-left">
                <button
                  onClick={() => handleSort("chapNo")}
                  className="flex items-center gap-1 text-xs font-medium text-gray-300 uppercase tracking-wider hover:text-white"
                >
                  Chapter
                  <ArrowUpDown className="h-4 w-4" />
                  {queryParams.sort === "chapNo" && (
                    <span>{queryParams.isAscending ? "↑" : "↓"}</span>
                  )}
                </button>
              </th>
              <th className="px-6 py-3 text-left">
                <button
                  onClick={() => handleSort("title")}
                  className="flex items-center gap-1 text-xs font-medium text-gray-300 uppercase tracking-wider hover:text-white"
                >
                  Title
                  <ArrowUpDown className="h-4 w-4" />
                  {queryParams.sort === "title" && (
                    <span>{queryParams.isAscending ? "↑" : "↓"}</span>
                  )}
                </button>
              </th>
              <th className="px-6 py-3 text-left">
                <button
                  onClick={() => handleSort("pageCount")}
                  className="flex items-center gap-1 text-xs font-medium text-gray-300 uppercase tracking-wider hover:text-white"
                >
                  Pages
                  <ArrowUpDown className="h-4 w-4" />
                  {queryParams.sort === "pageCount" && (
                    <span>{queryParams.isAscending ? "↑" : "↓"}</span>
                  )}
                </button>
              </th>
              <th className="px-6 py-3 text-left">
                <button
                  onClick={() => handleSort("releaseDate")}
                  className="flex items-center gap-1 text-xs font-medium text-gray-300 uppercase tracking-wider hover:text-white"
                >
                  Release Date
                  <ArrowUpDown className="h-4 w-4" />
                  {queryParams.sort === "releaseDate" && (
                    <span>{queryParams.isAscending ? "↑" : "↓"}</span>
                  )}
                </button>
              </th>
              <th className="px-6 py-3 text-left">
                <span className="text-xs font-medium text-gray-300 uppercase tracking-wider">
                  Actions
                </span>
              </th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-700">
            {loading ? (
              <tr>
                <td colSpan="5">
                  <ChapterListSkeleton />
                </td>
              </tr>
            ) : (
              chapters.map((chapter) => (
                <tr key={chapter.id}>
                  <td className="px-6 py-4 whitespace-nowrap text-white">
                    Chapter {chapter.chapNo}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-white">
                    {chapter.title}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-white">
                    {chapter.pageCount}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-white">
                    {chapter.releaseDate
                      ? new Date(chapter.releaseDate).toLocaleDateString()
                      : "N/A"}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="flex gap-2">
                      <Link
                        href={`/admin/books/${bookId}/volumes/${volumeId}/chapters/${chapter.id}/pages`}
                        className="p-1 text-yellow-400 hover:text-yellow-300"
                        title="Manage Pages"
                      >
                        <Files className="h-5 w-5" />
                      </Link>
                      <button
                        onClick={() => {
                          setEditingChapter(chapter);
                          setIsModalOpen(true);
                        }}
                        className="p-1 text-blue-400 hover:text-blue-300"
                        title="Edit"
                      >
                        <Pencil className="h-5 w-5" />
                      </button>
                      <button
                        onClick={() => handleDelete(chapter.id)}
                        className="p-1 text-red-400 hover:text-red-300"
                        title="Delete"
                      >
                        <Trash2 className="h-5 w-5" />
                      </button>
                    </div>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {pagination && !loading && (
        <div className="mt-6">
          <Pagination
            currentPage={queryParams.page}
            totalPages={pagination.totalPage}
            onPageChange={handlePageChange}
          />
        </div>
      )}

      {isModalOpen && (
        <ChapterModal
          volumeId={volumeId}
          chapter={editingChapter}
          onClose={() => {
            setIsModalOpen(false);
            setEditingChapter(null);
          }}
          onSuccess={() => {
            setIsModalOpen(false);
            setEditingChapter(null);
            refetch();
          }}
        />
      )}
    </div>
  );
}

export default withAdminAuth(AdminChaptersPage);
