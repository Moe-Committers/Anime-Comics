export function Art() {
  return (
    <div className="grid grid-cols-2 sm:grid-cols-3 gap-4">
      {Array(6)
        .fill(0)
        .map((_, i) => (
          <div
            key={i}
            className="aspect-[3/4] bg-[#2a2a2a] rounded-lg overflow-hidden"
          >
            <img
              src={`/placeholder-art-${i + 1}.jpg`}
              alt="Fan Art"
              className="w-full h-full object-cover hover:scale-105 transition-transform"
            />
          </div>
        ))}
    </div>
  );
}
