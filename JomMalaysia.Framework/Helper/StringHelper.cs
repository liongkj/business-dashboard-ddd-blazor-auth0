using System.Collections.Generic;
using System.Text;

namespace JomMalaysia.Framework.Helper
{
    public static class StringHelper
    {
        public static string CapitalizeOrConvertNullToEmptyString(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static string ListToString(this IEnumerable<object> objects)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var obj in objects)
            {
                builder.Append(obj).Append(',');
            }

            return builder.ToString();
        }
    }
}