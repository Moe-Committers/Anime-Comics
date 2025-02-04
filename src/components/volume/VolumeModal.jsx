import { useState, useEffect } from "react";
import { toast, Toaster } from "sonner";
import { X, Upload } from "lucide-react";
import { volumeService } from "@/services/volume.service";

export function VolumeModal({ bookId, volume = null, onClose, onSuccess }) {
  const [formData, setFormData] = useState({
    volumeNo: 1,
    title: "",
    description: "",
    releaseDate: "",
    coverImg: null,
  });
  const [preview, setPreview] = useState(null);
  const [loading, setLoading] = useState(false);
  const [initialLoading, setInitialLoading] = useState(volume !== null);

  useEffect(() => {
    async function fetchVolumeDetails() {
      try {
        const response = await volumeService.getVolume(volume.id);
        if (!response.success) {
          throw new Error(response.message);
        }

        setFormData({
          volumeNo: response.data.volumeNo,
          title: response.data.title,
          description: response.data.description,
          releaseDate: response.data.releaseDate
            ? new Date(response.data.releaseDate).toISOString().split("T")[0]
            : "",
          coverImg: null,
        });

        if (response.data.coverImg) {
          setPreview(
            `${process.env.NEXT_PUBLIC_API_URL}${response.data.coverImg}`
          );
        }

        setInitialLoading(false);
      } catch (error) {
        toast.error("Failed to fetch volume details");
        onClose();
      }
    }

    if (volume) {
      fetchVolumeDetails();
    }
  }, [volume]);

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setFormData((prev) => ({ ...prev, coverImg: file }));
      const reader = new FileReader();
      reader.onloadend = () => {
        setPreview(reader.result);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      const submitData = new FormData();
      
      submitData.append("volumeNo", formData.volumeNo.toString());
      submitData.append("title", formData.title);
      submitData.append("description", formData.description);

      if (formData.releaseDate) {
        submitData.append(
          "releaseDate",
          new Date(formData.releaseDate).toISOString()
        );
      }

      if (formData.coverImg) {
        submitData.append("coverImg", formData.coverImg);
      }

      const response = volume
        ? await volumeService.updateVolume(bookId, volume.id, submitData)
        : await volumeService.createVolume(bookId, submitData);

      if (!response.success) {
        throw new Error(response.message || "Operation failed");
      }

      toast.success(`Volume ${volume ? "updated" : "created"} successfully`);
      onSuccess();
    } catch (error) {
      console.error("Volume submission error:", error);

      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        `Failed to ${volume ? "update" : "create"} volume`;

      toast.error(errorMessage);
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
            {volume ? "Edit Volume" : "New Volume"}
          </h2>
          <button onClick={onClose} className="text-gray-400 hover:text-white">
            <X className="h-5 w-5" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-300 mb-1">
                Volume Number
              </label>
              <input
                type="number"
                value={formData.volumeNo}
                onChange={(e) =>
                  setFormData((prev) => ({
                    ...prev,
                    volumeNo: parseInt(e.target.value),
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

          <div>
            <label className="block text-sm font-medium text-gray-300 mb-1">
              Cover Image
            </label>
            <div className="flex items-center gap-4">
              {preview && (
                <img
                  src={preview}
                  alt="Preview"
                  className="h-32 w-24 object-cover rounded"
                />
              )}
              <label className="flex items-center gap-2 px-4 py-2 bg-gray-700 text-white rounded-lg cursor-pointer hover:bg-gray-600">
                <Upload className="h-5 w-5" />
                Choose Cover
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
              disabled={loading}
            >
              {loading ? "Saving..." : volume ? "Update" : "Create"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
