import { authService } from "@/services/auth.service";
import { create } from "zustand";


export const useAuthStore = create((set) => ({
  user: null,
  isAuthenticated: false,
  isLoading: false,
  error: null,

  login: async (credentials) => {
    try {
      set({ isLoading: true, error: null });
      const response = await authService.login(credentials);
      localStorage.setItem('accessToken', response.accessToken);
      set({ 
        user: response.user, 
        isAuthenticated: true,
        isLoading: false
      });
    } catch (error) {
      set({ 
        error: error.response?.data?.message || 'Login failed', 
        isLoading: false 
      });
      throw error;
    }
  },

  register: async (credentials) => {
    try {
      set({ isLoading: true, error: null });
      const response = await authService.register(credentials);
      localStorage.setItem('accessToken', response.accessToken);
      set({ 
        user: response.user, 
        isAuthenticated: true,
        isLoading: false
      });
    } catch (error) {
      set({ 
        error: error.response?.data?.message || 'Registration failed', 
        isLoading: false 
      });
      throw error;
    }
  },

  logout: async () => {
    try {
      await authService.logout();
      set({ user: null, isAuthenticated: false });
    } catch (error) {
      set({ error: error.response?.data?.message || 'Logout failed' });
    }
  },

  checkAuth: async () => {
    try {
      set({ isLoading: true });
      const response = await authService.me();
      set({ 
        user: response.user, 
        isAuthenticated: true,
        isLoading: false
      });
    } catch (error) {
      set({ 
        user: null, 
        isAuthenticated: false,
        isLoading: false
      });
    }
  }
}));