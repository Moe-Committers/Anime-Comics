import Link from "next/link";
import { UpdatesCard } from "./UpdatesCard";
import { UpdatesCardSkeleton } from "./UpdatesCardSkeleton";
import { useLatestUpdates } from "@/hooks/useLatestUpdates";
import { timeSince } from "@/lib/timeSince";

export function LatestUpdates() {
  const { data, loading, error } = useLatestUpdates();

  if (error) {
    return null;
  }

  const columns = loading
    ? [1, 2, 3]
    : [data.slice(0, 6), data.slice(6, 12), data.slice(12, 18)];

  return (
    <section className="py-6">
      <div className="flex justify-between items-center mb-6">
        <h2 className="text-xl font-bold text-white">Latest Updates</h2>
        <Link
          href="/titles/latest"
          className="text-gray-400 hover:text-white transition-colors text-sm"
        >
          View All
        </Link>
      </div>

      <div className="hidden md:grid md:grid-cols-3 gap-4">
        {loading
          ? columns.map((_, index) => <UpdatesCardSkeleton key={index} />)
          : columns.map((books, index) => (
              <UpdatesCard key={index} books={books} timeSince={timeSince} />
            ))}
      </div>

      <div className="md:hidden">
        {loading ? (
          <UpdatesCardSkeleton />
        ) : (
          <UpdatesCard books={data.slice(0, 6)} timeSince={timeSince} />
        )}
      </div>
    </section>
  );
}
