// ------------------------------------------------------------------------------------
// StringExtensions.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace netca.Application.Common.Extensions
{
    /// <summary>
    /// StringExtensions
    /// </summary>
    public static class StringExtensions
    {

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
        /// ToPolicyNameFormat
        /// </summary>
        /// <returns></returns>
        public static string ToPolicyNameFormat(string serviceName = "", string httpMethod = "", List<string> path = null)
        {
            var policy = "";
            if (path is not { Count: > 3 }) return policy;
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
    }
}
