import { MangaDetailPage } from "@/components/manga/Manga/MangaDetailPage";
import React from "react";

const test = () => {
  const mockdata = {
    title: "The Phantom Chronicles",
    coverImage: "/test2.jpg",
    rating: 4.7,
    status: "Ongoing",
    totalVolumes: 12,
    description:
      "In a world where reality bends and shadows whisper, a young hero embarks on a journey to uncover the truth behind the mysterious disappearances of those close to him.",
    genres: ["Action", "Fantasy", "Adventure", "Mystery"],
    volumes: [
      {
        id: 1,
        number: 1,
        coverImage: "https://example.com/images/volume1.jpg",
        chapters: [
          { id: 1, number: 1, title: "The Awakening", pagesCount: 22 },
          { id: 2, number: 2, title: "Shadows in the Night", pagesCount: 30 },
        ],
      },
      {
        id: 2,
        number: 2,
        coverImage: "https://example.com/images/volume2.jpg",
        chapters: [
          { id: 3, number: 1, title: "Into the Darkness", pagesCount: 28 },
          { id: 4, number: 2, title: "The Hidden Path", pagesCount: 25 },
        ],
      },
      {
        id: 3,
        number: 3,
        coverImage: "https://example.com/images/volume3.jpg",
        chapters: [
          { id: 5, number: 1, title: "Unseen Forces", pagesCount: 33 },
          { id: 6, number: 2, title: "The Betrayal", pagesCount: 27 },
        ],
      },
    ],
  };

  return (
    <div>
      <MangaDetailPage manga={mockdata} />
    </div>
  );
};

export default test;
