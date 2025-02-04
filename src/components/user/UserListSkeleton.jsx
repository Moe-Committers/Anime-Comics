export function UserListSkeleton() {
  return (
    <div className="p-6 max-w-7xl mx-auto">
      <div className="flex justify-between items-center mb-6">
        <div className="h-8 w-48 bg-gray-700 rounded animate-pulse" />
        <div className="flex gap-4">
          <div className="h-10 w-64 bg-gray-700 rounded animate-pulse" />
          <div className="h-10 w-48 bg-gray-700 rounded animate-pulse" />
        </div>
      </div>

      <div className="bg-gray-800 rounded-lg overflow-hidden">
        <table className="w-full">
          <thead>
            <tr className="bg-gray-700">
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">
                Profile
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">
                Name
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">
                Email
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">
                Age
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">
                Created At
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">
                Role
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">
                Status
              </th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-700">
            {[...Array(5)].map((_, index) => (
              <tr key={index}>
                <td className="px-6 py-4 whitespace-nowrap">
                  <div className="w-10 h-10 bg-gray-700 rounded-full animate-pulse" />
                </td>
                <td className="px-6 py-4 whitespace-nowrap">
                  <div className="h-4 w-32 bg-gray-700 rounded animate-pulse" />
                </td>
                <td className="px-6 py-4 whitespace-nowrap">
                  <div className="h-4 w-48 bg-gray-700 rounded animate-pulse" />
                </td>
                <td className="px-6 py-4 whitespace-nowrap">
                  <div className="h-4 w-12 bg-gray-700 rounded animate-pulse" />
                </td>
                <td className="px-6 py-4 whitespace-nowrap">
                  <div className="h-4 w-32 bg-gray-700 rounded animate-pulse" />
                </td>
                <td className="px-6 py-4 whitespace-nowrap">
                  <div className="h-6 w-20 bg-gray-700 rounded-full animate-pulse" />
                </td>
                <td className="px-6 py-4 whitespace-nowrap">
                  <div className="h-8 w-24 bg-gray-700 rounded-full animate-pulse" />
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
