// ------------------------------------------------------------------------------------
// JsonExtensions.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace Newtonsoft.Json.Serialization
{
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
                },
            };
        }

        /// <summary>
        /// SerializerSettings.
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static JsonSerializerSettings SerializerSettings(ILogger logger)
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
        /// SyncSerializerSettings.
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static JsonSerializerSettings SyncSerializerSettings(ILogger logger)
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
                Error = HandleDeserializationError(logger),
                Formatting = Formatting.Indented
            };
        }

        /// <summary>
        /// HandleDeserializationError.
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static EventHandler<ErrorEventArgs> HandleDeserializationError(ILogger logger)
        {
            void HandleErrorParsing(object? sender, ErrorEventArgs errorArgs)
            {
                var currentError = errorArgs.ErrorContext.Error.Message;

                logger.LogWarning("Error when serialize value: {currentError}", currentError);

                errorArgs.ErrorContext.Handled = true;
            }

            return HandleErrorParsing;
        }
    }
}