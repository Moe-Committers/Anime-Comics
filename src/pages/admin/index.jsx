import { Book, Users, UserCheck, Tags } from "lucide-react";
import { useDashboard } from "@/hooks/useDashboard";
import withAdminAuth from "@/components/layout/WithAdminAuth";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from "recharts";

function AdminDashboardPage() {
  const { data, loading } = useDashboard();

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="w-8 h-8 border-4 border-blue-500 border-t-transparent rounded-full animate-spin" />
      </div>
    );
  }

  const monthlyData = data?.monthlyNewUsers.map((item, index) => ({
    name: new Date(2024, item.month - 1).toLocaleString("default", {
      month: "short",
    }),
    "New Users": item.count,
    "New Books": data.monthlyNewBooks[index]?.count || 0,
  }));

  return (
    <div className="max-w-7xl mx-auto py-8 px-4">
      <h1 className="text-2xl font-bold text-white mb-8">Dashboard</h1>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
        <div className="bg-gray-800 p-6 rounded-lg">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-gray-400">Total Books</p>
              <p className="text-2xl font-bold text-white mt-1">
                {data?.totalBooks}
              </p>
            </div>
            <Book className="h-8 w-8 text-blue-500" />
          </div>
        </div>

        <div className="bg-gray-800 p-6 rounded-lg">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-gray-400">Total Users</p>
              <p className="text-2xl font-bold text-white mt-1">
                {data?.totalUsers}
              </p>
            </div>
            <Users className="h-8 w-8 text-green-500" />
          </div>
        </div>

        <div className="bg-gray-800 p-6 rounded-lg">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-gray-400">Active Users</p>
              <p className="text-2xl font-bold text-white mt-1">
                {data?.activeUsers}
              </p>
            </div>
            <UserCheck className="h-8 w-8 text-orange-500" />
          </div>
        </div>

        <div className="bg-gray-800 p-6 rounded-lg">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-gray-400">Categories</p>
              <p className="text-2xl font-bold text-white mt-1">
                {data?.totalCategories}
              </p>
            </div>
            <Tags className="h-8 w-8 text-purple-500" />
          </div>
        </div>
      </div>

      <div className="bg-gray-800 p-6 rounded-lg">
        <h2 className="text-lg font-medium text-white mb-4">Monthly Growth</h2>
        <div className="h-80">
          <ResponsiveContainer width="100%" height="100%">
            <LineChart
              data={monthlyData}
              margin={{
                top: 5,
                right: 30,
                left: 20,
                bottom: 5,
              }}
            >
              <CartesianGrid strokeDasharray="3 3" stroke="#374151" />
              <XAxis dataKey="name" stroke="#9CA3AF" />
              <YAxis stroke="#9CA3AF" />
              <Tooltip
                contentStyle={{
                  backgroundColor: "#1F2937",
                  border: "none",
                  borderRadius: "0.5rem",
                }}
                itemStyle={{ color: "#E5E7EB" }}
                labelStyle={{ color: "#E5E7EB" }}
              />
              <Legend />
              <Line
                type="monotone"
                dataKey="New Users"
                stroke="#3B82F6"
                strokeWidth={2}
                dot={false}
              />
              <Line
                type="monotone"
                dataKey="New Books"
                stroke="#10B981"
                strokeWidth={2}
                dot={false}
              />
            </LineChart>
          </ResponsiveContainer>
        </div>
      </div>
    </div>
  );
}

export default withAdminAuth(AdminDashboardPage);
