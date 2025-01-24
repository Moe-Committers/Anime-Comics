import { useState } from "react";
import {
  ChevronFirst,
  ChevronLast,
  ChevronLeft,
  ChevronRight,
} from "lucide-react";

export function Pagination({
  currentPage,
  totalPages,
  onPageChange,
  showGoToPage = true,
}) {
  const [goToPage, setGoToPage] = useState("");

  const handleGoToPage = (e) => {
    e.preventDefault();
    const page = parseInt(goToPage);
    if (page >= 1 && page <= totalPages) {
      onPageChange(page);
      setGoToPage("");
    }
  };

  const getPageNumbers = () => {
    const pageNumbers = [];
    const maxVisiblePages = 5;

    if (totalPages <= maxVisiblePages) {
      return Array.from({ length: totalPages }, (_, i) => i + 1);
    }

    pageNumbers.push(1);

    let startPage = Math.max(2, currentPage - 1);
    let endPage = Math.min(totalPages - 1, currentPage + 1);

    if (currentPage <= 3) {
      endPage = 4;
    }

    if (currentPage >= totalPages - 2) {
      startPage = totalPages - 3;
    }

    if (startPage > 2) {
      pageNumbers.push("...");
    }

    for (let i = startPage; i <= endPage; i++) {
      pageNumbers.push(i);
    }

    if (endPage < totalPages - 1) {
      pageNumbers.push("...");
    }

    if (totalPages > 1) {
      pageNumbers.push(totalPages);
    }

    return pageNumbers;
  };

  return (
    <div className="flex items-center justify-center gap-2">
      <button
        onClick={() => onPageChange(1)}
        disabled={currentPage === 1}
        className="p-2 text-gray-400 hover:text-white disabled:opacity-50 disabled:cursor-not-allowed"
      >
        <ChevronFirst className="h-4 w-4" />
      </button>

      <button
        onClick={() => onPageChange(currentPage - 1)}
        disabled={currentPage === 1}
        className="p-2 text-gray-400 hover:text-white disabled:opacity-50 disabled:cursor-not-allowed"
      >
        <ChevronLeft className="h-4 w-4" />
      </button>

      <div className="flex items-center gap-1">
        {getPageNumbers().map((page, index) =>
          page === "..." && showGoToPage ? (
            <form
              key={`ellipsis-${index}`}
              onSubmit={handleGoToPage}
              className="inline-flex items-center"
            >
              <input
                type="number"
                min="1"
                max={totalPages}
                value={goToPage}
                onChange={(e) => setGoToPage(e.target.value)}
                className="w-16 h-8 bg-gray-700 text-white text-sm rounded px-2 focus:outline-none focus:ring-1 focus:ring-blue-500"
                placeholder="Go to"
              />
            </form>
          ) : (
            <button
              key={`page-${page}-${index}`}
              onClick={() => typeof page === "number" && onPageChange(page)}
              className={`h-8 w-8 flex items-center justify-center rounded text-sm
                ${
                  currentPage === page
                    ? "bg-orange-500 hover:bg-orange-600 text-white"
                    : "text-gray-400 hover:text-white hover:bg-gray-700"
                }
                ${typeof page !== "number" ? "cursor-default" : ""}
              `}
            >
              {page}
            </button>
          )
        )}
      </div>

      <button
        onClick={() => onPageChange(currentPage + 1)}
        disabled={currentPage === totalPages}
        className="p-2 text-gray-400 hover:text-white disabled:opacity-50 disabled:cursor-not-allowed"
      >
        <ChevronRight className="h-4 w-4" />
      </button>

      <button
        onClick={() => onPageChange(totalPages)}
        disabled={currentPage === totalPages}
        className="p-2 text-gray-400 hover:text-white disabled:opacity-50 disabled:cursor-not-allowed"
      >
        <ChevronLast className="h-4 w-4" />
      </button>
    </div>
  );
}
