export function ChapterListSkeleton() {
  return (
    <div className="p-6 max-w-7xl mx-auto">
      <div className="flex justify-between items-center mb-6">
        <div className="h-8 w-48 bg-gray-700 rounded animate-pulse" />
        <div className="flex gap-4">
          <div className="h-10 w-64 bg-gray-700 rounded animate-pulse" />
          <div className="h-10 w-32 bg-gray-700 rounded animate-pulse" />
        </div>
      </div>

      <div className="bg-gray-800 rounded-lg overflow-hidden">
        <table className="w-full">
          <thead>
            <tr className="bg-gray-700">
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">
                Chapter
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">
                Title
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">
                Pages
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">
                Release Date
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">
                Actions
              </th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-700">
            {[...Array(5)].map((_, index) => (
              <tr key={index}>
                <td className="px-6 py-4 whitespace-nowrap">
                  <div className="h-4 w-16 bg-gray-700 rounded animate-pulse" />
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
                  <div className="flex gap-2">
                    <div className="h-8 w-8 bg-gray-700 rounded animate-pulse" />
                    <div className="h-8 w-8 bg-gray-700 rounded animate-pulse" />
                    <div className="h-8 w-8 bg-gray-700 rounded animate-pulse" />
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
