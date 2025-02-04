import { VolumeListSkeleton } from "@/components/volume/VolumeListSkeleton";
import { VolumeModal } from "@/components/volume/VolumeModal";
import { Pagination } from "@/components/common/Pagination";
import withAdminAuth from "@/components/layout/WithAdminAuth";
import { useVolumes } from "@/hooks/useVolumes";
import { volumeService } from "@/services/volume.service";
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

function AdminVolumesPage() {
  const router = useRouter();
  const { bookId } = router.query;

  const [queryParams, setQueryParams] = useState({
    page: 1,
    pageSize: 10,
    search: "",
    sort: "volumeNo",
    isAscending: true,
  });

  const { volumes, pagination, loading, refetch } = useVolumes(
    bookId,
    queryParams
  );
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingVolume, setEditingVolume] = useState(null);

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

  const handleDelete = async (volumeId) => {
    if (!confirm("Are you sure you want to delete this volume?")) return;

    try {
      const response = await volumeService.deleteVolume(bookId, volumeId);
      if (!response.success) {
        throw new Error(response.message);
      }
      toast.success("Volume deleted successfully");
      refetch();
    } catch (error) {
      toast.error(error.message || "Failed to delete volume");
    }
  };

  return (
    <div className="max-w-7xl mx-auto py-4 mt-8">
      <Toaster/>
      <div className="flex justify-between items-center mb-6">
        <div className="flex items-center gap-2">
          <button
            onClick={() => router.push("/admin/books")}
            className="text-gray-400 hover:text-white"
          >
            <Book className="h-5 w-5" />
          </button>
          <span className="text-gray-400">/</span>
          <h1 className="text-2xl font-bold text-white">Manage Volumes</h1>
        </div>
        <div className="flex gap-4">
          <div className="relative">
            <input
              type="text"
              value={queryParams.search}
              onChange={(e) => handleSearch(e.target.value)}
              placeholder="Search volumes..."
              className="w-64 px-4 py-2 pr-10 rounded-lg bg-gray-700 text-white placeholder-gray-400 focus:outline-none"
            />
            <Search className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 h-5 w-5" />
          </div>

          <button
            onClick={() => {
              setEditingVolume(null);
              setIsModalOpen(true);
            }}
            className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
          >
            <PlusCircle className="h-5 w-5" />
            Add Volume
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
                  onClick={() => handleSort("volumeNo")}
                  className="flex items-center gap-1 text-xs font-medium text-gray-300 uppercase tracking-wider hover:text-white"
                >
                  Volume
                  <ArrowUpDown className="h-4 w-4" />
                  {queryParams.sort === "volumeNo" && (
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
                  <VolumeListSkeleton />
                </td>
              </tr>
            ) : (
              volumes.map((volume) => (
                <tr key={volume.id}>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <img
                      src={`${process.env.NEXT_PUBLIC_API_URL}${volume.coverImg}`}
                      alt={volume.title}
                      className="h-20 w-16 object-cover rounded"
                    />
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-white">
                    Volume {volume.volumeNo}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-white">
                    {volume.title}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-white">
                    {volume.releaseDate
                      ? new Date(volume.releaseDate).toLocaleDateString()
                      : "N/A"}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="flex gap-2">
                      <Link
                        href={`/admin/books/${bookId}/volumes/${volume.id}/chapters`}
                        className="p-1 text-yellow-400 hover:text-yellow-300"
                        title="Manage Chapters"
                      >
                        <Files className="h-5 w-5" />
                      </Link>
                      <button
                        onClick={() => {
                          setEditingVolume(volume);
                          setIsModalOpen(true);
                        }}
                        className="p-1 text-blue-400 hover:text-blue-300"
                        title="Edit"
                      >
                        <Pencil className="h-5 w-5" />
                      </button>
                      <button
                        onClick={() => handleDelete(volume.id)}
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
        <VolumeModal
          bookId={bookId}
          volume={editingVolume}
          onClose={() => {
            setIsModalOpen(false);
            setEditingVolume(null);
          }}
          onSuccess={() => {
            setIsModalOpen(false);
            setEditingVolume(null);
            refetch();
          }}
        />
      )}
    </div>
  );
}

export default withAdminAuth(AdminVolumesPage);
