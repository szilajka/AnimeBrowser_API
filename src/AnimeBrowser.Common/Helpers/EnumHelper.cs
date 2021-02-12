using System;
using System.ComponentModel;
using System.Linq;

namespace AnimeBrowser.Common.Helpers
{
    public static class EnumHelper
    {
        public static string GetDescription(this Enum value)
        {
            return value.GetType()
            .GetField(value.ToString())
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .SingleOrDefault() is not DescriptionAttribute attribute ? value.ToString() : attribute.Description;
        }

        public static string GetIntValueAsString(this Enum value)
        {
            return value.ToString("d");
        }

        public static string GetDescriptionFromValue(string value, Type enumType)
        {
            if (int.TryParse(value, out int enumVal))
            {
                foreach (var val in Enum.GetValues(enumType))
                {
                    if ((int)val == enumVal)
                    {
                        return val.GetType().GetField(val.ToString())
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .SingleOrDefault() is not DescriptionAttribute attribute ? val.ToString() : attribute.Description;
                    }
                }
            }
            return "";
        }
    }
}
