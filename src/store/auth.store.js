import { authService } from "@/services/auth.service";
import { create } from "zustand";

export const useAuthStore = create((set) => ({
  user: null,
  isAuthenticated: false,
  isLoading: true,
  error: null,

  login: async (credentials) => {
    try {
      set({ isLoading: true, error: null });
      const response = await authService.login(credentials);

      if (!response.success) {
        throw new Error(response.message || "Login failed");
      }

      localStorage.setItem("accessToken", response.data.accessToken);
      set({
        user: response.data.user,
        isAuthenticated: true,
        isLoading: false,
      });
    } catch (error) {
      set({
        error:
          error?.response?.data?.message || error.message || "Login failed",
        isLoading: false,
        isAuthenticated: false,
        user: null,
      });
      throw error;
    }
  },

  register: async (credentials) => {
    try {
      set({ isLoading: true, error: null });
      const response = await authService.register(credentials);

      if (!response.success) {
        throw new Error(response.message || "Registration failed");
      }

      localStorage.setItem("accessToken", response.data.accessToken);
      set({
        user: response.data.user,
        isAuthenticated: true,
        isLoading: false,
      });
    } catch (error) {
      set({
        error:
          error?.response?.data?.message ||
          error.message ||
          "Registration failed",
        isLoading: false,
        isAuthenticated: false,
        user: null,
      });
      throw error;
    }
  },

  logout: async () => {
    try {
      await authService.logout();
      localStorage.removeItem("accessToken");
      set({
        user: null,
        isAuthenticated: false,
        error: null,
      });
    } catch (error) {
      set({
        error:
          error?.response?.data?.message || error.message || "Logout failed",
      });
      throw error;
    }
  },

  checkAuth: async () => {
    try {
      set({ isLoading: true });
      const token = localStorage.getItem("accessToken");

      if (!token) {
        set({
          user: null,
          isAuthenticated: false,
          isLoading: false,
        });
        return;
      }

      const response = await authService.me();
      if (response.success) {
        set({
          user: response.data,
          isAuthenticated: true,
          isLoading: false,
          error: null,
        });
      } else {
        set({
          user: null,
          isAuthenticated: false,
          isLoading: false,
          error: "Authentication failed",
        });
      }
    } catch (error) {
      set({
        user: null,
        isAuthenticated: false,
        isLoading: false,
        error: error?.response?.data?.message || "Authentication check failed",
      });
    }
  },
}));

if (typeof window !== 'undefined') {
  useAuthStore.getState().checkAuth();
}