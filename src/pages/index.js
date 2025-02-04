import withAuth from "@/components/layout/WithAuth";
import { FeaturedCarousel } from "@/components/manga/Features/FeaturedCarousel";
import { LatestUpdates } from "@/components/manga/LatestUpdates/LatestUpdates";
import { ShowcaseSection } from "@/components/manga/Showcases/ShowcaseSection";

function Home() {
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
    <>
      <FeaturedCarousel />

      <LatestUpdates />

      <ShowcaseSection />
    </>
  );
}

export default withAuth(Home , {requiresAuth:false});
