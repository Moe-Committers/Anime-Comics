import { ChevronDown, ChevronUp } from "lucide-react";
import { useState } from "react";

export function Chapters({ volumes }) {
  const [expandedVolumes, setExpandedVolumes] = useState(new Set());

  const toggleVolume = (volumeId) => {
    setExpandedVolumes((prev) => {
      const newSet = new Set(prev);
      if (newSet.has(volumeId)) {
        newSet.delete(volumeId);
      } else {
        newSet.add(volumeId);
      }
      return newSet;
    });
  };

  return (
    <div className="space-y-4">
      {volumes.map((volume) => (
        <div key={volume.id} className="bg-[#2a2a2a] rounded-lg">
          <button
            onClick={() => toggleVolume(volume.id)}
            className="w-full px-6 py-4 flex items-center justify-between text-white hover:bg-[#363636] rounded-lg"
          >
            <div className="flex items-center gap-4">
              <div className="text-left">
                <div className="font-medium">Volume {volume.number}</div>
                <div className="text-sm text-gray-400">
                  {volume.chapters.length} Chapters
                </div>
              </div>
            </div>
            {expandedVolumes.has(volume.id) ? (
              <ChevronUp className="h-5 w-5" />
            ) : (
              <ChevronDown className="h-5 w-5" />
            )}
          </button>

          {expandedVolumes.has(volume.id) && (
            <div className="px-6 pb-4 space-y-2">
              {volume.chapters.map((chapter) => (
                <button
                  key={chapter.id}
                  className="w-full px-4 py-2 text-left text-gray-300 hover:bg-[#363636] rounded flex items-center justify-between"
                >
                  <span>
                    Chapter {chapter.number}: {chapter.title}
                  </span>
                  <div className="flex items-center gap-4">
                    <span className="text-gray-500 text-sm">
                      {chapter.pagesCount} Pages
                    </span>
                    <time className="text-gray-500 text-sm">
                      {chapter.uploadDate}
                    </time>
                  </div>
                </button>
              ))}
            </div>
          )}
        </div>
      ))}
    </div>
  );
}
