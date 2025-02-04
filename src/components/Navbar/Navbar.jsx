import { useState, useEffect, useRef } from 'react';
import Link from 'next/link';
import { useAuthStore } from '@/store/auth.store';
import { BookOpen, Search, Settings, Moon, ChevronDown, MenuIcon } from 'lucide-react';
import { useClickOutside } from '@/hooks/useClickOutside';

export function Navbar() {
  const { user, logout } = useAuthStore();
  const [isScrolled, setIsScrolled] = useState(false);
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const dropdownRef = useRef();

  useClickOutside(dropdownRef, () => setIsDropdownOpen(false));

  useEffect(() => {
    const handleScroll = () => {
      setIsScrolled(window.scrollY > 20);
    };
    window.addEventListener('scroll', handleScroll);
    return () => window.removeEventListener('scroll', handleScroll);
  }, []);

  return (
    <nav className={`fixed top-0 left-0 right-0 z-50 transition-all duration-300 
      ${isScrolled ? 'bg-[#1f1f1f] shadow-lg' : 'bg-gradient-to-b from-black/80 to-transparent'}`}>
    
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-16">

          <div className="flex items-center gap-5">
          <Link href="/" className="flex items-center gap-2">
              <MenuIcon className="h-8 w-8 text-white"/>
            </Link>
            <Link href="/" className="flex items-center gap-2">
              <BookOpen className="h-8 w-8 text-orange-500" />
              <span className="text-xl font-bold text-white">MangaVerse</span>
            </Link>
          </div>

          <div className="flex items-center gap-4">

            <div className="relative">
              <input
                type="text"
                placeholder="Search manga..."
                className="w-64 h-8 px-4 py-1 rounded bg-[#374151] text-white placeholder-gray-400 
                focus:outline-none focus:ring-1 focus:ring-orange-500 text-sm"
              />
              <Search className="absolute right-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400" />
            </div>
            {user ? (
              <div className="relative" ref={dropdownRef}>
                <button
                  onClick={() => setIsDropdownOpen(!isDropdownOpen)}
                  className="flex items-center gap-2 p-2 rounded-md hover:bg-gray-700 transition-colors"
                >
                  <div className="w-8 h-8 rounded-full bg-gray-700 flex items-center justify-center text-white">
                    Guest
                  </div>
                  <ChevronDown className="h-4 w-4 text-gray-400" />
                </button>

                {isDropdownOpen && (
                  <div className="absolute right-0 mt-2 w-56 bg-[#2A2A2A] rounded-md shadow-lg overflow-hidden">

                    <div className="px-4 py-3 border-b border-gray-700">
                      <p className="text-white text-sm font-medium">Guest</p>
                    </div>

                    <div className="p-2 border-b border-gray-700">
                      <button className="w-full px-3 py-2 text-sm text-gray-300 hover:bg-gray-700 rounded flex items-center gap-2">
                        <Settings className="h-4 w-4" />
                        Settings
                      </button>
                      <button className="w-full px-3 py-2 text-sm text-gray-300 hover:bg-gray-700 rounded flex items-center gap-2">
                        <Moon className="h-4 w-4" />
                        Theme
                      </button>
                    </div>

                    <div className="p-2 border-b border-gray-700">
                      <div className="px-3 py-2">
                        <p className="text-sm text-gray-300 mb-2">Interface Language</p>
                        <select className="w-full bg-gray-700 text-white text-sm rounded px-2 py-1">
                          <option>English</option>
                        </select>
                      </div>
                      <div className="px-3 py-2">
                        <p className="text-sm text-gray-300 mb-2">Chapter Languages</p>
                        <select className="w-full bg-gray-700 text-white text-sm rounded px-2 py-1">
                          <option>All</option>
                        </select>
                      </div>
                    </div>

                    <div className="p-2">
                      <button
                        onClick={logout}
                        className="w-full px-3 py-2 text-center text-white bg-orange-500 hover:bg-orange-600 rounded text-sm font-medium"
                      >
                        Sign Out
                      </button>
                    </div>
                  </div>
                )}
              </div>
            ) : (
              <Link
                href="/login"
                className="px-4 py-2 bg-orange-500 hover:bg-orange-600 text-white rounded-md text-sm font-medium transition-colors"
              >
                Sign In
              </Link>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
}