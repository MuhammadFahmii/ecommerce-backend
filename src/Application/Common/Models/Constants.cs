// ------------------------------------------------------------------------------------
// Constants.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

namespace netca.Application.Common.Models
{
    /// <summary>
    /// Constants
    /// </summary>
    public static class Constants
    {
        #region  healthcheck

        /// <summary>
        /// DefaultHealthCheckQuery
        /// </summary>
        public const string DefaultHealthCheckQuery = @"(SELECT ""Id"" FROM ""Changelogs"" LIMIT 1) UNION ALL
                                                    (SELECT ""Id"" FROM ""Assignments"" LIMIT 1)";

        /// <summary>
        /// DefaultHealthCheckUmsName
        /// </summary>
        public const string DefaultHealthCheckUmsName = "UserManagementService";

        /// <summary>
        /// DefaultHealthCheckGateWayName
        /// </summary>
        public const string DefaultHealthCheckGateWayName = "Gateway";

        /// <summary>
        /// DefaultHealthCheckMessageName
        /// </summary>
        public const string DefaultHealthCheckMessageName = "Message";

        /// <summary>
        /// DefaultHealthCheckRedisName
        /// </summary>
        public const string DefaultHealthCheckRedisName = "Redis";

        /// <summary>
        /// DefaultHealthCheckCpuUsage
        /// </summary>
        public const string DefaultHealthCheckCpuUsage = "CpuUsage";

        /// <summary>
        /// DefaultHealthCheckMemoryUsage
        /// </summary>
        public const string DefaultHealthCheckMemoryUsage = "Memory";

        /// <summary>
        /// DefaultHealthCheckDatabaseName
        /// </summary>
        public const string DefaultHealthCheckDatabaseName = "DB";
        #endregion healthcheck

        #region header


        /// <summary>
        /// HeaderJson
        /// </summary>
        public const string HeaderJson = "application/json";

        /// <summary>
        /// HeaderIfNoneMatch
        /// </summary>
        public const string HeaderIfNoneMatch = "If-None-Match";

        /// <summary>
        /// HeaderETag
        /// </summary>
        public const string HeaderETag = "ETag";

        #endregion header

        #region redis
        /// <summary>
        /// RedisSubKeyMessageConsume
        /// </summary>
        public const string RedisSubKeyMessageConsume = "MessageConsume";
        /// <summary>
        /// RedisSubKeyMessageProduce
        /// </summary>
        public const string RedisSubKeyMessageProduce = "MessageProduce";
        /// <summary>
        /// RedisSubKeyHttpRequest
        /// </summary>
        public const string RedisSubKeyHttpRequest = "HttpRequest";

        #endregion redis

        #region MsTeams

        /// <summary>
        /// MsTeamsImageWarning
        /// </summary>
        public const string MsTeamsImageWarning = "https://image.flaticon.com/icons/png/512/1537/premium/1537854.png";

        /// <summary>
        /// MsTeamsImageError
        /// </summary>
        public const string MsTeamsImageError = "https://image.flaticon.com/icons/png/512/2100/premium/2100813.png";

        /// <summary>
        /// MsTeamsSummary
        /// </summary>
        public const string MsTeamsSummaryError = "Someting wrong";

        /// <summary>
        /// MsTeamsactivitySubtitleStart
        /// </summary>
        public const string MsTeamsactivitySubtitleStart = "Application has started";

        /// <summary>
        /// MsTeamsactivitySubtitleStop
        /// </summary>
        public const string MsTeamsactivitySubtitleStop = "Application has stoped";

        /// <summary>
        /// MsTeamsThemeColorError
        /// </summary>
        public const string MsTeamsThemeColorError = "#eb090d";

        /// <summary>
        /// MsTeamsThemeColorWarning
        /// </summary>
        public const string MsTeamsThemeColorWarning = "#f7db05";

        /// <summary>
        /// MsTeamsThemeColorError
        /// </summary>
        public const string MsTeamsActivityImageType = "article";
        #endregion MsTeams

        #region  paging

        /// <summary>
        /// DefaultPageSize
        /// </summary>
        public const int DefaultPageSize = 10;

        /// <summary>
        /// DefaultPageSize
        /// </summary>
        public const int DefaultPageNumber = 1;

        #endregion paging

        #region  system

        /// <summary>
        /// format date yyyyMMddHHmmss
        /// </summary>
        public const string yyyyMMddHHmmss = "yyyyMMddHHmmss";

        /// <summary>
        /// SystemName
        /// </summary>
        public const string SystemName = "System";

        /// <summary>
        /// SystemCustomerName
        /// </summary>
        public const string SystemCustomerName = "PAMA";

        /// <summary>
        /// SystemEmail
        /// </summary>
        public const string SystemEmail = "System@unitedtractors.com";

        /// <summary>
        /// SystemClientId
        /// </summary>
        public const string SystemClientId = "datacaptureapps01";

        #endregion system

        #region  UserAttribute

        /// <summary>
        /// ClientId
        /// </summary>
        public const string ClientId = "client_id";

        /// <summary>
        /// CustomerCode
        /// </summary>
        public const string CustomerCode = "customer_code";

        /// <summary>
        /// PlantFieldName
        /// </summary>
        public const string PlantFieldName = "Plant";

        /// <summary>
        /// WorkCenterFieldName
        /// </summary>
        public const string WorkCenterFieldName = "Work Center";

        /// <summary>
        /// ServiceName
        /// </summary>
        public const string ServiceName = "netca";

        /// <summary>
        /// All
        /// </summary>
        public const string All = "*";

        #endregion UserAttribute
    }
}
