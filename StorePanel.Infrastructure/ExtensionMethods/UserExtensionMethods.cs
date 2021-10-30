using StorePanel.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StorePanel.Infrastructure.ExtensionMethods
{
    public static class UserExtensionMethods
    {
        public static string GetAvatar(this User user)
        {
            if (string.IsNullOrEmpty(user.Avatar) == false && File.Exists(Path.Combine("UploadedFiles", "UserAvatars", user.Avatar)))
            {
                return $"/UploadedFiles/UserAvatars/{user.Avatar}";
            }
            else
            {
                return "/UploadedFiles/UserAvatars/user-avatar.png";
            }
        }
    }
}
