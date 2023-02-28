// ------------------------------------------------------------------------------------
// MessagingExtensions.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using ecommerce.Application.Common.Models;

namespace ecommerce.Application.Common.Extensions;

/// <summary>
/// MessagingExtensions
/// </summary>
public static class MessagingExtensions
{
    /// <summary>
    /// GetTopicValue
    /// </summary>
    /// <param name="value"></param>
    /// <param name="name"></param>
    /// <param name="topicName"></param>
    /// <returns></returns>
    public static string? GetTopicValue(this AppSetting value, string name, string topicName)
    {
        if (value == null)
            return null;

        var topicData = value?.Messaging?.AzureEventHubs.Find(x => x.Name.Equals(name))?.Topics;
        var topic = topicData?.Find(x => x.Name.Equals(topicName))?.Value;

        return topic;
    }
}