// ------------------------------------------------------------------------------------
// JsonExtensions.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

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
                ContractResolver = new DefaultContractResolver{
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };
        }
        /// <summary>
        /// SerializerSettings
        /// </summary>
        /// <returns></returns>
        public static JsonSerializerSettings SerializerSettings()
        {
            return new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new DefaultContractResolver()
            };
        }
        /// <summary>
        /// SyncSerializerSettings
        /// </summary>
        /// <returns></returns>
        public static JsonSerializerSettings SyncSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
                Formatting = Formatting.Indented
            };
        }
    }
}
