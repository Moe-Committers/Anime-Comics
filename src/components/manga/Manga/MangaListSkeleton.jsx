export function MangaListSkeleton() {
  return (
    <div className="space-y-4">
      {[...Array(10)].map((_, i) => (
        <div
          key={i}
          className="bg-[#2a2a2a] p-4 rounded-lg flex gap-4 animate-pulse"
        >
          <div className="flex-shrink-0 w-[72px] h-[96px] bg-gray-700 rounded" />

          <div className="flex-1">
            <div className="flex justify-between">
              <div className="space-y-2 flex-1">
                <div className="h-6 bg-gray-700 rounded w-3/4" />
                <div className="h-4 bg-gray-700 rounded w-1/2" />
              </div>
              <div className="w-20 h-4 bg-gray-700 rounded" />
            </div>
            <div className="mt-auto pt-4">
              <div className="h-4 bg-gray-700 rounded w-1/3" />
            </div>
          </div>
        </div>
      ))}
    </div>
  );
}
