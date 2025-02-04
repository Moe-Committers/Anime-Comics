import { axiosInstance } from "@/lib/axios";

export const volumeService = {
  async getBookVolumes(bookId, params) {
    try {
      const { data } = await axiosInstance.get(`/volumes/book/${bookId}`, {
        params,
      });
      return data;
    } catch (error) {
      console.error("Get volumes error:", error.response?.data || error);
      throw error;
    }
  },

  async getVolume(id) {
    try {
      const { data } = await axiosInstance.get(`/volumes/${id}`);
      return data;
    } catch (error) {
      console.error("Get volume error:", error.response?.data || error);
      throw error;
    }
  },

  async createVolume(bookId, formData) {
    try {

      const { data } = await axiosInstance.post(
        `/volumes/book/${bookId}`,
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );
      return data;
    } catch (error) {
      console.error("Create volume error:", error.response?.data || error);
      throw error;
    }
  },

  async updateVolume(bookId, volumeId, formData) {
    try {

      const { data } = await axiosInstance.put(
        `/volumes/book/${bookId}/volume/${volumeId}`,
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );
      return data;
    } catch (error) {
      console.error("Update volume error:", error.response?.data || error);
      throw error;
    }
  },

  async deleteVolume(bookId, volumeId) {
    try {
      const { data } = await axiosInstance.delete(
        `/volumes/book/${bookId}/volume/${volumeId}`
      );
      return data;
    } catch (error) {
      console.error("Delete volume error:", error.response?.data || error);
      throw error;
    }
  },
};
