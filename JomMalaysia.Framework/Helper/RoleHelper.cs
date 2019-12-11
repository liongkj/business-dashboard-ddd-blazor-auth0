using System.Collections.Generic;

namespace JomMalaysia.Framework.Helper
{
    public static class RoleHelper
    {
        private static readonly string Superadmin = "Superadmin";
        private static readonly string Manager = "Manager";
        private static readonly string Admin = "Admin";
        private static readonly string Editor = "Editor";

        public static readonly List<string> AuthorityList = new List<string>
        {
            Superadmin.ToUpper(), Superadmin,
            Manager.ToUpper(), Manager,
            Admin.ToUpper(), Admin,
            Editor.ToUpper(), Editor
        };

        public static List<string> GetAssignableRoles(string role)
        {
            var find = role.ToLower();
            var roleList = new List<string> {"manager", "admin", "editor"};
            switch (find)
            {
                case "superadmin":
                    break;
                case "manager":
                    roleList.RemoveAt(0);
                    break;
                case "admin":
                    roleList.RemoveRange(0, 2);
                    break;
                default:
                    return null;
            }

            return roleList;
        }
    }
}