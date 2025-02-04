import { useState } from "react";
import { toast, Toaster } from "sonner";
import { Upload, User, KeyRound } from "lucide-react";
import { useAuthStore } from "@/store/auth.store";
import withAuth from "@/components/layout/WithAuth";
import { userService } from "@/services/user.service";

function ProfilePage() {
  const { user, checkAuth } = useAuthStore();
  const [loading, setLoading] = useState(false);
  const [changePassword, setChangePassword] = useState(false);
  const [preview, setPreview] = useState(
    user?.data.profile
      ? `${process.env.NEXT_PUBLIC_API_URL}${user.data.profile}`
      : null
  );

  const [formData, setFormData] = useState({
    name: user?.data.name || "",
    fileImg: null,
    currentPassword: "",
    newPassword: "",
    confirmPassword: "",
  });

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setFormData((prev) => ({ ...prev, fileImg: file }));
      const reader = new FileReader();
      reader.onloadend = () => {
        setPreview(reader.result);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleUpdateProfile = async () => {
    const profileData = new FormData();

    if (formData.name && formData.name !== user.data.name) {
      profileData.append("name", formData.name);
    }
    if (formData.fileImg) {
      profileData.append("fileImg", formData.fileImg);
    }

    const response = await userService.updateProfile(profileData);
    if (!response.success) {
      throw new Error(response.message || "Failed to update profile");
    }
    return response;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      if (formData.name !== user.data.name || formData.fileImg) {
        console.log("Updating profile...");
        await handleUpdateProfile();
      }

      if (changePassword) {
        if (formData.newPassword !== formData.confirmPassword) {
          throw new Error("Passwords do not match");
        }

        console.log("Changing password...");
        await userService.changePassword({
          currentPassword: formData.currentPassword,
          newPassword: formData.newPassword,
          confirmPassword: formData.confirmPassword,
        });
      }

      toast.success("Profile updated successfully");
      await checkAuth();

      if (changePassword) {
        setFormData((prev) => ({
          ...prev,
          currentPassword: "",
          newPassword: "",
          confirmPassword: "",
        }));
        setChangePassword(false);
      }
    } catch (error) {
      console.error("Profile update error:", error);
      toast.error(error.message || "Failed to update profile");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="max-w-2xl mx-auto py-8 px-4">
      <Toaster/>
      <h1 className="text-2xl font-bold text-white mb-8">My Profile</h1>

      <form onSubmit={handleSubmit} className="space-y-6">
        <div className="flex flex-col items-center gap-4">
          <div className="relative w-32 h-32">
            {preview ? (
              <img
                src={preview}
                alt="Profile"
                className="w-full h-full rounded-full object-cover border-4 border-gray-700"
              />
            ) : (
              <div className="w-full h-full rounded-full bg-gray-700 flex items-center justify-center border-4 border-gray-600">
                <User className="h-16 w-16 text-gray-400" />
              </div>
            )}
            <label className="absolute bottom-0 right-0 p-2 bg-gray-700 rounded-full cursor-pointer hover:bg-gray-600 transition-colors">
              <Upload className="h-5 w-5 text-white" />
              <input
                type="file"
                accept="image/*"
                onChange={handleImageChange}
                className="hidden"
              />
            </label>
          </div>
        </div>

        <div className="bg-gray-800 rounded-lg p-6">
          <h2 className="text-lg font-medium text-white mb-4">
            Basic Information
          </h2>
          <div className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-300 mb-1">
                Name
              </label>
              <input
                type="text"
                value={formData.name}
                onChange={(e) =>
                  setFormData((prev) => ({ ...prev, name: e.target.value }))
                }
                className="w-full px-3 py-2 bg-gray-700 text-white rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
              />
            </div>
          </div>
        </div>

        <div className="bg-gray-800 rounded-lg p-6">
          <div className="flex items-center justify-between mb-4">
            <h2 className="text-lg font-medium text-white">Password</h2>
            <button
              type="button"
              onClick={() => setChangePassword(!changePassword)}
              className="flex items-center gap-2 px-4 py-2 text-sm text-gray-300 hover:text-white"
            >
              <KeyRound className="h-4 w-4" />
              {changePassword ? "Cancel" : "Change Password"}
            </button>
          </div>

          {changePassword && (
            <div className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-300 mb-1">
                  Current Password
                </label>
                <input
                  type="password"
                  value={formData.currentPassword}
                  onChange={(e) =>
                    setFormData((prev) => ({
                      ...prev,
                      currentPassword: e.target.value,
                    }))
                  }
                  className="w-full px-3 py-2 bg-gray-700 text-white rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                  required={changePassword}
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-300 mb-1">
                  New Password
                </label>
                <input
                  type="password"
                  value={formData.newPassword}
                  onChange={(e) =>
                    setFormData((prev) => ({
                      ...prev,
                      newPassword: e.target.value,
                    }))
                  }
                  className="w-full px-3 py-2 bg-gray-700 text-white rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                  required={changePassword}
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-300 mb-1">
                  Confirm Password
                </label>
                <input
                  type="password"
                  value={formData.confirmPassword}
                  onChange={(e) =>
                    setFormData((prev) => ({
                      ...prev,
                      confirmPassword: e.target.value,
                    }))
                  }
                  className="w-full px-3 py-2 bg-gray-700 text-white rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                  required={changePassword}
                />
              </div>
            </div>
          )}
        </div>

        <div className="flex justify-end">
          <button
            type="submit"
            className="px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50 transition-colors"
            disabled={loading}
          >
            {loading ? "Saving..." : "Save Changes"}
          </button>
        </div>
      </form>
    </div>
  );
}

export default withAuth(ProfilePage);
