import { timeSince } from '@/lib/timeSince';
import Link from 'next/link';

export function MangaListItem({ manga }) {
  return (
    <Link
      href={`/manga/${manga.id}`}
      className="flex bg-[#2a2a2a] hover:bg-[#363636] transition-colors rounded-lg overflow-hidden"
    >
      <div className="flex p-4 w-full gap-4">

        <div className="flex-shrink-0">
          <img
            src={`${process.env.NEXT_PUBLIC_API_URL}${manga.imageUrl}`}
            alt={manga.title}
            className="w-[72px] h-[96px] object-cover rounded"
          />
        </div>

        <div className="flex flex-col flex-1 min-w-0">
          <div className="flex items-start justify-between gap-4">
            <div className="flex-1 min-w-0">
              <h3 className="text-white text-lg font-medium truncate">
                {manga.title}
              </h3>
              <div className="flex items-center gap-2 mt-1">
                <span className="text-gray-400 text-sm">
                  {manga.author}
                </span>
              </div>
            </div>

            <div className="flex flex-col items-end text-sm text-gray-400 whitespace-nowrap">
              <span>{timeSince(new Date(manga.published_at))}</span>
              {manga.fav > 0 && (
                <span className="text-gray-500 mt-1">
                  {manga.fav.toLocaleString()} favs
                </span>
              )}
            </div>
          </div>

          <div className="mt-auto flex items-center gap-2 text-sm">
            <span className="text-gray-500 truncate">
              {manga.userName}
            </span>
          </div>
        </div>
      </div>
    </Link>
  );
}