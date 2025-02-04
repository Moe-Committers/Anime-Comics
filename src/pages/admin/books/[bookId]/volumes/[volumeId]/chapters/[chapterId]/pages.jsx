import { useRouter } from "next/router";
import { useEffect, useState } from "react";
import withAdminAuth from "@/components/layout/WithAdminAuth";
import { PageManager } from "@/components/page/PageManager";
import { chapterService } from "@/services/chapter.service";
import Link from "next/link";
import { Book, ChevronRight, Files } from "lucide-react";

function AdminChapterPagesPage() {
  const router = useRouter();
  const { chapterId, volumeId, bookId } = router.query;
  const [chapter, setChapter] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!chapterId) return;

    async function fetchChapter() {
      try {
        const response = await chapterService.getChapter(chapterId);
        if (response.success) {
          setChapter(response.data);
        }
      } catch (error) {
        console.error("Failed to fetch chapter:", error);
      } finally {
        setLoading(false);
      }
    }

    fetchChapter();
  }, [chapterId]);

  if (loading) {
    return (
      <div className="flex justify-center items-center h-screen">
        <div className="w-8 h-8 border-4 border-blue-500 border-t-transparent rounded-full animate-spin" />
      </div>
    );
  }

  if (!chapter) {
    return (
      <div className="max-w-7xl mx-auto py-4 mt-8">
        <div className="text-center text-gray-400">Chapter not found</div>
      </div>
    );
  }

  return (
    <div className="max-w-7xl mx-auto py-4 mt-8">
      <div className="mb-6">
        <div className="flex items-center gap-2 text-sm text-gray-400 mb-2">
          <Link href="/admin/books" className="hover:text-white">
            <Book className="h-4 w-4" />
          </Link>
          <ChevronRight className="h-4 w-4" />
          <Link
            href={`/admin/books/${bookId}/volumes`}
            className="hover:text-white"
          >
            Volumes
          </Link>
          <ChevronRight className="h-4 w-4" />
          <Link
            href={`/admin/books/${bookId}/volumes/${volumeId}/chapters`}
            className="hover:text-white"
          >
            Chapters
          </Link>
          <ChevronRight className="h-4 w-4" />
          <span className="text-white">Pages</span>
        </div>

        <div className="flex justify-between items-center">
          <div>
            <h1 className="text-2xl font-bold text-white">
              Chapter {chapter.chapNo}: {chapter.title}
            </h1>
            <p className="text-gray-400">Manage Pages</p>
          </div>
          <button
            onClick={() => router.back()}
            className="px-4 py-2 text-gray-400 hover:text-white"
          >
            Back to Chapters
          </button>
        </div>
      </div>

      <PageManager chapterId={parseInt(chapterId)} />
    </div>
  );
}

export default withAdminAuth(AdminChapterPagesPage);
