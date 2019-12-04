using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;

namespace JomMalaysia.Framework.Helper
{
    public static class EnumHelper
    {
        
        
        
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name == null) return null;
            System.Reflection.FieldInfo field = type.GetField(name);
            if (field == null) return null;
            if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attr)
            {
                return attr.Description;
            }

            return null;
        }
        
        
    }
}