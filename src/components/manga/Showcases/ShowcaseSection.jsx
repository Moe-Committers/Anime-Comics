import { useShowcase } from "@/hooks/useShowcase";
import { CarouselWithDots } from "./CarouselDots";
import { ShowcaseSkeleton } from "./ShowcaseSkeleton";
// import { ErrorDisplay } from '../common/ErrorDisplay';

export function ShowcaseSection() {
  const { data, error, loading } = useShowcase();

  if (loading) {
    return <ShowcaseSkeleton />;
  }

  //   if (error) {
  //     return <ErrorDisplay message="Failed to load showcase" />;
  //   }

  if (!data?.data?.sections?.length) {
    console.log(data)
    return null;
  }

  return (
    <div className="space-y-12">
      {data.data.sections.map((section) => (
        <div key={section.id}>
          <div className="flex justify-between items-center mb-6">
            <h2 className="text-xl font-bold text-white">{section.title}</h2>
            <a
              href={`/manga/browse?category=${section.categoryId || ""}`}
              className="text-gray-400 hover:text-white transition-colors"
            >
              View All
            </a>
          </div>

          <CarouselWithDots
            items={section.books.map((book) => ({
              id: book.id,
              title: book.title,
              coverUrl: book.imageUrl,
              description: book.description,
              author: book.author,
            }))}
          />
        </div>
      ))}
    </div>
  );
}
