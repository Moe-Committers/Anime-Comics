import { axiosInstance } from "@/lib/axios";

export const authService = {
  async login(credentials) {
    const { data } = await axiosInstance.post("/auth/login", credentials);
    return data;
  },

  async register(credentials) {
    const { data } = await axiosInstance.post("/auth/register", credentials);
    return data;
  },

  async logout() {
    const { data } = await axiosInstance.post("/auth/logout");
    return data;
  },

  async me() {
    const { data } = await axiosInstance.get("/auth/me");
    return data;
  },
};
