import { useEffect, useRef, useState } from "react";
import { toast, Toaster } from "sonner";
import {
  DndContext,
  closestCenter,
  KeyboardSensor,
  PointerSensor,
  useSensor,
  useSensors,
} from "@dnd-kit/core";
import {
  arrayMove,
  SortableContext,
  sortableKeyboardCoordinates,
  rectSortingStrategy,
} from "@dnd-kit/sortable";
import { SortablePage } from "./SortablePage";
import { Upload } from "lucide-react";
import { pageService } from "@/services/page.service";
import { useChapterPages } from "@/hooks/useChapterPages";
import { EditPageModal } from "./EditPageModal";

export function PageManager({ chapterId }) {
  const [selectedPages, setSelectedPages] = useState([]);
  const [uploading, setUploading] = useState(false);
  const [editingPage, setEditingPage] = useState(null);
  const [newOrder, setNewOrder] = useState([]);
  const { pages, loading, pagination, hasMore, loadMore, refetch } =
    useChapterPages(chapterId);
  const observerTarget = useRef(null);

  const sensors = useSensors(
    useSensor(PointerSensor),
    useSensor(KeyboardSensor, {
      coordinateGetter: sortableKeyboardCoordinates,
    })
  );

  useEffect(() => {
    const observer = new IntersectionObserver(
      (entries) => {
        if (entries[0].isIntersecting && hasMore) {
          loadMore();
        }
      },
      { threshold: 0.5 }
    );

    if (observerTarget.current) {
      observer.observe(observerTarget.current);
    }

    return () => observer.disconnect();
  }, [loadMore, hasMore]);

  const handleFileUpload = async (event) => {
    const files = Array.from(event.target.files);
    if (files.length === 0) return;

    const validFiles = files.filter((file) => {
      const isValid = file.type.startsWith("image/");
      if (!isValid) {
        toast.error(`${file.name} is not an image file`);
      }
      return isValid;
    });

    if (validFiles.length === 0) {
      toast.error("No valid image files selected");
      return;
    }

    try {
      setUploading(true);
      const response = await pageService.addPages(chapterId, validFiles);

      if (!response.success) {
        throw new Error(response.message || "Failed to upload pages");
      }

      toast.success(`Successfully uploaded ${validFiles.length} pages`);
      await refetch();
    } catch (error) {
      console.error("Upload error:", error);
      toast.error(
        error.response?.data?.message ||
          error.message ||
          "Failed to upload pages"
      );
    } finally {
      setUploading(false);
      event.target.value = "";
    }
  };

  const handleDelete = async () => {
    if (selectedPages.length === 0) return;

    if (!confirm("Are you sure you want to delete the selected pages?")) return;

    try {
      const response = await pageService.deletePages(chapterId, selectedPages);

      if (!response.success) {
        throw new Error(response.message);
      }

      toast.success("Pages deleted successfully");
      setSelectedPages([]);
      await refetch();
    } catch (error) {
      console.error("Delete error:", error);
      toast.error(error.message || "Failed to delete pages");
    }
  };

  const handleDragEnd = async (event) => {
    const { active, over } = event;
    if (!over || active.id === over.id) return;

    try {
      const oldIndex = pages.findIndex((p) => p.id === active.id);
      const newIndex = pages.findIndex((p) => p.id === over.id);

      if (oldIndex === -1 || newIndex === -1) return;

      const reorderedPages = [...pages];
      const [movedPage] = reorderedPages.splice(oldIndex, 1);
      reorderedPages.splice(newIndex, 0, movedPage);

      const updatedOrder = reorderedPages.map((page, index) => ({
        pageId: page.id,
        newPageNumber: index + 1,
      }));

      const response = await pageService.reorderPages(chapterId, {
        newOrder: updatedOrder,
      });

      if (!response.success) {
        throw new Error(response.message);
      }

      await refetch();
    } catch (error) {
      console.error("Reorder error:", error);
      toast.error("Failed to reorder pages");
    }
  };

  if (loading && pages.length === 0) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="w-8 h-8 border-4 border-blue-500 border-t-transparent rounded-full animate-spin" />
      </div>
    );
  }

  return (
    <div className="space-y-4">
      <Toaster />
      <div className="flex justify-between items-center">
        <div>
          <h2 className="text-xl font-bold text-white">Pages</h2>
          {pagination && (
            <p className="text-sm text-gray-400">
              Total {pagination.totalItems} pages
            </p>
          )}
        </div>
        <div className="flex gap-2">
          {selectedPages.length > 0 && (
            <button
              onClick={handleDelete}
              className="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors"
            >
              Delete Selected ({selectedPages.length})
            </button>
          )}
          <label className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 cursor-pointer transition-colors">
            <Upload className="h-5 w-5 inline-block mr-2" />
            Upload Pages
            <input
              type="file"
              multiple
              accept="image/*"
              onChange={handleFileUpload}
              className="hidden"
              disabled={uploading}
            />
          </label>
        </div>
      </div>

      <DndContext
        sensors={sensors}
        collisionDetection={closestCenter}
        onDragEnd={handleDragEnd}
      >
        <SortableContext
          items={pages.map((page) => page.id)}
          strategy={rectSortingStrategy}
        >
          <div className="grid grid-cols-6 gap-4">
            {pages
              .map((page) => {
                const newPosition = newOrder.find((o) => o.pageId === page.id);
                return {
                  ...page,
                  currentNumber: newPosition
                    ? newPosition.newPageNumber
                    : page.pageNumber,
                };
              })
              .sort((a, b) => a.currentNumber - b.currentNumber)
              .map((page) => (
                <SortablePage
                  key={page.id}
                  page={page}
                  selected={selectedPages.includes(page.id)}
                  onSelect={(selected) => {
                    setSelectedPages((prev) =>
                      selected
                        ? [...prev, page.id]
                        : prev.filter((id) => id !== page.id)
                    );
                  }}
                  onEdit={() => setEditingPage(page)}
                />
              ))}
          </div>
        </SortableContext>
      </DndContext>

      {hasMore && (
        <div ref={observerTarget} className="flex justify-center py-4">
          <div className="w-8 h-8 border-4 border-blue-500 border-t-transparent rounded-full animate-spin" />
        </div>
      )}

      {editingPage && (
        <EditPageModal
          page={editingPage}
          chapterId={chapterId}
          onClose={() => setEditingPage(null)}
          onSuccess={() => {
            setEditingPage(null);
            refetch();
          }}
        />
      )}
    </div>
  );
}
