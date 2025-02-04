import { axiosInstance } from "@/lib/axios";

export const dashboardService = {
  async getDashboardStats() {
    try {
      const { data } = await axiosInstance.get("/dashboard");
      return data;
    } catch (error) {
      console.error(
        "Get dashboard stats error:",
        error.response?.data || error
      );
      throw error;
    }
  },
};
