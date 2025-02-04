import { useState, useRef } from 'react';
import { ChevronLeft, ChevronRight } from 'lucide-react';
import { DotPagination } from './DotPagination';

export function CarouselWithDots({ items }) {
  const [currentIndex, setCurrentIndex] = useState(0);
  const scrollRef = useRef(null);

  const scrollTo = (index) => {
    if (scrollRef.current) {
      const container = scrollRef.current;
      const item = container.children[index];

      container.scrollTo({
        left:
          item.offsetLeft - container.offsetWidth / 2 + item.offsetWidth / 2,
        behavior: "smooth",
      });
    }
  };

  const handleNext = () => {
    const nextIndex = (currentIndex + 1) % items.length;
    setCurrentIndex(nextIndex);
    scrollTo(nextIndex);
  };

  const handlePrev = () => {
    const prevIndex = (currentIndex - 1 + items.length) % items.length;
    setCurrentIndex(prevIndex);
    scrollTo(prevIndex);
  };

  return (
    <div className="relative">
      <div className="flex gap-1 mb-4">
        <button
          onClick={handlePrev}
          className="p-1 text-gray-400 hover:text-white transition-colors"
        >
          <ChevronLeft className="h-5 w-5" />
        </button>
        <button
          onClick={handleNext}
          className="p-1 text-gray-400 hover:text-white transition-colors"
        >
          <ChevronRight className="h-5 w-5" />
        </button>
      </div>

      <div
        ref={scrollRef}
        className="flex gap-4 overflow-x-hidden scroll-smooth"
      >
        {items.map((item, index) => (
          <div
            key={item.id}
            className="flex-none w-[200px] transition-transform duration-300"
          >
            <div className="bg-[#2a2a2a] rounded-lg overflow-hidden">
              <div className="aspect-[3/4] relative">
                <img
                  src={`${process.env.NEXT_PUBLIC_API_URL}${item.coverUrl}`}
                  alt={item.title}
                  className="w-full h-full object-cover"
                />
                <div className="absolute inset-0 bg-black/60 opacity-0 hover:opacity-100 transition-opacity p-4">
                  <p className="text-white text-sm line-clamp-6">
                    {item.description}
                  </p>
                </div>
              </div>
              <div className="p-3">
                <h3 className="text-white font-medium truncate">
                  {item.title}
                </h3>
                <div className="text-gray-400 text-sm mt-1">{item.author}</div>
              </div>
            </div>
          </div>
        ))}
      </div>

      <DotPagination
        total={items.length}
        current={currentIndex}
        onClick={(index) => {
          setCurrentIndex(index);
          scrollTo(index);
        }}
      />
    </div>
  );
}
