import { useState, useRef, useEffect } from "react";
import { X, ChevronDown, Search, Loader2 } from "lucide-react";
import { useClickOutside } from "@/hooks/useClickOutside";
import { categoryService } from "@/services/category.service";

export function MultiSelect({
  selectedItems = [],
  onItemSelect,
  onItemRemove,
}) {
  const [isOpen, setIsOpen] = useState(false);
  const [searchTerm, setSearchTerm] = useState("");
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(false);
  const dropdownRef = useRef(null);
  const searchTimeout = useRef(null);

  useClickOutside(dropdownRef, () => setIsOpen(false));

  const fetchItems = async (search = "") => {
    try {
      setLoading(true);
      const response = await categoryService.getCategories({
        search,
        page: 1,
        pageSize: 10,
      });

      if (response.success) {
        const filteredItems = response.data.filter(
          (item) => !selectedItems.some((selected) => selected.id === item.id)
        );
        setItems(filteredItems);
      }
    } catch (error) {
      console.error("Failed to fetch categories:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (isOpen) {
      fetchItems(searchTerm);
    }
  }, [isOpen]);

  const handleSearch = (value) => {
    setSearchTerm(value);

    if (searchTimeout.current) {
      clearTimeout(searchTimeout.current);
    }

    searchTimeout.current = setTimeout(() => {
      fetchItems(value);
    }, 300);
  };

  return (
    <div className="relative" ref={dropdownRef}>
      <div
        className="min-h-[42px] px-3 py-2 bg-gray-700 rounded-lg cursor-pointer focus:outline-none focus:ring-2 focus:ring-blue-500"
        onClick={() => setIsOpen(!isOpen)}
      >
        <div className="flex flex-wrap gap-2">
          {selectedItems.map((item) => (
            <div
              key={item.id}
              className="flex items-center gap-1 px-2 py-1 bg-gray-600 rounded text-sm text-white"
            >
              {item.name}
              <button
                onClick={(e) => {
                  e.stopPropagation();
                  onItemRemove(item);
                }}
                className="text-gray-400 hover:text-white"
              >
                <X className="h-3 w-3" />
              </button>
            </div>
          ))}
          <div className="flex items-center justify-between flex-1 min-w-[100px]">
            <span className="text-gray-400 text-sm">
              {selectedItems.length === 0 && "Select categories..."}
            </span>
            <ChevronDown
              className={`h-4 w-4 text-gray-400 transition-transform ${
                isOpen ? "rotate-180" : ""
              }`}
            />
          </div>
        </div>
      </div>

      {isOpen && (
        <div className="absolute z-50 w-full mt-1 bg-gray-800 rounded-lg shadow-lg overflow-hidden">
          <div className="p-2">
            <div className="relative">
              <input
                type="text"
                value={searchTerm}
                onChange={(e) => handleSearch(e.target.value)}
                placeholder="Search categories..."
                className="w-full px-8 py-2 bg-gray-700 text-white rounded focus:outline-none focus:ring-1 focus:ring-blue-500"
              />
              <Search className="absolute left-2 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400" />
              {loading && (
                <Loader2 className="absolute right-2 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400 animate-spin" />
              )}
            </div>
          </div>

          <div className="max-h-60 overflow-y-auto">
            {items.length === 0 ? (
              <div className="px-4 py-3 text-sm text-gray-400">
                {loading ? "Loading..." : "No categories found"}
              </div>
            ) : (
              <div className="py-1">
                {items.map((item) => (
                  <button
                    key={item.id}
                    onClick={() => {
                      onItemSelect(item);
                      setSearchTerm("");
                      fetchItems("");
                    }}
                    className="w-full px-4 py-2 text-sm text-left text-white hover:bg-gray-700"
                  >
                    {item.name}
                  </button>
                ))}
              </div>
            )}
          </div>
        </div>
      )}
    </div>
  );
}
