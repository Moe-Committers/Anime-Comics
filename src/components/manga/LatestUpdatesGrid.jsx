import { Clock } from "lucide-react";

export function LatestUpdatesGrid() {
  const updates = [
    {
      id: 1,
      title: "My New Girlfriend Is Not Human?",
      chapter: "Vol. 1 Ch. 41.5 - História paralela",
      group: "@oNatsukeYuu(x)",
      timeAgo: "2 minutes ago",
      coverUrl: "/api/placeholder/120/180",
      flag: "br",
    },
    {
      id: 2,
      title: "My New Girlfriend Is Not Human?",
      chapter: "Vol. 1 Ch. 41.5 - História paralela",
      group: "@oNatsukeYuu(x)",
      timeAgo: "2 minutes ago",
      coverUrl: "/api/placeholder/120/180",
      flag: "br", 
    },
    {
      id: 3,
      title: "My New Girlfriend Is Not Human?",
      chapter: "Vol. 1 Ch. 41.5 - História paralela",
      group: "@oNatsukeYuu(x)",
      timeAgo: "2 minutes ago",
      coverUrl: "/api/placeholder/120/180",
      flag: "br", 
    },
  ];

  return (
    <section className="mb-8 mt-3">
      <div className="flex justify-between items-center mb-6">
        <div className="flex items-center gap-2">
          <Clock className="h-6 w-6 text-gray-400" />
          <h2 className="text-xl font-bold text-white">Latest Updates</h2>
        </div>
        <a href="/latest" className="text-gray-400 hover:text-white">
          View All
        </a>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {updates.map((item) => (
          <div
            key={item.id}
            className="flex gap-3 bg-[#242424] p-3 rounded hover:bg-[#2c2c2c] transition-colors"
          >
            <img
              src={item.coverUrl}
              alt={item.title}
              className="w-16 h-24 object-cover rounded"
            />
            <div className="flex-1 min-w-0">
              <h3 className="font-medium text-white truncate">{item.title}</h3>
              <p className="text-sm text-gray-400 mb-1">{item.chapter}</p>
              <div className="flex items-center gap-2 text-xs text-gray-500">
                <img
                  src={`/flags/${item.flag}.svg`}
                  alt={item.flag}
                  className="w-4 h-4"
                />
                <span>{item.group}</span>
                <span>•</span>
                <span>{item.timeAgo}</span>
              </div>
            </div>
          </div>
        ))}
      </div>
    </section>
  );
}
