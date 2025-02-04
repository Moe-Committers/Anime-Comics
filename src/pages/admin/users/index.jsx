import { useState } from "react";
import { useUsers } from "@/hooks/useUsers";
import { userService } from "@/services/user.service";
import { Pagination } from "@/components/common/Pagination";
import { UserListSkeleton } from "@/components/user/UserListSkeleton";
import { UserProfileModal } from "@/components/user/UserProfileModal";
import { DateRangeFilter } from "@/components/common/DateRangeFilter";
import withAdminAuth from "@/components/layout/WithAdminAuth";
import {
  ArrowUpDown,
  Search,
  AlertCircle,
  CheckCircle,
  Calendar,
  Mail,
  User as UserIcon,
  Settings,
} from "lucide-react";
import { toast, Toaster } from "sonner";
import { format } from "date-fns";

function AdminUsersPage() {
  const [queryParams, setQueryParams] = useState({
    page: 1,
    pageSize: 10,
    search: "",
    sort: "created",
    isAscending: false,
    fromDate: null,
    toDate: null,
  });

  const [editingUser, setEditingUser] = useState(null);
  const { users, pagination, loading, refetch } = useUsers(queryParams);

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

  const handleDateFilter = (fromDate, toDate) => {
    setQueryParams((prev) => ({
      ...prev,
      fromDate,
      toDate,
      page: 1,
    }));
  };

  const clearDateFilter = () => {
    setQueryParams((prev) => ({
      ...prev,
      fromDate: null,
      toDate: null,
      page: 1,
    }));
  };

  const handleToggleStatus = async (userId, currentStatus) => {
    try {
      const response = await userService.toggleUserStatus(
        userId,
        !currentStatus
      );
      if (!response.success) {
        throw new Error(response.message);
      }
      toast.success("User status updated successfully");
      refetch();
    } catch (error) {
      toast.error(error.message || "Failed to update user status");
    }
  };

  return (
    <div className="max-w-7xl mx-auto py-4 mt-8">
      <Toaster/>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-white">User Management</h1>
        <div className="flex gap-4">
          <div className="relative">
            <input
              type="text"
              value={queryParams.search}
              onChange={(e) => handleSearch(e.target.value)}
              placeholder="Search users..."
              className="w-64 px-4 py-2 pr-10 rounded-lg bg-gray-700 text-white placeholder-gray-400 focus:outline-none"
            />
            <Search className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 h-5 w-5" />
          </div>
          <DateRangeFilter
            onApply={handleDateFilter}
            onClear={clearDateFilter}
          />
        </div>
      </div>

      <div className="bg-gray-800 rounded-lg overflow-hidden">
        <table className="w-full">
          <thead>
            <tr className="bg-gray-700">
              <th className="px-6 py-3 text-left">
                <span className="text-xs font-medium text-gray-300 uppercase tracking-wider">
                  Profile
                </span>
              </th>
              <th className="px-6 py-3 text-left">
                <button
                  onClick={() => handleSort("name")}
                  className="flex items-center gap-1 text-xs font-medium text-gray-300 uppercase tracking-wider hover:text-white"
                >
                  <UserIcon className="h-4 w-4" />
                  Name
                  <ArrowUpDown className="h-4 w-4" />
                  {queryParams.sort === "name" && (
                    <span>{queryParams.isAscending ? "↑" : "↓"}</span>
                  )}
                </button>
              </th>
              <th className="px-6 py-3 text-left">
                <div className="flex items-center gap-1 text-xs font-medium text-gray-300 uppercase tracking-wider">
                  <Mail className="h-4 w-4" />
                  Email
                </div>
              </th>
              <th className="px-6 py-3 text-left">
                <button
                  onClick={() => handleSort("age")}
                  className="flex items-center gap-1 text-xs font-medium text-gray-300 uppercase tracking-wider hover:text-white"
                >
                  Age
                  <ArrowUpDown className="h-4 w-4" />
                  {queryParams.sort === "age" && (
                    <span>{queryParams.isAscending ? "↑" : "↓"}</span>
                  )}
                </button>
              </th>
              <th className="px-6 py-3 text-left">
                <button
                  onClick={() => handleSort("created")}
                  className="flex items-center gap-1 text-xs font-medium text-gray-300 uppercase tracking-wider hover:text-white"
                >
                  <Calendar className="h-4 w-4" />
                  Created At
                  <ArrowUpDown className="h-4 w-4" />
                  {queryParams.sort === "created" && (
                    <span>{queryParams.isAscending ? "↑" : "↓"}</span>
                  )}
                </button>
              </th>
              <th className="px-6 py-3 text-left">
                <span className="text-xs font-medium text-gray-300 uppercase tracking-wider">
                  Role
                </span>
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
                <td colSpan="8">
                  <UserListSkeleton />
                </td>
              </tr>
            ) : (
              users.map((user) => (
                <tr key={user.id}>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="w-10 h-10 rounded-full bg-gray-700 flex items-center justify-center">
                      {user.profile ? (
                        <img
                          src={`${process.env.NEXT_PUBLIC_API_URL}${user.profile}`}
                          alt={user.name}
                          className="w-full h-full rounded-full object-cover"
                        />
                      ) : (
                        <span className="text-lg font-medium text-white">
                          {user.name[0].toUpperCase()}
                        </span>
                      )}
                    </div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-white">
                    {user.name}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-white">
                    {user.email}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-white">
                    {user.age}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-white">
                    {format(new Date(user.createdAt), "MMM dd, yyyy")}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <span
                      className={`px-2 py-1 text-xs rounded-full 
                        ${
                          user.role === 0
                            ? "bg-purple-900 text-purple-300"
                            : "bg-blue-900 text-blue-300"
                        }`}
                    >
                      {user.role === 0 ? "Admin" : "User"}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <button
                      onClick={() =>
                        handleToggleStatus(user.id, user.status === 0)
                      }
                      className={`flex items-center gap-2 px-3 py-1 rounded-full text-sm font-medium transition-colors
                        ${
                          user.status === 0
                            ? "bg-green-900 text-green-300 hover:bg-green-800"
                            : "bg-red-900 text-red-300 hover:bg-red-800"
                        }`}
                    >
                      {user.status === 0 ? (
                        <>
                          <CheckCircle className="h-4 w-4" />
                          Active
                        </>
                      ) : (
                        <>
                          <AlertCircle className="h-4 w-4" />
                          Inactive
                        </>
                      )}
                    </button>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <button
                      onClick={() => setEditingUser(user)}
                      className="p-2 text-gray-400 hover:text-white rounded-lg hover:bg-gray-700"
                      title="Edit Profile"
                    >
                      <Settings className="h-5 w-5" />
                    </button>
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

      {editingUser && (
        <UserProfileModal
          user={editingUser}
          onClose={() => setEditingUser(null)}
          onSuccess={() => {
            setEditingUser(null);
            refetch();
          }}
        />
      )}
    </div>
  );
}

export default withAdminAuth(AdminUsersPage);
