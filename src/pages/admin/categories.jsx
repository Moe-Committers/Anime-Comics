import { useState } from "react";
import { useCategories } from "@/hooks/useCategories";
import { categoryService } from "@/services/category.service";
import { toast, Toaster } from "sonner";
import { Search, PlusCircle, Pencil, Trash2, EyeOff, Eye, ArrowUpDown } from "lucide-react";
import { Pagination } from "@/components/common/Pagination";
import { CategoryModal } from "@/components/category/CategoryModal";
import CategoryListSkeleton from "@/components/category/CategoryListSkeleton";
import withAdminAuth from "@/components/layout/WithAdminAuth";

function AdminCategoriesPage({user}) {
  const [queryParams, setQueryParams] = useState({
    page: 1,
    pageSize: 10,
    search: "",
    sort: "name",
    isAscending: true,
  });

  const { categories, pagination, loading, refetch } = useCategories(queryParams);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingCategory, setEditingCategory] = useState(null);

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
      const response = await categoryService.deleteCategory(id);
      if (!response.success) {
        throw new Error(response.message);
      }
      toast.success("Category deleted successfully");
      refetch();
    } catch (error) {
      toast.error(error.message || "Failed to delete category");
    }
  };

  return (
    <div className="max-w-7xl mx-auto py-4 mt-8">
      <Toaster/>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-white">Manage Categories</h1>
        <div className="flex gap-4">
          <div className="relative">
            <input
              type="text"
              value={queryParams.search}
              onChange={(e) => handleSearch(e.target.value)}
              placeholder="Search categories..."
              className="w-64 px-4 py-2 pr-10 rounded-lg bg-gray-700 text-white placeholder-gray-400 focus:outline-none"
            />
            <Search className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 h-5 w-5" />
          </div>

          <button
            onClick={() => {
              setEditingCategory(null);
              setIsModalOpen(true);
            }}
            className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
          >
            <PlusCircle className="h-5 w-5" />
            Add Category
          </button>
        </div>
      </div>

      <div className="bg-gray-800 rounded-lg overflow-hidden">
        <table className="w-full">
          <thead>
            <tr className="bg-gray-700">
              <th className="px-6 py-3 text-left">
                <span className="text-xs font-medium text-gray-300 uppercase tracking-wider">
                  Icon
                </span>
              </th>
              <th className="px-6 py-3 text-left">
                <button
                  onClick={() => handleSort("name")}
                  className="flex items-center gap-1 text-xs font-medium text-gray-300 uppercase tracking-wider hover:text-white"
                >
                  Name
                  <ArrowUpDown className="h-4 w-4" />
                  {queryParams.sort === "name" && (
                    <span>{queryParams.isAscending ? "↑" : "↓"}</span>
                  )}
                </button>
              </th>
              <th className="px-6 py-3 text-left">
                <button
                  onClick={() => handleSort("order")}
                  className="flex items-center gap-1 text-xs font-medium text-gray-300 uppercase tracking-wider hover:text-white"
                >
                  Order
                  <ArrowUpDown className="h-4 w-4" />
                  {queryParams.sort === "order" && (
                    <span>{queryParams.isAscending ? "↑" : "↓"}</span>
                  )}
                </button>
              </th>
              <th className="px-6 py-3 text-left">
                <button
                  onClick={() => handleSort("status")}
                  className="flex items-center gap-1 text-xs font-medium text-gray-300 uppercase tracking-wider hover:text-white"
                >
                  Status
                  <ArrowUpDown className="h-4 w-4" />
                  {queryParams.sort === "status" && (
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
                  <CategoryListSkeleton />
                </td>
              </tr>
            ) : (
              categories.map((category) => (
                <tr key={category.id}>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <img
                      src={`${process.env.NEXT_PUBLIC_API_URL}${category.icon}`}
                      alt={category.name}
                      className="h-8 w-8 rounded"
                    />
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-white">
                    {category.name}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-white">
                    {category.order}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    {category.status === 1 ? (
                      <span className="px-2 py-1 text-xs rounded-full bg-green-900 text-green-300">
                        Active
                      </span>
                    ) : (
                      <span className="px-2 py-1 text-xs rounded-full bg-red-900 text-red-300">
                        Inactive
                      </span>
                    )}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="flex gap-2">
                      <button
                        onClick={() => {
                          setEditingCategory(category);
                          setIsModalOpen(true);
                        }}
                        className="p-1 text-blue-400 hover:text-blue-300"
                      >
                        <Pencil className="h-5 w-5" />
                      </button>
                      <button
                        onClick={() => handleDelete(category.id)}
                        className="p-1 text-red-400 hover:text-red-300"
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
        <CategoryModal
          category={editingCategory}
          onClose={() => setIsModalOpen(false)}
          onSuccess={() => {
            setIsModalOpen(false);
            refetch();
          }}
        />
      )}
    </div>
  );
}

export default withAdminAuth(AdminCategoriesPage);
