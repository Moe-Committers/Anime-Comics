using anime_comics.DB;
using anime_comics.Models;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Utils.Helpers.Extensions;

public static class ChapterExtensions
{
    public static void UpdatePageCount(this Chapters chapter)
    {
        if (chapter.Pages == null)
        {
            chapter.PageCount = 0;
            return;
        }

        chapter.PageCount = chapter.Pages.Count;
    }

    public static async Task UpdatePageCountAsync(this Chapters chapter, database db)
    {
        if (chapter.Pages == null)
        {
            var count = await db.pages
                .Where(p => p.ChapterId == chapter.Id)
                .CountAsync();

            chapter.PageCount = count;
        }
        else
        {
            chapter.PageCount = chapter.Pages.Count;
        }
    }
}