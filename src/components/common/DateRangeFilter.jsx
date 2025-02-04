import { CalendarRange, X } from "lucide-react";
import { useState } from "react";

export function DateRangeFilter({ onApply, onClear }) {
  const [fromDate, setFromDate] = useState("");
  const [toDate, setToDate] = useState("");

  const handleApply = () => {
    if (fromDate && toDate) {
      onApply(fromDate, toDate);
    }
  };

  const handleClear = () => {
    setFromDate("");
    setToDate("");
    onClear();
  };

  return (
    <div className="flex items-center gap-2">
      <CalendarRange className="h-5 w-5 text-gray-400" />
      <input
        type="date"
        value={fromDate}
        onChange={(e) => setFromDate(e.target.value)}
        className="px-3 py-2 bg-gray-700 text-white rounded-lg focus:outline-none text-sm"
      />
      <span className="text-gray-400">to</span>
      <input
        type="date"
        value={toDate}
        onChange={(e) => setToDate(e.target.value)}
        className="px-3 py-2 bg-gray-700 text-white rounded-lg focus:outline-none text-sm"
      />
      <button
        onClick={handleApply}
        disabled={!fromDate || !toDate}
        className="px-3 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed text-sm"
      >
        Apply
      </button>
      {(fromDate || toDate) && (
        <button
          onClick={handleClear}
          className="p-2 text-gray-400 hover:text-white"
        >
          <X className="h-5 w-5" />
        </button>
      )}
    </div>
  );
}
