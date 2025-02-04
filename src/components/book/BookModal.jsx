import { useState, useEffect } from "react";
import { toast, Toaster } from "sonner";
import { X, Upload } from "lucide-react";
import { bookService } from "@/services/book.service";
import { categoryService } from "@/services/category.service";
import { AsyncMultiSelect } from "../common/AsyncMultiSelect";

export function BookModal({ book = null, onClose, onSuccess }) {
  const [formData, setFormData] = useState({
    title: "",
    description: "",
    author: "",
    categoryIds: [],
    imageUrl: null,
  });
  const [preview, setPreview] = useState(null);
  const [loading, setLoading] = useState(false);
  const [initialLoading, setInitialLoading] = useState(book !== null);
  const [categories, setCategories] = useState([]);
  const [selectedCategories, setSelectedCategories] = useState([]);
  const [loadingCategories, setLoadingCategories] = useState(false);

  useEffect(() => {
    async function fetchBookDetails() {
      try {
        const response = await bookService.getBook(book.id);
        if (!response.success) {
          throw new Error(response.message);
        }

        setFormData({
          title: response.data.title,
          description: response.data.description,
          author: response.data.author,
          categoryIds: response.data.categories.map((c) => c.id),
          imageUrl: null,
        });
        setSelectedCategories(response.data.categories);
        setPreview(
          `${process.env.NEXT_PUBLIC_API_URL}${response.data.imageUrl}`
        );
        setInitialLoading(false);
      } catch (error) {
        toast.error("Failed to fetch book details");
        onClose();
      }
    }

    if (book) {
      fetchBookDetails();
    }
  }, [book]);

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setFormData((prev) => ({ ...prev, imageUrl: file }));
      const reader = new FileReader();
      reader.onloadend = () => {
        setPreview(reader.result);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleSearchCategories = async (search) => {
    try {
      setLoadingCategories(true);
      const response = await categoryService.getCategories({
        search,
        page: 1,
        pageSize: 10,
      });

      if (response.success) {
        const filteredCategories = response.data.filter(
          (category) =>
            !selectedCategories.some((selected) => selected.id === category.id)
        );
        setCategories(filteredCategories);
      }
    } catch (error) {
      console.error("Failed to fetch categories:", error);
    } finally {
      setLoadingCategories(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      const submitData = new FormData();
      submitData.append("title", formData.title);
      submitData.append("description", formData.description);
      submitData.append("author", formData.author);
      formData.categoryIds.forEach((id) => {
        submitData.append("categoryIds", id);
      });
      if (formData.imageUrl) {
        submitData.append("imageUrl", formData.imageUrl);
      }

      const response = book
        ? await bookService.updateBook(book.id, submitData)
        : await bookService.createBook(submitData);

      if (!response.success) {
        throw new Error(response.message);
      }

      toast.success(`Book ${book ? "updated" : "created"} successfully`);
      onSuccess();
    } catch (error) {
      toast.error(
        error.message || `Failed to ${book ? "update" : "create"} book`
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
            {book ? "Edit Book" : "New Book"}
          </h2>
          <button onClick={onClose} className="text-gray-400 hover:text-white">
            <X className="h-5 w-5" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
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

            <div>
              <label className="block text-sm font-medium text-gray-300 mb-1">
                Author
              </label>
              <input
                type="text"
                value={formData.author}
                onChange={(e) =>
                  setFormData((prev) => ({ ...prev, author: e.target.value }))
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
              Categories
            </label>
            <AsyncMultiSelect
              selectedItems={selectedCategories}
              items={categories}
              loading={loadingCategories}
              placeholder="Select categories..."
              onSearch={handleSearchCategories}
              onItemSelect={(category) => {
                setSelectedCategories((prev) => [...prev, category]);
                setFormData((prev) => ({
                  ...prev,
                  categoryIds: [...prev.categoryIds, category.id],
                }));
                setCategories((prev) =>
                  prev.filter((c) => c.id !== category.id)
                );
              }}
              onItemRemove={(category) => {
                setSelectedCategories((prev) =>
                  prev.filter((c) => c.id !== category.id)
                );
                setFormData((prev) => ({
                  ...prev,
                  categoryIds: prev.categoryIds.filter(
                    (id) => id !== category.id
                  ),
                }));
              }}
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
              {loading ? "Saving..." : book ? "Update" : "Create"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
