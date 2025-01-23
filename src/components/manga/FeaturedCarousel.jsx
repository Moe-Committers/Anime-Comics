import { useState } from "react";
import { ChevronLeft, ChevronRight } from "lucide-react";

export function FeaturedCarousel() {
  const [currentSlide, setCurrentSlide] = useState(0);

  const featured = [
    {
      id: 1,
      title: "The Legendary Hero is an Academy Honors Student",
      coverImage: "/test.jpg",
      genres: ["ACTION", "ADVENTURE", "FANTASY"],
      description:
        "Back in the Age of Despair, Kyle defeated Erebos, the genesis of evil. Then 5000 years later, he reincarnated as Leo with the memories of his past life intact. His comrades — the Brave Aron, the Divine Blacksmith Dweno, the Poet of the Stars Luna, and the Wise King Lysinca — were all recorded as great heroes.",
      author: "Jeena, Studio Inus (스튜디오 이너스)",
    },
  ];

  return (
    <div className="-mt-8 relative w-screen left-[calc(-50vw+50%)] right-[calc(-50vw+50%)] bg-[#1f1f1f]">
      <div className="relative h-[500px]">

        <div className="absolute inset-0">
          <img
            src={featured[currentSlide].coverImage}
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
                src={featured[currentSlide].coverImage}
                alt={featured[currentSlide].title}
                className="h-[350px] w-auto rounded shadow-xl"
              />

              <div className="flex flex-col max-w-xl">
                <h1 className="text-3xl font-bold text-white mb-3">
                  {featured[currentSlide].title}
                </h1>
                <div className="flex gap-2 mb-4">
                  {featured[currentSlide].genres.map((genre) => (
                    <span
                      key={genre}
                      className="px-2 py-1 bg-gray-800 text-white text-xs rounded"
                    >
                      {genre}
                    </span>
                  ))}
                </div>
                <p className="text-gray-300 text-base line-clamp-4 mb-4">
                  {featured[currentSlide].description}
                </p>
                <span className="text-gray-400 text-sm">
                  {featured[currentSlide].author}
                </span>
              </div>
            </div>
          </div>

          <div className="absolute bottom-0 right-8 flex items-center gap-2">
            <span className="text-white text-md">No. {currentSlide + 1}</span>
            <div className="flex gap-1">
              <button
                onClick={() => setCurrentSlide((prev) => (prev - 1 + featured.length) % featured.length)}
                className="p-1 text-white transition-colors"
              >
                <ChevronLeft className="h-7 w-7" />
              </button>
              <button
                onClick={() => setCurrentSlide((prev) => (prev + 1) % featured.length)}
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