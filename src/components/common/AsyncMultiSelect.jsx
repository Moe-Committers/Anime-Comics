import { useState, useRef } from "react";
import { X, ChevronDown, Search, Loader2 } from "lucide-react";
import { useClickOutside } from "@/hooks/useClickOutside";

export function AsyncMultiSelect({
  selectedItems = [],
  onItemSelect,
  onItemRemove,
  onSearch,
  loading = false,
  items = [],
  placeholder = "Select items...",
}) {
  const [isOpen, setIsOpen] = useState(false);
  const [searchTerm, setSearchTerm] = useState("");
  const dropdownRef = useRef(null);
  const searchTimeout = useRef(null);

  useClickOutside(dropdownRef, () => setIsOpen(false));

  const handleSearch = (value) => {
    setSearchTerm(value);
    if (searchTimeout.current) {
      clearTimeout(searchTimeout.current);
    }
    searchTimeout.current = setTimeout(() => {
      onSearch(value);
    }, 300);
  };

  return (
    <div className="relative" ref={dropdownRef}>
      <div
        className="min-h-[42px] px-3 py-2 bg-gray-700 text-white rounded-lg cursor-pointer focus:outline-none focus:ring-2 focus:ring-blue-500"
        onClick={() => setIsOpen(!isOpen)}
      >
        <div className="flex flex-wrap gap-2">
          {selectedItems.map((item) => (
            <div
              key={item.id}
              className="flex items-center gap-1 px-2 py-1 bg-gray-600 rounded text-sm"
            >
              <span className="max-w-[150px] truncate">{item.name}</span>
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
              {selectedItems.length === 0 && placeholder}
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
        <div className="absolute z-50 w-full mt-1 bg-gray-800 rounded-lg shadow-lg overflow-hidden border border-gray-700">
          <div className="p-2">
            <div className="relative">
              <input
                type="text"
                value={searchTerm}
                onChange={(e) => handleSearch(e.target.value)}
                placeholder="Search..."
                className="w-full px-8 py-2 bg-gray-700 text-white rounded focus:outline-none focus:ring-1 focus:ring-blue-500 text-sm"
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
                {loading ? "Loading..." : "No items found"}
              </div>
            ) : (
              <div className="py-1">
                {items.map((item) => (
                  <button
                    key={item.id}
                    onClick={() => {
                      onItemSelect(item);
                      setSearchTerm("");
                    }}
                    className="w-full px-4 py-2 text-sm text-left text-white hover:bg-gray-700 focus:outline-none"
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
