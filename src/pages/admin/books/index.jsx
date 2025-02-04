import { BookListSkeleton } from "@/components/book/BookListSkeleton";
import { BookModal } from "@/components/book/BookModal";
import { Pagination } from "@/components/common/Pagination";
import withAdminAuth from "@/components/layout/WithAdminAuth";
import { useBooks } from "@/hooks/useBooks";
import { bookService } from "@/services/book.service";
import {
  ArrowUpDown,
  Eye,
  EyeOff,
  Files,
  Pencil,
  PlusCircle,
  Search,
  Trash2,
} from "lucide-react";
import Link from "next/link";
import { useState } from "react";
import { toast, Toaster } from "sonner";

function AdminBooksPage() {
  const [queryParams, setQueryParams] = useState({
    page: 1,
    pageSize: 10,
    search: "",
    sort: "created",
    isAscending: false,
  });

  const { books, pagination, loading, refetch } = useBooks(queryParams);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingBook, setEditingBook] = useState(null);

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

  const handleDelete = async (id) => {
    try {
      const response = await bookService.deleteBook(id);
      if (!response.success) {
        throw new Error(response.message);
      }
      toast.success("Book deleted successfully");
      refetch();
    } catch (error) {
      toast.error(error.message || "Failed to delete book");
    }
  };

  const handlePublish = async (id, currentStatus) => {
    try {
      const response = await bookService.publishBook(id, !currentStatus);
      if (!response.success) {
        throw new Error(response.message);
      }
      toast.success(
        `Book ${currentStatus ? "unpublished" : "published"} successfully`
      );
      refetch();
    } catch (error) {
      toast.error(error.message || "Failed to update book status");
    }
  };

  return (
    <div className="max-w-7xl mx-auto py-4 mt-8">
      <Toaster/>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-white">Manage Books</h1>
        <div className="flex gap-4">
          <div className="relative">
            <input
              type="text"
              value={queryParams.search}
              onChange={(e) => handleSearch(e.target.value)}
              placeholder="Search books..."
              className="w-64 px-4 py-2 pr-10 rounded-lg bg-gray-700 text-white placeholder-gray-400 focus:outline-none"
            />
            <Search className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 h-5 w-5" />
          </div>

          <button
            onClick={() => {
              setEditingBook(null);
              setIsModalOpen(true);
            }}
            className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
          >
            <PlusCircle className="h-5 w-5" />
            Add Book
          </button>
        </div>
      </div>

      <div className="bg-gray-800 rounded-lg overflow-hidden">
        <table className="w-full">
          <thead>
            <tr className="bg-gray-700">
              <th className="px-6 py-3 text-left">
                <span className="text-xs font-medium text-gray-300 uppercase tracking-wider">
                  Cover
                </span>
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
                  onClick={() => handleSort("author")}
                  className="flex items-center gap-1 text-xs font-medium text-gray-300 uppercase tracking-wider hover:text-white"
                >
                  Author
                  <ArrowUpDown className="h-4 w-4" />
                  {queryParams.sort === "author" && (
                    <span>{queryParams.isAscending ? "↑" : "↓"}</span>
                  )}
                </button>
              </th>
              <th className="px-6 py-3 text-left">
                <span className="text-xs font-medium text-gray-300 uppercase tracking-wider">
                  Status
                </span>
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
                <td colSpan="6">
                  <BookListSkeleton />
                </td>
              </tr>
            ) : (
              books.map((book) => (
                <tr key={book.id}>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <img
                      src={`${process.env.NEXT_PUBLIC_API_URL}${book.imageUrl}`}
                      alt={book.title}
                      className="h-20 w-16 object-cover rounded"
                    />
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-white">
                    {book.title}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-white">
                    {book.author}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    {book.published_at ? (
                      <span className="px-2 py-1 text-xs rounded-full bg-green-900 text-green-300">
                        Published
                      </span>
                    ) : (
                      <span className="px-2 py-1 text-xs rounded-full bg-yellow-900 text-yellow-300">
                        Draft
                      </span>
                    )}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="flex gap-2">
                      <Link
                        href={`/admin/books/${book.id}/volumes`}
                        className="p-1 text-yellow-400 hover:text-yellow-300"
                        title="Manage Volumes"
                      >
                        <Files className="h-5 w-5" />
                      </Link>
                      <button
                        onClick={() => {
                          setEditingBook(book);
                          setIsModalOpen(true);
                        }}
                        className="p-1 text-blue-400 hover:text-blue-300"
                        title="Edit"
                      >
                        <Pencil className="h-5 w-5" />
                      </button>
                      <button
                        onClick={() =>
                          handlePublish(book.id, !!book.published_at)
                        }
                        className="p-1 text-yellow-400 hover:text-yellow-300"
                        title={book.published_at ? "Unpublish" : "Publish"}
                      >
                        {book.published_at ? (
                          <EyeOff className="h-5 w-5" />
                        ) : (
                          <Eye className="h-5 w-5" />
                        )}
                      </button>
                      <button
                        onClick={() => handleDelete(book.id)}
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
        <BookModal
          book={editingBook}
          onClose={() => {
            setIsModalOpen(false);
            setEditingBook(null);
          }}
          onSuccess={() => {
            setIsModalOpen(false);
            setEditingBook(null);
            refetch();
          }}
        />
      )}
    </div>
  );
}

export default withAdminAuth(AdminBooksPage);
