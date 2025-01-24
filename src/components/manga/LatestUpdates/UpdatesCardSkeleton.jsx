export function UpdatesCardSkeleton() {
  return (
    <div className="bg-[#2a2a2a] rounded-lg overflow-hidden">
      {[...Array(6)].map((_, i) => (
        <div
          key={i}
          className="flex gap-3 p-3 border-b border-gray-700 last:border-b-0"
        >
          <div className="flex-shrink-0">
            <div className="w-[60px] h-[80px] bg-gray-700 rounded animate-pulse" />
          </div>
          <div className="flex flex-col flex-1 gap-2">
            <div className="h-4 bg-gray-700 rounded w-3/4 animate-pulse" />
            <div className="h-3 bg-gray-700 rounded w-1/2 animate-pulse" />
            <div className="h-3 bg-gray-700 rounded w-1/3 animate-pulse mt-auto" />
          </div>
        </div>
      ))}
    </div>
  );
}
