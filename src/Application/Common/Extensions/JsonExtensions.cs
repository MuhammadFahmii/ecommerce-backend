// ------------------------------------------------------------------------------------
// JsonExtensions.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace Newtonsoft.Json.Serialization;

/// <summary>
/// JsonExtensions
/// </summary>
public static class JsonExtensions
{
    /// <summary>
    /// ErrorSerializerSettings
    /// </summary>
    /// <returns></returns>
    public static JsonSerializerSettings ErrorSerializerSettings()
    {
        return new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };
    }

    /// <summary>
    /// SerializerSettings
    /// </summary>
    /// <param name="logger"></param>
    /// <returns></returns>
    public static JsonSerializerSettings SerializerSettings(ILogger? logger = null)
    {
        return new JsonSerializerSettings
        {
            Error = HandleDeserializationError(logger),
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new DefaultContractResolver()
        };
    }

    /// <summary>
    /// SyncSerializerSettings
    /// </summary>
    /// <param name="logger"></param>
    /// <returns></returns>
    public static JsonSerializerSettings SyncSerializerSettings(ILogger? logger = null)
    {
        return new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver(),
            Error = HandleDeserializationError(logger),
            Formatting = Formatting.Indented
        };
    }

    /// <summary>
    /// HandleDeserializationError
    /// </summary>
    /// <param name="logger"></param>
    /// <returns></returns>
    public static EventHandler<ErrorEventArgs> HandleDeserializationError(ILogger? logger = null)
    {
        void HandleErrorParsing(object sender, ErrorEventArgs errorArgs)
        {
            var currentError = errorArgs.ErrorContext.Error.Message;

            if (logger != null)
                logger.LogWarning("Error when serialize value: {message}", currentError);

            errorArgs.ErrorContext.Handled = true;
        }

        return HandleErrorParsing!;
    }
}