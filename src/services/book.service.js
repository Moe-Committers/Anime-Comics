import { axiosInstance } from '@/lib/axios';

export const bookService = {
  async getShowcase() {
    try {
      const { data } = await axiosInstance.get('/Books/showcase');
      return data;
    } catch (error) {
      throw error;
    }
  },
};