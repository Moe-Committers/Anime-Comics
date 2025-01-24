export function DotPagination({ total, current, onClick }) {
  const getVisibleDots = () => {
    let dots = [];
    if (total <= 5) {
      dots = Array.from({ length: total }, (_, i) => i);
    } else {
      let start = current - 2;
      let end = current + 2;

      if (start < 0) {
        start = 0;
        end = 4;
      }
      if (end >= total) {
        end = total - 1;
        start = total - 5;
      }

      dots = Array.from({ length: 5 }, (_, i) => start + i);
    }
    return dots;
  };

  const getDotSize = (index) => {
    const distance = Math.abs(2 - dots.indexOf(index));
    switch (distance) {
      case 0: return 'w-2.5 h-2.5';
      case 1: return 'w-2 h-2';
      case 2: return 'w-1.5 h-1.5';
      default: return 'w-1.5 h-1.5';
    }
  };

  const dots = getVisibleDots();

  return (
    <div className="flex items-center justify-center gap-2 mt-6">
      {dots.map((dotIndex) => (
        <button
          key={dotIndex}
          onClick={() => onClick(dotIndex)}
          className="p-1"
        >
          <div
            className={`${getDotSize(dotIndex)} rounded-full transition-all duration-300 
              ${dotIndex === current ? 'bg-white' : 'bg-gray-600 hover:bg-gray-400'}`}
          />
        </button>
      ))}
    </div>
  );
}
