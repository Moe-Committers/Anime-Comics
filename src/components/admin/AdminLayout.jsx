import { useState } from "react";
import { AdminSidebar } from "./AdminSidebar";
import { AdminNavbar } from "./AdminNavbar";

export function AdminLayout({ children }) {
  const [collapsed, setCollapsed] = useState(false);

  return (
    <div className="flex min-h-screen bg-gray-900">
      <AdminSidebar collapsed={collapsed} setCollapsed={setCollapsed} />

      <div
        className={`flex-1 flex flex-col transition-all duration-300`}
      >
        <AdminNavbar />
        <main className="flex-1">{children}</main>
      </div>
    </div>
  );
}
