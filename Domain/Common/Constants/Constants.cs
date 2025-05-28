using System.Reflection;

namespace Domain.Common.Constants
{
    public class Roles
    {
        public const string USER = "User";
        public const string SUB_ADMIN = "SubAdmin";
        public const string ADMIN = "Admin";

        public static List<string> GetAllRoles()
        {
            List<string> roles = [];

            // Get all public static readonly fields in the class
            var fields = typeof(Roles).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            foreach (var field in fields)
            {
                // Check if the field is a string and readonly
                if (field.FieldType == typeof(string))
                {
                    // Add the value to the list
                    roles.Add((string)field.GetValue(null));
                }
            }

            return roles;
        }
    }

    public static class Policies
    {
        public const string ADMIN_SUB_ADMIN_POLICY = "Admin_Sub_ADMIN_POLICY";
        public const string ADMIN_POLICY = "Admin_Policy";
        public const string USER_POLICY = "User_Policy";
    }
}