import { useState } from "react";
import { ChevronDown, ChevronUp, Star, Library, BookOpen } from "lucide-react";
import { Chapters } from "@/components/tab/Chapters";
import { Comments } from "@/components/tab/Comments";
import { Art } from "@/components/tab/Arts";
import { MangaHero } from "./MangaHero";

export function MangaDetailPage({ manga }) {
  const [activeTab, setActiveTab] = useState("chapters");

  const tabs = [
    { id: "chapters", label: "Chapters" },
    { id: "comments", label: `Comments (${manga.commentsCount || 0})` },
    { id: "art", label: "Art" },
  ];

  return (
    <div className="min-h-screen bg-[#1f1f1f]">
      {/* Hero Section */}
      <MangaHero manga={manga} />

      {/* Content Section with Tabs */}
      <div className="relative z-10 h-screen bg-[#1f1f1f] ">
        <div className="max-w-7xl relative z-10 mx-auto px-4 sm:px-6 lg:px-8 pt-[65px] pb-8">
          <div className="flex gap-8">
            {/* Left Sidebar - Info & Tabs */}
            <div className="w-64 flex-shrink-0">
              <div className="bg-[#2a2a2a] rounded-lg p-4 mb-6">
                <h3 className="text-white font-medium mb-2">Information</h3>
                <div className="space-y-2 text-sm">
                  <div className="flex justify-between">
                    <span className="text-gray-400">Author</span>
                    <span className="text-white">{manga.author}</span>
                  </div>
                  <div className="flex justify-between">
                    <span className="text-gray-400">Artist</span>
                    <span className="text-white">{manga.artist}</span>
                  </div>
                  <div className="flex justify-between">
                    <span className="text-gray-400">Status</span>
                    <span className="text-white">{manga.status}</span>
                  </div>
                  <div className="flex justify-between">
                    <span className="text-gray-400">Volumes</span>
                    <span className="text-white">{manga.volumes.length}</span>
                  </div>
                </div>
              </div>

              <div className="bg-[#2a2a2a] rounded-lg p-4 mb-6">
                <h3 className="text-white font-medium mb-2">Genres</h3>
                <div className="flex flex-wrap gap-2">
                  {manga.genres.map((genre) => (
                    <span
                      key={genre}
                      className="px-2 py-1 bg-[#363636] text-white text-xs rounded"
                    >
                      {genre}
                    </span>
                  ))}
                </div>
              </div>

              <div className="space-y-1">
                {tabs.map((tab) => (
                  <button
                    key={tab.id}
                    onClick={() => setActiveTab(tab.id)}
                    className={`w-full px-4 py-2 text-left rounded-lg transition-colors ${
                      activeTab === tab.id
                        ? "bg-[#2a2a2a] text-white"
                        : "text-gray-400 hover:bg-[#2a2a2a] hover:text-white"
                    }`}
                  >
                    {tab.label}
                  </button>
                ))}
              </div>
            </div>

            <div className="flex-1">
              {activeTab === "chapters" && <Chapters volumes={manga.volumes} />}
              {activeTab === "comments" && <Comments />}
              {activeTab === "art" && <Art />}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
