export function ShowcaseSkeleton() {
    return (
      <div className="space-y-12">
        {[1, 2, 3].map((section) => (
          <div key={section}>
            <div className="flex justify-between items-center mb-6">
              <div className="h-7 w-48 bg-gray-700 rounded animate-pulse" />
              <div className="h-6 w-20 bg-gray-700 rounded animate-pulse" />
            </div>
  
            <div className="flex gap-4">
              {[1, 2, 3, 4, 5].map((item) => (
                <div 
                  key={item} 
                  className="flex-none w-[200px] bg-gray-800 rounded-lg overflow-hidden animate-pulse"
                >
                  <div className="aspect-[3/4] bg-gray-700" />
                  <div className="p-3 space-y-2">
                    <div className="h-5 bg-gray-700 rounded w-3/4" />
                    <div className="h-4 bg-gray-700 rounded w-1/2" />
                  </div>
                </div>
              ))}
            </div>
          </div>
        ))}
      </div>
    );
  }