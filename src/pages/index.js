import { Navbar } from "@/components/layout/Navbar";
import { FeaturedCarousel } from "@/components/manga/Features/FeaturedCarousel";
import { LatestUpdates } from "@/components/manga/LatestUpdates/LatestUpdates";
import { ShowcaseSection } from "@/components/manga/Showcases/ShowcaseSection";

export default function Home() {

  // 1. Dark theme (current MangaDex style):
  // - Background: #1f1f1f
  // - Primary: #ec4899 (pink)
  // - Secondary: #2c2c2c
  // - Text: #ffffff, #9ca3af (gray)

  // 2. Light pink theme:
  // - Background: #ffffff
  // - Primary: #ff4d8d
  // - Secondary: #fff0f5
  // - Text: #1a1a1a, #4a4a4a

  return (
    <div className="min-h-screen bg-[#1f1f1f]">
      <Navbar />

      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-6">
        <FeaturedCarousel />

        <LatestUpdates />

        <ShowcaseSection />
      </main>
    </div>
  );
}
