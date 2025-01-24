const { default: Link } = require("next/link");

export function UpdateItem({ book , timeSince }) {
    return (
      <Link
        href={`/manga/${book.id}`} 
        className="flex gap-3 p-3 hover:bg-[#363636] transition-colors"
      >

        <div className="flex-shrink-0">
          <img 
            src={`${process.env.NEXT_PUBLIC_BACKEND_URL}${book.imageUrl}`} 
            alt={book.title}
            className="w-[60px] h-[80px] object-cover rounded"
          />
        </div>
  
        <div className="flex flex-col min-w-0 flex-1">
          <h3 className="text-sm font-medium text-white hover:text-blue-400 truncate">
            {book.title}
          </h3>
          <div className="flex items-center gap-1 mt-0.5">
            {book.flag && (
              <img 
                src={`/flags/${book.flag}.svg`}
                alt={book.flag}
                className="w-4 h-4"
              />
            )}
            <p className="text-xs text-gray-400 truncate">
              {book.author}
            </p>
          </div>
          <div className="mt-auto flex items-center gap-1.5 text-[11px] text-gray-500">
            <span className="truncate">{book.userName}</span>
            <span>•</span>
            <span className="whitespace-nowrap">{timeSince(new Date(book.published_at))}</span>
          </div>
        </div>
      </Link>
    );
  }