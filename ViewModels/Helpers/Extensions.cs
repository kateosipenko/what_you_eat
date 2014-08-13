using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViewModels
{
    internal static class Extensions
    {
        public static string AddPageParameter(this string page, string key, object value)
        {
            string result = page;
            if (value == null)
                return result;

            if (!result.Contains("?"))
            {
                result = result + "?" + key + "=" + value;
            }
            else
            {
                result = result + "&" + key + "=" + value;
            }

            return result;
        }
    }
}
