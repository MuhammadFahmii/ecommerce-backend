// ------------------------------------------------------------------------------------
// StringExtensions.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace netca.Application.Common.Extensions
{
    /// <summary>
    /// StringExtensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// AsRelativeResource
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        public static string AsRelativeResource(this string resourcePath)
        {
            return resourcePath.StartsWith("/") ? resourcePath.Substring(1) : resourcePath;
        }

        /// <summary>
        /// Truncate
        /// </summary>
        /// <param name="s"></param>
        /// <param name="maxChars"></param>
        /// <returns></returns>
        public static string Truncate(string s, int maxChars)
        {
            if (s == null)
                return s;
            return s.Length <= maxChars ? s : s.Substring(0, maxChars);
        }

        /// <summary>
        /// SplitArrayString
        /// </summary>
        /// <param name="arrayString"></param>
        /// <returns></returns>
        public static string SplitArrayString(string arrayString)
        {
            string result = "";
            var splitString = arrayString.Split(' ');
            if (splitString.Length > 1)
            {
                bool first = true;
                foreach (var category in splitString)
                {
                    if (first)
                    {
                        first = false;
                        continue;
                    }

                    result += category + " ";
                }

                return result;
            }
            else
            {
                return arrayString;
            }
        }

        /// <summary>
        /// JsonRepair
        /// </summary>
        /// <param name="value"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public static string JsonRepair(string value, string regex)
        {
            var result = value;
            var regexs = regex != null ? regex.Split(",").ToList() : new List<string>();
            foreach (var item in regexs)
            {
                var regexReplace = Regex.Escape($@"{item}");
                result = Regex.Replace(result, regexReplace, "");
            }

            return result;
        }

        /// <summary>
        /// ReplaceLast
        /// </summary>
        /// <param name="find"></param>
        /// <param name="replace"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceLast(string find, string replace, string str)
        {
            int lastIndex = str.LastIndexOf(find);

            if (lastIndex == -1)
                return str;

            string beginString = str.Substring(0, lastIndex);
            string endString = str.Substring(lastIndex + find.Length);

            return beginString + replace + endString;
        }

        /// <summary>
        /// ToCamelCase
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string value)
        {
            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        /// <summary>
        /// ToPolicyNameFormat
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="httpMethod"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToPolicyNameFormat(string serviceName = "", string httpMethod = "", List<string> path = null)
        {
            var policy = "";
            if (path is not { Count: > 3 })
                return policy;

            try
            {
                policy = serviceName + ":" + httpMethod + ":" + path[3] + "_" + string.Join("_", path.Skip(4));
            }
            catch
            {
                // ignored
            }

            return policy;
        }

        /// <summary>
        /// GetQueryString
        /// </summary>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetQueryString(string url, object obj)
        {
            var properties = obj.GetType().GetProperties()
                .Where(x => x.CanRead && x.GetValue(obj, null) != null)
                .ToDictionary(x => x.Name, x => x.GetValue(obj, null));

            var propertyNames = properties
                .Where(x => !(x.Value is string) && x.Value is IEnumerable)
                .Select(x => x.Key)
                .ToList();

            var ienumerableProperties = new List<string>();

            foreach (var key in propertyNames)
            {
                var valueType = properties[key].GetType();
                var valueElemType = valueType.IsGenericType
                                        ? valueType.GetGenericArguments()[0]
                                        : valueType.GetElementType();
                if (valueElemType.IsPrimitive || valueElemType == typeof(string))
                {
                    var enumerable = properties[key] as IEnumerable;
                    foreach (var item in enumerable)
                    {
                        ienumerableProperties.Add(key + "=" + item.ToString());
                    }
                }

                properties.Remove(key);
            }

            return url + "?" +
                string.Join("&", properties
                    .Select(x => string.Concat(
                        Uri.EscapeDataString(x.Key),
                        "=",
                        Uri.EscapeDataString(x.Value.ToString())))
                    ) +
                string.Join("&", ienumerableProperties);
        }

        /// <summary>
        /// Get Attribute List
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<string> GetAttribute(this Dictionary<string, List<string>> dict, string key)
        {
            if (dict == null || !dict.ContainsKey(key))
                return new List<string>();

            return dict[key].Distinct().ToList();
        }

        /// <summary>
        /// NullSafeToLower
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NullSafeToLower(this string value)
        {
            if (value == null)
                return value;

            return value.ToLower();
        }
    }
}