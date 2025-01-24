import { useState } from "react";
import { ChevronLeft, ChevronRight } from "lucide-react";
import { usePopularBooks } from "@/hooks/usePopularBooks";
import { FeaturedSkeleton } from "./FeatureSkeleton";

export function FeaturedCarousel() {
  const [currentSlide, setCurrentSlide] = useState(0);
  const { data: featured, loading, error } = usePopularBooks();

  if (loading) {
    return <FeaturedSkeleton/>;
  }

  if (error || !featured?.length) {
    return null;
  }

  const currentBook = featured[currentSlide];
  return (
    <div className="-mt-8 relative w-screen left-[calc(-50vw+50%)] right-[calc(-50vw+50%)] bg-[#1f1f1f]">
      <div className="relative h-[500px]">
        <div className="absolute inset-0">
          <img
            src={`${process.env.NEXT_PUBLIC_BACKEND_URL}${currentBook.imageUrl}`}
            alt=""
            className="w-full h-full object-cover object-center blur-sm brightness-50"
          />
        </div>

        <div className="absolute -bottom-2 left-0 right-0 h-32 bg-gradient-to-t from-[#1f1f1f] to-transparent" />

        <div className="relative max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="pt-[65px]">
            <h2 className="text-xl font-bold text-white">Popular New Titles</h2>
          </div>

          <div className="pt-5">
            <div className="flex gap-6">
              <img
                src={`${process.env.NEXT_PUBLIC_BACKEND_URL}${currentBook.imageUrl}`}
                alt={currentBook.title}
                className="h-[350px] w-auto rounded shadow-xl"
              />

              <div className="flex flex-col max-w-xl">
                <h1 className="text-3xl font-bold text-white mb-3">
                  {currentBook.title}
                </h1>
                <div className="flex gap-2 mb-4">
                  {currentBook.categories.map((category) => (
                    <span
                      key={category.id}
                      className="px-2 py-1 bg-gray-800 text-white text-xs rounded"
                    >
                      {category.name}
                    </span>
                  ))}
                </div>
                <p className="text-gray-300 text-base line-clamp-4 mb-4">
                  {currentBook.description}
                </p>
                <div className="text-gray-400 text-sm">
                  <span>{currentBook.author}</span>
                  <span className="mx-2">•</span>
                  <span>{currentBook.userName}</span>
                  <span className="mx-2">•</span>
                  <span>{currentBook.fav} favorites</span>
                </div>
              </div>
            </div>
          </div>

          <div className="absolute bottom-0 right-8 flex items-center gap-2">
            <span className="text-white text-md">
              No. {currentSlide + 1}
            </span>
            <div className="flex gap-1">
              <button
                onClick={() =>
                  setCurrentSlide((prev) => (prev - 1 + featured.length) % featured.length)
                }
                className="p-1 text-white transition-colors"
              >
                <ChevronLeft className="h-7 w-7" />
              </button>
              <button
                onClick={() =>
                  setCurrentSlide((prev) => (prev + 1) % featured.length)
                }
                className="p-1 text-white transition-colors"
              >
                <ChevronRight className="h-7 w-7" />
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}