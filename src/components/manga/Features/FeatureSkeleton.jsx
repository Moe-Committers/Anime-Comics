export function FeaturedSkeleton() {
  return (
    <div className="-mt-8 relative w-screen left-[calc(-50vw+50%)] right-[calc(-50vw+50%)] bg-[#1f1f1f]">
      <div className="relative h-[500px]">
        <div className="absolute inset-0">
          <div className="w-full h-full bg-gray-800" />
        </div>

        <div className="absolute -bottom-2 left-0 right-0 h-32 bg-gradient-to-t from-[#1f1f1f] to-transparent" />

        <div className="relative max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="pt-[65px]">
            <div className="h-7 w-48 bg-gray-700 rounded animate-pulse" />
          </div>

          <div className="pt-5">
            <div className="flex gap-6">
              <div className="h-[350px] w-[250px] bg-gray-700 rounded animate-pulse" />

              <div className="flex flex-col max-w-xl">
                <div className="h-9 w-3/4 bg-gray-700 rounded animate-pulse mb-3" />
                
                <div className="flex gap-2 mb-4">
                  {[1, 2, 3].map((i) => (
                    <div
                      key={i}
                      className="h-6 w-20 bg-gray-700 rounded animate-pulse"
                    />
                  ))}
                </div>

                <div className="space-y-2 mb-4">
                  {[1, 2, 3, 4].map((i) => (
                    <div
                      key={i}
                      className="h-4 bg-gray-700 rounded animate-pulse"
                      style={{
                        width: `${100 - i * 10}%`
                      }}
                    />
                  ))}
                </div>

                <div className="flex gap-2 items-center">
                  <div className="h-4 w-32 bg-gray-700 rounded animate-pulse" />
                  <div className="h-4 w-4 bg-gray-700 rounded-full animate-pulse" />
                  <div className="h-4 w-24 bg-gray-700 rounded animate-pulse" />
                  <div className="h-4 w-4 bg-gray-700 rounded-full animate-pulse" />
                  <div className="h-4 w-20 bg-gray-700 rounded animate-pulse" />
                </div>
              </div>
            </div>
          </div>

          <div className="absolute bottom-0 right-8 flex items-center gap-2">
            <div className="h-6 w-16 bg-gray-700 rounded animate-pulse" />
            <div className="flex gap-1">
              <div className="h-9 w-9 bg-gray-700 rounded-full animate-pulse" />
              <div className="h-9 w-9 bg-gray-700 rounded-full animate-pulse" />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}