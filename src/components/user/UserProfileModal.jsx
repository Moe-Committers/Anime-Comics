import { useState } from "react";
import { toast, Toaster } from "sonner";
import { X, Upload, User } from "lucide-react";
import { userService } from "@/services/user.service";

export function UserProfileModal({ user, onClose, onSuccess }) {
  const [loading, setLoading] = useState(false);
  const [mode, setMode] = useState("profile"); // 'profile' or 'password'
  const [preview, setPreview] = useState(
    user.profile ? `${process.env.NEXT_PUBLIC_API_URL}${user.profile}` : null
  );

  const [formData, setFormData] = useState({
    name: user.name || "",
    fileImg: null,
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

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      if (mode === "profile") {
        const profileData = new FormData();
        if (formData.name !== user.name) {
          profileData.append("name", formData.name);
        }
        if (formData.fileImg) {
          profileData.append("fileImg", formData.fileImg);
        }

        await userService.adminUpdateUserProfile(user.id, profileData);
        toast.success("User profile updated successfully");
      } else {
        if (formData.newPassword !== formData.confirmPassword) {
          throw new Error("Passwords do not match");
        }

        await userService.adminChangeUserPassword(user.id, {
          newPassword: formData.newPassword,
          confirmPassword: formData.confirmPassword,
        });
        toast.success("User password updated successfully");
      }

      onSuccess();
    } catch (error) {
      toast.error(error.message || `Failed to update user ${mode}`);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
      <Toaster/>
      <div className="bg-gray-800 rounded-lg p-6 w-full max-w-md">
        <div className="flex justify-between items-center mb-4">
          <h2 className="text-xl font-bold text-white">
            Help User: {user.name}
          </h2>
          <button onClick={onClose} className="text-gray-400 hover:text-white">
            <X className="h-5 w-5" />
          </button>
        </div>

        <div className="flex gap-4 mb-6">
          <button
            onClick={() => setMode("profile")}
            className={`flex-1 py-2 rounded-lg ${
              mode === "profile"
                ? "bg-blue-600 text-white"
                : "bg-gray-700 text-gray-300 hover:bg-gray-600"
            }`}
          >
            Update Profile
          </button>
          <button
            onClick={() => setMode("password")}
            className={`flex-1 py-2 rounded-lg ${
              mode === "password"
                ? "bg-blue-600 text-white"
                : "bg-gray-700 text-gray-300 hover:bg-gray-600"
            }`}
          >
            Change Password
          </button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          {mode === "profile" ? (
            <>
              <div className="flex flex-col items-center gap-4">
                <div className="relative w-24 h-24">
                  {preview ? (
                    <img
                      src={preview}
                      alt="Profile"
                      className="w-full h-full rounded-full object-cover"
                    />
                  ) : (
                    <div className="w-full h-full rounded-full bg-gray-700 flex items-center justify-center">
                      <User className="h-12 w-12 text-gray-400" />
                    </div>
                  )}
                  <label className="absolute bottom-0 right-0 p-1 bg-gray-700 rounded-full cursor-pointer hover:bg-gray-600">
                    <Upload className="h-4 w-4 text-white" />
                    <input
                      type="file"
                      accept="image/*"
                      onChange={handleImageChange}
                      className="hidden"
                    />
                  </label>
                </div>
              </div>

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
            </>
          ) : (
            <>
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
                  required={mode === "password"}
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
                  required={mode === "password"}
                />
              </div>
            </>
          )}

          <div className="flex justify-end gap-3">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 text-gray-400 hover:text-white"
              disabled={loading}
            >
              Cancel
            </button>
            <button
              type="submit"
              className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50"
              disabled={loading}
            >
              {loading ? "Saving..." : "Save Changes"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
