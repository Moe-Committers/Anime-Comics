import { axiosInstance } from "@/lib/axios";

export const bookService = {
  async getShowcase() {
    try {
      const { data } = await axiosInstance.get("/Books/showcase");
      return data;
    } catch (error) {
      throw error;
    }
  },
  async getBooks(params) {
    const { data } = await axiosInstance.get("/books", { params });
    return data;
  },

  async getBook(id) {
    const { data } = await axiosInstance.get(`/books/${id}`);
    return data;
  },

  async createBook(formData) {
    const { data } = await axiosInstance.post("/books", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
    return data;
  },

  async updateBook(id, formData) {
    const { data } = await axiosInstance.put(`/books/${id}`, formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
    return data;
  },

  async deleteBook(id) {
    const { data } = await axiosInstance.delete(`/books/${id}`);
    return data;
  },

  async publishBook(id, publish = true) {
    const { data } = await axiosInstance.put(`/books/publish/${id}`, {
      publish,
    });
    return data;
  },
};
