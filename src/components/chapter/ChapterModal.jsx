import { useState, useEffect } from "react";
import { toast, Toaster } from "sonner";
import { X } from "lucide-react";
import { chapterService } from "@/services/chapter.service";

export function ChapterModal({ volumeId, chapter = null, onClose, onSuccess }) {
  const [formData, setFormData] = useState({
    chapNo: 1,
    title: "",
    description: "",
    releaseDate: "",
  });
  const [loading, setLoading] = useState(false);
  const [initialLoading, setInitialLoading] = useState(chapter !== null);

  useEffect(() => {
    async function fetchChapterDetails() {
      try {
        const response = await chapterService.getChapter(chapter.id);
        if (!response.success) {
          throw new Error(response.message);
        }

        setFormData({
          chapNo: response.data.chapNo,
          title: response.data.title,
          description: response.data.description,
          releaseDate: response.data.releaseDate
            ? new Date(response.data.releaseDate).toISOString().split("T")[0]
            : "",
        });

        setInitialLoading(false);
      } catch (error) {
        toast.error("Failed to fetch chapter details");
        onClose();
      }
    }

    if (chapter) {
      fetchChapterDetails();
    }
  }, [chapter]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      const submitData = {
        chapNo: parseInt(formData.chapNo),
        title: formData.title,
        description: formData.description,
        releaseDate: formData.releaseDate
          ? new Date(formData.releaseDate).toISOString()
          : null,
      };

      const response = chapter
        ? await chapterService.updateChapter(volumeId, chapter.id, submitData)
        : await chapterService.createChapter(volumeId, submitData);

      if (!response.success) {
        throw new Error(response.message);
      }

      toast.success(`Chapter ${chapter ? "updated" : "created"} successfully`);
      onSuccess();
    } catch (error) {
      toast.error(
        error.message || `Failed to ${chapter ? "update" : "create"} chapter`
      );
    } finally {
      setLoading(false);
    }
  };

  if (initialLoading) {
    return (
      <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
        <div className="bg-gray-800 rounded-lg p-6 w-full max-w-2xl">
          <div className="flex justify-center items-center h-64">
            <div className="w-8 h-8 border-4 border-blue-500 border-t-transparent rounded-full animate-spin" />
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
      <Toaster/>
      <div className="bg-gray-800 rounded-lg p-6 w-full max-w-2xl">
        <div className="flex justify-between items-center mb-4">
          <h2 className="text-xl font-bold text-white">
            {chapter ? "Edit Chapter" : "New Chapter"}
          </h2>
          <button onClick={onClose} className="text-gray-400 hover:text-white">
            <X className="h-5 w-5" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-300 mb-1">
                Chapter Number
              </label>
              <input
                type="number"
                value={formData.chapNo}
                onChange={(e) =>
                  setFormData((prev) => ({
                    ...prev,
                    chapNo: parseInt(e.target.value),
                  }))
                }
                min="1"
                className="w-full px-3 py-2 bg-gray-700 text-white rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                required
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-300 mb-1">
                Title
              </label>
              <input
                type="text"
                value={formData.title}
                onChange={(e) =>
                  setFormData((prev) => ({ ...prev, title: e.target.value }))
                }
                className="w-full px-3 py-2 bg-gray-700 text-white rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                required
              />
            </div>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-300 mb-1">
              Description
            </label>
            <textarea
              value={formData.description}
              onChange={(e) =>
                setFormData((prev) => ({
                  ...prev,
                  description: e.target.value,
                }))
              }
              rows={4}
              className="w-full px-3 py-2 bg-gray-700 text-white rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-300 mb-1">
              Release Date
            </label>
            <input
              type="date"
              value={formData.releaseDate}
              onChange={(e) =>
                setFormData((prev) => ({
                  ...prev,
                  releaseDate: e.target.value,
                }))
              }
              className="w-full px-3 py-2 bg-gray-700 text-white rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>

          <div className="flex justify-end gap-3">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 text-gray-400 hover:text-white"
              disabled={loading}
            >
              Cancel
            </button>
            <button
              type="submit"
              className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50"
              disabled={loading}
            >
              {loading ? "Saving..." : chapter ? "Update" : "Create"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
