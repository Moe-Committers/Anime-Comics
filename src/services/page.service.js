import { axiosInstance } from "@/lib/axios";

export const pageService = {
  async getChapterPages(chapterId, params) {
    try {
      const { data } = await axiosInstance.get(`/pages/chapter/${chapterId}`, {
        params,
      });
      return data;
    } catch (error) {
      console.error("Get pages error:", error.response?.data || error);
      throw error;
    }
  },

  async getPage(id) {
    try {
      const { data } = await axiosInstance.get(`/pages/${id}`);
      return data;
    } catch (error) {
      console.error("Get page error:", error.response?.data || error);
      throw error;
    }
  },

  async addPages(chapterId, files) {
    try {
      const formData = new FormData();
      files.forEach((file) => {
        formData.append("images", file);
      });

      const { data } = await axiosInstance.post(
        `/pages/chapter/${chapterId}`,
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );
      return data;
    } catch (error) {
      console.error("Add pages error:", error.response?.data || error);
      throw error;
    }
  },

  async updatePage(chapterId, pageId, formData) {
    try {
      const { data } = await axiosInstance.put(
        `/pages/chapter/${chapterId}/page/${pageId}`,
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );
      return data;
    } catch (error) {
      console.error("Update page error:", error.response?.data || error);
      throw error;
    }
  },

  async deletePages(chapterId, pageIds) {
    try {
      const { data } = await axiosInstance.delete(
        `/pages/chapter/${chapterId}`,
        {
          headers: {
            "Content-Type": "application/json",
          },
          data: pageIds,
        }
      );
      return data;
    } catch (error) {
      console.error("Delete pages error:", error.response?.data || error);
      throw error;
    }
  },

  async reorderPages(chapterId, newOrder) {
    try {
      const { data } = await axiosInstance.put(
        `/pages/chapter/${chapterId}/reorder`,
        newOrder.newOrder
      );
      return data;
    } catch (error) {
      console.error("Reorder pages error:", error.response?.data || error);
      throw error;
    }
  },
};
