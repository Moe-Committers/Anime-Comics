import { useState, useEffect, useCallback } from "react";
import { ChevronLeft, ChevronRight } from "lucide-react";
import { usePopularBooks } from "@/hooks/usePopularBooks";
import { FeaturedSkeleton } from "./FeatureSkeleton";

export function FeaturedCarousel() {
  const [slidePosition, setSlidePosition] = useState(0);
  const { data: featured, loading, error } = usePopularBooks();
  const [autoPlayKey, setAutoPlayKey] = useState(0);

  const updateSlidePosition = useCallback(
    (direction) => {
      if (!featured?.length) return;

      setSlidePosition((prev) => {
        const newPosition = direction === "next" ? prev - 100 : prev + 100;

        if (Math.abs(newPosition) >= featured.length * 100) {
          return 0;
        } else if (newPosition > 0) {
          return -((featured.length - 1) * 100);
        }

        return newPosition;
      });
    },
    [featured]
  );

  useEffect(() => {
    if (!featured?.length) return;

    const timer = setInterval(() => {
      updateSlidePosition("next");
    }, 5000);

    return () => clearInterval(timer);
  }, [featured, autoPlayKey, updateSlidePosition]);

  const handleNavigation = (direction) => {
    updateSlidePosition(direction);
    setAutoPlayKey((prev) => prev + 1);
  };

  if (loading) {
    return <FeaturedSkeleton />;
  }

  if (error || !featured?.length) {
    return null;
  }

  const currentIndex = Math.abs(
    Math.round(slidePosition / 100) % featured.length
  );

  return (
    <div className="-mt-8 relative w-screen left-[calc(-50vw+50%)] right-[calc(-50vw+50%)] bg-[#1f1f1f] overflow-hidden">
      {/******fix ing the w-screen issue up**** */}
      <div className="relative h-[500px]">
        <div
          className="absolute inset-0 flex transition-transform duration-700 ease-out"
          style={{ transform: `translateX(${slidePosition}%)` }}
        >
          {featured.map((book) => (
            <div key={book.id} className="relative w-full flex-shrink-0">
              <div className="absolute inset-0">
                <img
                  src={`${process.env.NEXT_PUBLIC_API_URL}${book.imageUrl}`}
                  alt=""
                  className="w-full h-full object-cover object-center blur-sm brightness-50"
                />
              </div>

              <div className="absolute -bottom-2 left-0 right-0 h-32 bg-gradient-to-t from-[#1f1f1f] to-transparent" />

              <div className="relative max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-full">
                <div className="pt-[65px]">
                  <h2 className="text-xl font-bold text-white">
                    Popular New Titles
                  </h2>
                </div>

                <div className="pt-5">
                  <div className="flex gap-6">
                    <img
                      src={`${process.env.NEXT_PUBLIC_API_URL}${book.imageUrl}`}
                      alt={book.title}
                      className="h-[350px] w-auto rounded shadow-xl"
                    />

                    <div className="flex flex-col max-w-xl">
                      <h1 className="text-3xl font-bold text-white mb-3">
                        {book.title}
                      </h1>
                      <div className="flex gap-2 mb-4">
                        {book.categories.map((category) => (
                          <span
                            key={category.id}
                            className="px-2 py-1 bg-gray-800 text-white text-xs rounded"
                          >
                            {category.name}
                          </span>
                        ))}
                      </div>
                      <p className="text-gray-300 text-base line-clamp-4 mb-4">
                        {book.description}
                      </p>
                      <div className="text-gray-400 text-sm">
                        <span>{book.author}</span>
                        <span className="mx-2">•</span>
                        <span>{book.userName}</span>
                        <span className="mx-2">•</span>
                        <span>{book.fav} favorites</span>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>

        <div className="absolute inset-x-0 bottom-8 pointer-events-none">
          <div className="relative max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
            <div className="absolute bottom-0 right-8 flex items-center gap-2 pointer-events-auto">
              <span className="text-white text-md">No. {currentIndex + 1}</span>
              <div className="flex gap-1">
                <button
                  onClick={() => handleNavigation("prev")}
                  className="p-1 text-white transition-colors hover:text-gray-300"
                >
                  <ChevronLeft className="h-7 w-7" />
                </button>
                <button
                  onClick={() => handleNavigation("next")}
                  className="p-1 text-white transition-colors hover:text-gray-300"
                >
                  <ChevronRight className="h-7 w-7" />
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
