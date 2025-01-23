import { axiosInstance } from "@/lib/axios";


export const authService = {
  async login(credentials) {
    const { data } = await axiosInstance.post('/auth/login', credentials);
    return data;
  },

  async register(credentials) {
    const { data } = await axiosInstance.post('/auth/register', credentials);
    return data;
  },

  async logout() {
    await axiosInstance.post('/auth/logout');
    localStorage.removeItem('accessToken');
  },

  async me() {
    const { data } = await axiosInstance.get('/auth/me');
    return data;
  }
};