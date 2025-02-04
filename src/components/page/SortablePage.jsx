import { memo } from "react";
import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import { GripVertical, Pencil } from "lucide-react";

export const SortablePage = memo(function SortablePage({
  page,
  selected,
  onSelect,
  onEdit,
}) {
  const {
    attributes,
    listeners,
    setNodeRef,
    transform,
    transition,
    isDragging,
  } = useSortable({ id: page.id });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    zIndex: isDragging ? 50 : undefined,
    opacity: isDragging ? 0.5 : undefined,
  };

  return (
    <div ref={setNodeRef} style={style} className="relative group touch-none">
      <div
        className={`relative aspect-[2/3] rounded-lg overflow-hidden border-2 ${
          selected
            ? "border-blue-500"
            : "border-transparent hover:border-gray-500"
        }`}
      >
        <img
          src={`${process.env.NEXT_PUBLIC_API_URL}${page.imageUrl}`}
          alt={`Page ${page.pageNumber}`}
          className="w-full h-full object-cover"
          draggable={false}
        />
        <div className="absolute inset-0 bg-black/50 opacity-0 group-hover:opacity-100 transition-opacity">
          <div className="absolute top-2 left-2">
            <div {...attributes} {...listeners}>
              <GripVertical className="h-5 w-5 text-white cursor-grab active:cursor-grabbing" />
            </div>
          </div>
          <div className="absolute top-2 right-2 flex gap-2">
            <button
              onClick={() => onEdit(page)}
              className="p-1 text-white hover:text-blue-400"
            >
              <Pencil className="h-4 w-4" />
            </button>
            <input
              type="checkbox"
              checked={selected}
              onChange={(e) => onSelect(e.target.checked)}
              className="h-4 w-4"
            />
          </div>
          <div className="absolute bottom-2 left-2 text-white text-sm">
            Page {page.pageNumber}
          </div>
        </div>
      </div>
    </div>
  );
});
