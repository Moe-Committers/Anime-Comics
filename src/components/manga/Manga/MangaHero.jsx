import { BookOpen, Library } from "lucide-react";

export function MangaHero({ manga }) {
  if (!manga) return null;

  return (
    <>
      {/* Fixed Background Layer */}
      <div className="fixed top-0 left-0 right-0 h-screen">
        <div
          className="absolute inset-0 bg-cover bg-center bg-no-repeat"
          style={{
            backgroundImage: `url(${manga.coverImage})`,
            filter: "blur(8px)",
          }}
        />
        <div className="absolute inset-0 bg-black/50" />
      </div>

      {/* Scrollable Content */}
      <div className="relative z-10 h-[500px]">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-full">
          <div className="pt-[65px]">
            <h2 className="text-xl font-bold text-white">Details</h2>
          </div>

          <div className="pt-5 flex gap-6">
            {/* Cover Image */}
            <img
              src={manga.coverImage}
              alt={manga.title}
              className="h-[350px] w-auto rounded shadow-xl"
            />

            {/* Info */}
            <div className="flex flex-col max-w-xl">
              <h1 className="text-3xl font-bold text-white mb-3">
                {manga.title}
              </h1>

              <div className="flex gap-2 mb-4">
                {manga.genres.map((genre) => (
                  <span
                    key={genre}
                    className="px-2 py-1 bg-gray-800 text-white text-xs rounded"
                  >
                    {genre}
                  </span>
                ))}
              </div>

              <p className="text-gray-300 text-base line-clamp-4 mb-4">
                {manga.description}
              </p>

              <div className="text-gray-400 text-sm">
                <span>{manga.author}</span>
                <span className="mx-2">•</span>
                <span>{manga.status}</span>
                <span className="mx-2">•</span>
                <span>{manga.totalVolumes} Volumes</span>
              </div>

              <div className="flex gap-4 mt-6">
                <button className="px-6 py-2 bg-orange-500 hover:bg-orange-600 rounded text-white flex items-center gap-2">
                  <Library className="h-5 w-5" />
                  Add to Library
                </button>
                <button className="px-6 py-2 bg-[#2a2a2a] hover:bg-[#363636] text-white rounded flex items-center gap-2">
                  <BookOpen className="h-5 w-5" />
                  Start Reading
                </button>
              </div>
            </div>
          </div>
        </div>

        {/* Gradient overlay at bottom */}
        <div className="absolute -bottom-2 left-0 right-0 h-32 bg-gradient-to-t from-[#1f1f1f] to-transparent" />
      </div>
    </>
  );
}
