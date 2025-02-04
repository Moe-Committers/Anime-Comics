import { useState } from "react";
import Link from "next/link";
import { useRouter } from "next/router";
import { useAuthStore } from "@/store/auth.store";
import {
  BookOpen,
  LayoutDashboard,
  Users,
  Tags,
  ChevronDown,
  Settings,
  LogOut,
  ChevronsLeft,
  ChevronsRight,
} from "lucide-react";

export function AdminSidebar({ collapsed, setCollapsed }) {
  const router = useRouter();
  const { user, logout } = useAuthStore();
  const [openMenus, setOpenMenus] = useState({});

  const menuItems = [
    {
      title: "Dashboard",
      icon: LayoutDashboard,
      href: "/admin",
    },
    {
      title: "Content Management",
      icon: BookOpen,
      submenu: [
        {
          title: "Books",
          href: "/admin/books",
        },
        {
          title: "Categories",
          href: "/admin/categories",
        },
      ],
    },
    {
      title: "User Management",
      icon: Users,
      href: "/admin/users",
    },
    {
      title: "Settings",
      icon: Settings,
      href: "/admin/settings",
    },
  ];

  const isActive = (path) => router.pathname === path;
  const isSubmenuActive = (submenu) =>
    submenu.some((item) => router.pathname.startsWith(item.href));

  const toggleMenu = (title) => {
    setOpenMenus((prev) => ({
      ...prev,
      [title]: !prev[title],
    }));
  };

  const handleLogout = async () => {
    try {
      await logout();
      router.push("/login");
    } catch (error) {
      console.error("Logout failed:", error);
    }
  };

  return (
    <div
      className={`min-h-screen bg-[#1A1C23] flex flex-col transition-all duration-300 relative ${
        collapsed ? "w-16" : "w-60"
      }`}
    >

      <div className="h-16 flex items-center justify-center border-b border-gray-800">
        <BookOpen className="h-6 w-6 text-orange-500 flex-shrink-0" />
        {!collapsed && (
          <span className="ml-3 text-lg font-bold text-white">MangaVerse</span>
        )}
      </div>

      <button
        onClick={() => setCollapsed(!collapsed)}
        className="absolute -right-3 top-4 bg-[#1A1C23] border border-gray-800 rounded-full p-1.5 hover:bg-gray-800"
      >
        {collapsed ? (
          <ChevronsRight className="h-4 w-4 text-gray-400" />
        ) : (
          <ChevronsLeft className="h-4 w-4 text-gray-400" />
        )}
      </button>

      <div className="flex-1 py-4 overflow-y-auto">
        <nav className="px-2 space-y-1">
          {menuItems.map((item) => (
            <div key={item.title}>
              {item.submenu ? (
                <>
                  <button
                    onClick={() => toggleMenu(item.title)}
                    className={`w-full flex items-center text-sm rounded-lg transition-colors
                      ${collapsed ? "justify-center p-3" : "px-3 py-2"} 
                      ${
                        isSubmenuActive(item.submenu)
                          ? "bg-gray-800 text-white"
                          : "text-gray-400 hover:bg-gray-800 hover:text-white"
                      }`}
                  >
                    <div className="flex items-center justify-center w-6">
                      <item.icon className="h-5 w-5" />
                    </div>
                    {!collapsed && (
                      <>
                        <span className="ml-3 flex-1">{item.title}</span>
                        <ChevronDown
                          className={`h-4 w-4 transition-transform ${
                            openMenus[item.title] ? "rotate-180" : ""
                          }`}
                        />
                      </>
                    )}
                  </button>
                  {openMenus[item.title] && !collapsed && (
                    <div className="mt-1 ml-9 space-y-1">
                      {item.submenu.map((subItem) => (
                        <Link
                          key={subItem.href}
                          href={subItem.href}
                          className={`block px-3 py-2 text-sm rounded-lg transition-colors ${
                            isActive(subItem.href)
                              ? "bg-gray-800 text-white"
                              : "text-gray-400 hover:bg-gray-800 hover:text-white"
                          }`}
                        >
                          {subItem.title}
                        </Link>
                      ))}
                    </div>
                  )}
                </>
              ) : (
                <Link
                  href={item.href}
                  className={`flex items-center text-sm rounded-lg transition-colors
                    ${collapsed ? "justify-center p-3" : "px-3 py-2"} 
                    ${
                      isActive(item.href)
                        ? "bg-gray-800 text-white"
                        : "text-gray-400 hover:bg-gray-800 hover:text-white"
                    }`}
                >
                  <div className="flex items-center justify-center w-6">
                    <item.icon className="h-5 w-5" />
                  </div>
                  {!collapsed && <span className="ml-3">{item.title}</span>}
                </Link>
              )}
            </div>
          ))}
        </nav>
      </div>

      <div className="p-4 border-t border-gray-800">
        <div className="flex items-center gap-3 mb-3">
          <div className="w-8 h-8 rounded-full bg-gray-800 flex items-center justify-center flex-shrink-0">
            {user?.data.profile ? (
              <img
                src={`${process.env.NEXT_PUBLIC_API_URL}${user.data.profile}`}
                alt={user?.data.name}
                className="w-full h-full rounded-full object-cover"
              />
            ) : (
              <span className="text-sm font-medium text-white">
                {user?.data.name?.[0]?.toUpperCase()}
              </span>
            )}
          </div>
          {!collapsed && (
            <div className="flex-1 min-w-0">
              <p className="text-sm font-medium text-white truncate">
                {user?.data.name}
              </p>
              <p className="text-xs text-gray-400 truncate">{user?.data.email}</p>
            </div>
          )}
        </div>
        <button
          onClick={handleLogout}
          className={`w-full flex items-center text-sm text-red-400 hover:bg-gray-800 rounded-lg transition-colors
            ${collapsed ? "justify-center p-3" : "px-3 py-2"}`}
        >
          <div className="flex items-center justify-center w-6">
            <LogOut className="h-5 w-5" />
          </div>
          {!collapsed && <span className="ml-3">Logout</span>}
        </button>
      </div>
    </div>
  );
}
