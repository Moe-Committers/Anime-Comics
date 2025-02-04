import { axiosInstance } from "@/lib/axios";

export const chapterService = {
  async getVolumeChapters(volumeId, params) {
    try {
      const { data } = await axiosInstance.get(`/chapters/volume/${volumeId}`, {
        params,
      });
      return data;
    } catch (error) {
      console.error("Get chapters error:", error.response?.data || error);
      throw error;
    }
  },

  async getChapter(id) {
    try {
      const { data } = await axiosInstance.get(`/chapters/${id}`);
      return data;
    } catch (error) {
      console.error("Get chapter error:", error.response?.data || error);
      throw error;
    }
  },

  async createChapter(volumeId, formData) {
    try {
      const { data } = await axiosInstance.post(
        `/chapters/volume/${volumeId}`,
        formData
      );
      return data;
    } catch (error) {
      console.error("Create chapter error:", error.response?.data || error);
      throw error;
    }
  },

  async updateChapter(volumeId, chapterId, formData) {
    try {
      const { data } = await axiosInstance.put(
        `/chapters/volume/${volumeId}/chapter/${chapterId}`,
        formData
      );
      return data;
    } catch (error) {
      console.error("Update chapter error:", error.response?.data || error);
      throw error;
    }
  },

  async deleteChapter(volumeId, chapterId) {
    try {
      const { data } = await axiosInstance.delete(
        `/chapters/volume/${volumeId}/chapter/${chapterId}`
      );
      return data;
    } catch (error) {
      console.error("Delete chapter error:", error.response?.data || error);
      throw error;
    }
  },
};
