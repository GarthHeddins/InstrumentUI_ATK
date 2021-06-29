using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstrumentUI_ATK.Common
{
    /// <summary>
    /// This class contains all the extension methods
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Extension method for checking that the passed string is not null, 
        /// non-empty and not comprised of only spaces
        /// </summary>
        /// <param name="val">The string itself that needs to be checked</param>
        /// <returns>false if it is a genuine string value</returns>
        public static bool IsNullOrWhiteSpace(this string val)
        {
            if (string.IsNullOrEmpty(val))
                return true;

            val = val.Trim();
            if (string.IsNullOrEmpty(val))
                return true;

            return false;
        }

        /// <summary>
        /// Extension method to get the distinct item based on a property from a specified collection
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> enumerable, Func<TSource, TSource, bool> comparer)
        {
            return enumerable.Distinct(new LambdaComparer<TSource>(comparer));
        }

        /// <summary>
        /// returns true, if the string is numeric otherwise returns false.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string val)
        {
            val = val.Trim();

            if (val.Length > 0)
            {
                double num;
                return double.TryParse(val, out num);
            }
            else
            {
                return false;
            }
        }

        public static bool Failed(this StringBuilder sbResult)
        {
            return sbResult.ToString().Contains("-1");
        }

        /// <summary>
        /// This function returns true if user name starts with demo, otherwise returns false
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsDemo(this InstrumentUI_ATK.DataService.User user)
        {
            if (user == null)
                return false;
            if (user.UserName.ToLower().StartsWith("demo"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// This function returns true if user is admin otherwise returns false
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsAdmin(this InstrumentUI_ATK.DataService.User user)
        {
            if (user == null || user.UserRole == null)
                return false;
            if (user.UserRole.RoleName.ToLower() == "admin")
                return true;
            else
                return false;
        }
    }
}
