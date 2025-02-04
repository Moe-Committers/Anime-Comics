import { axiosInstance } from "@/lib/axios";

export const categoryService = {
  async getCategories(params) {
    const { data } = await axiosInstance.get("/categories", { params });
    return data;
  },

  async getCategory(id) {
    const { data } = await axiosInstance.get(`/categories/${id}`);
    return data;
  },

  async createCategory(formData) {
    const { data } = await axiosInstance.post("/categories", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
    return data;
  },

  async updateCategory(id, formData) {
    const { data } = await axiosInstance.put(`/categories/${id}`, formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
    return data;
  },

  async deleteCategory(id) {
    const { data } = await axiosInstance.delete(`/categories/${id}`);
    return data;
  },
};
