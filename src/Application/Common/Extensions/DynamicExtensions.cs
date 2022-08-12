// ------------------------------------------------------------------------------------
// DynamicExtensions.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Dynamic;

namespace netca.Application.Common.Extensions;

/// <summary>
/// ClassExtensions
/// </summary>
public static class DynamicExtensions
{
    /// <summary>
    /// AddProperty
    /// </summary>
    /// <param name="expando"></param>
    /// <param name="propertyName"></param>
    /// <param name="propertyValue"></param>
    /// <param name="replace"></param>
    public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue,
        bool replace = true)
    {
        var exDict = expando as IDictionary<string, object>;
        if (exDict.ContainsKey(propertyName))
        {
            if (replace)
                exDict[propertyName] = propertyValue;
        }
        else
        {
            exDict.Add(propertyName, propertyValue);
        }
    }
}