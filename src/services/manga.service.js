import { axiosInstance } from "@/lib/axios";

export const mangaService = {
  async getPopularBooks() {
    const response = await axiosInstance.get("/Books/Popular-updates");
    if (!response.data.success) {
      throw new Error(response.data.message || "Failed to fetch popular books");
    }
    return response.data;
  },
  async getBooks(params) {
    const { data } = await axiosInstance.get("/Books/published", { params });
    return data;
  },
  async getPublishedBooks(params) {
    const { data } = await axiosInstance.get('/books/published', { params });
    return data;
  }
};
