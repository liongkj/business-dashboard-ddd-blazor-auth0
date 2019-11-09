using System;
using System.Collections.Generic;

namespace JomMalaysia.Framework.Helper
{
    public static class RoleHelper
    {
        private readonly static string Superadmin = "Superadmin";
        private readonly static string Manager = "Manager";
        private readonly static string Admin = "Admin";
        private readonly static string Editor = "Editor";

        public static readonly List<string> AuthorityList = new List<string>() {
            Superadmin.ToUpper(), Superadmin,
            Manager.ToUpper(), Manager,
            Admin.ToUpper(), Admin,
            Editor.ToUpper(), Editor, };

        public static List<string> GetAssignableRoles(string role)
        {
            var find = role.ToLower();
            List<string> roleList = new List<string>() { "superadmin", "manager", "admin", "editor" };
            switch (find)
            {
                case ("superadmin"):
                    break;
                case ("manager"):
                    roleList.RemoveAt(0);
                    break;
                case ("admin"):
                    roleList.RemoveRange(0, 2);
                    break;
                default:
                    return null;
            }
            return roleList;
        }

        
    }
}