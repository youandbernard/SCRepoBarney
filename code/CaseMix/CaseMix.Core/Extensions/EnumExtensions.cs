using System;
using System.ComponentModel;

namespace CaseMix.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescription(this Enum val)
        {
            if (Enum.IsDefined(val.GetType(), val))
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
                return attributes.Length > 0 ? attributes[0].Description : Enum.GetName(val.GetType(), val);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
