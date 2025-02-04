import { axiosInstance } from "@/lib/axios";

export const userService = {
  async updateProfile(formData) {
    try {
      console.log("Updating profile with:", {
        name: formData.get("name"),
        fileImg: formData.get("fileImg"),
      });

      const { data } = await axiosInstance.put(
        "/auth/change-avatar",
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );
      return data;
    } catch (error) {
      console.error("Update profile error:", {
        error: error.response?.data || error,
        status: error.response?.status,
      });
      throw error;
    }
  },

  async changePassword(passwordData) {
    try {
      console.log("Changing password...");
      const { data } = await axiosInstance.put(
        "/auth/change-password",
        passwordData
      );
      return data;
    } catch (error) {
      console.error("Change password error:", {
        error: error.response?.data || error,
        status: error.response?.status,
      });
      throw error;
    }
  },
  async getUsers(params) {
    try {
      const { data } = await axiosInstance.get("/auth", { params });
      return data;
    } catch (error) {
      console.error("Get users error:", error.response?.data || error);
      throw error;
    }
  },

  async toggleUserStatus(userId, toggle) {
    try {
      const { data } = await axiosInstance.put(
        `/UserManagement/toggle/${userId}`,
        {
          toggle,
        }
      );
      return data;
    } catch (error) {
      console.error("Toggle user status error:", error.response?.data || error);
      throw error;
    }
  },

  async adminUpdateUserProfile(userId, formData) {
    try {
      const { data } = await axiosInstance.put(
        `/UserManagement/change-avatar/${userId}`,
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );
      return data;
    } catch (error) {
      console.error(
        "Admin update user profile error:",
        error.response?.data || error
      );
      throw error;
    }
  },

  async adminChangeUserPassword(userId, passwordData) {
    try {
      const { data } = await axiosInstance.put(
        `/UserManagement/change-password/${userId}`,
        {
          newPassword: passwordData.newPassword,
          confirmPassword: passwordData.confirmPassword,
        }
      );
      return data;
    } catch (error) {
      console.error(
        "Admin change user password error:",
        error.response?.data || error
      );
      throw error;
    }
  },
};
