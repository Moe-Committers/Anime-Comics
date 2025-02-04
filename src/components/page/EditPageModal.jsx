import { useState } from "react";
import { toast, Toaster } from "sonner";
import { X, Upload } from "lucide-react";
import { pageService } from "@/services/page.service";

export function EditPageModal({ page, chapterId, onClose, onSuccess }) {
  const [preview, setPreview] = useState(
    `${process.env.NEXT_PUBLIC_API_URL}${page.imageUrl}`
  );
  const [imageFile, setImageFile] = useState(null);
  const [loading, setLoading] = useState(false);

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setImageFile(file);
      const reader = new FileReader();
      reader.onloadend = () => {
        setPreview(reader.result);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!imageFile) {
      onClose();
      return;
    }

    try {
      setLoading(true);
      const formData = new FormData();
      formData.append("image", imageFile);
      formData.append("pageNumber", page.pageNumber.toString());

      const response = await pageService.updatePage(
        chapterId,
        page.id,
        formData
      );
      if (!response.success) {
        throw new Error(response.message);
      }

      toast.success("Page updated successfully");
      onSuccess();
    } catch (error) {
      console.error("Update error:", error);
      toast.error(
        error.response?.data?.message ||
          error.message ||
          "Failed to update page"
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
      <Toaster/>
      <div className="bg-gray-800 rounded-lg p-6 w-full max-w-md">
        <div className="flex justify-between items-center mb-4">
          <h2 className="text-xl font-bold text-white">
            Edit Page {page.pageNumber}
          </h2>
          <button onClick={onClose} className="text-gray-400 hover:text-white">
            <X className="h-5 w-5" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-300 mb-2">
              Page Image
            </label>
            <div className="flex flex-col gap-4">
              <img
                src={preview}
                alt={`Page ${page.pageNumber}`}
                className="w-full aspect-[2/3] object-cover rounded"
              />
              <label className="flex items-center gap-2 px-4 py-2 bg-gray-700 text-white rounded-lg cursor-pointer hover:bg-gray-600 text-center">
                <Upload className="h-5 w-5" />
                Choose New Image
                <input
                  type="file"
                  accept="image/*"
                  onChange={handleImageChange}
                  className="hidden"
                />
              </label>
            </div>
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
              disabled={loading || !imageFile}
            >
              {loading ? "Saving..." : "Update"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
