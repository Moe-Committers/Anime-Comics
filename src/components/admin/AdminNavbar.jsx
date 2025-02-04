import { Bell, Search } from "lucide-react";
import { useRouter } from "next/router";

export function AdminNavbar() {
  const router = useRouter();

  const generateBreadcrumb = () => {
    const paths = router.pathname.split("/").filter(Boolean);
    const breadcrumbs = paths.map((path, index) => {
      const href = `/${paths.slice(0, index + 1).join("/")}`;
      return {
        title: path.charAt(0).toUpperCase() + path.slice(1),
        href,
      };
    });
    return breadcrumbs;
  };

  return (
    <div className="h-16 bg-gray-900 border-b border-gray-800 flex items-center justify-between px-6">
      <div className="flex items-center gap-4">
        <div className="relative">
          <input
            type="text"
            placeholder="Search..."
            className="w-64 h-9 px-4 py-1 pl-10 rounded-lg bg-gray-800 text-gray-300 placeholder-gray-500 border border-gray-700 focus:outline-none focus:border-orange-500"
          />
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-500" />
        </div>

        <div className="flex items-center gap-2 text-sm text-gray-400">
          {generateBreadcrumb().map((item, index, array) => (
            <div key={item.href} className="flex items-center">
              <a
                href={item.href}
                className="hover:text-white transition-colors"
              >
                {item.title}
              </a>
              {index < array.length - 1 && (
                <span className="mx-2 text-gray-600">/</span>
              )}
            </div>
          ))}
        </div>
      </div>

      <div className="flex items-center gap-4">
        <button className="relative p-2 text-gray-400 hover:text-white transition-colors">
          <Bell className="h-5 w-5" />
          <span className="absolute top-1 right-1 w-2 h-2 bg-orange-500 rounded-full"></span>
        </button>
      </div>
    </div>
  );
}
