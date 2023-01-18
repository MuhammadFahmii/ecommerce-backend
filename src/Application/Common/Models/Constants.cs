// ------------------------------------------------------------------------------------
// Constants.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

namespace netca.Application.Common.Models;

/// <summary>
/// Constants
/// </summary>
public static class Constants
{
    #region healthcheck

    /// <summary>
    /// DefaultHealthCheckQuery
    /// </summary>
    public const string DefaultHealthCheckQuery = @"SELECT TOP 1 Id FROM Changelogs UNION ALL 
                                      SELECT TOP 1 Id FROM TodoItems UNION ALL 
                                      SELECT TOP 1 Id FROM TodoLists;";

    /// <summary>
    /// DefaultHealthCheckTimeoutInSeconds
    /// </summary>
    public const int DefaultHealthCheckTimeoutInSeconds = 60;

    /// <summary>
    /// DefaultHealthCheckUmsName
    /// </summary>
    public const string DefaultHealthCheckUmsName = "userManagementService";

    /// <summary>
    /// DefaultHealthCheckGateWayName
    /// </summary>
    public const string DefaultHealthCheckGateWayName = "gateway";

    /// <summary>
    /// DefaultHealthCheckMessageName
    /// </summary>
    public const string DefaultHealthCheckMessageName = "message";

    /// <summary>
    /// DefaultHealthCheckRedisName
    /// </summary>
    public const string DefaultHealthCheckRedisName = "redis";

    /// <summary>
    /// DefaultHealthCheckCpuUsage
    /// </summary>
    public const string DefaultHealthCheckCpuUsage = "cpuUsage";

    /// <summary>
    /// DefaultHealthCheckMemoryUsage
    /// </summary>
    public const string DefaultHealthCheckMemoryUsage = "memory";

    /// <summary>
    /// DefaultHealthCheckDatabaseName
    /// </summary>
    public const string DefaultHealthCheckDatabaseName = "db";

    /// <summary>
    /// DefaultHealthCheckEventHub
    /// </summary>
    public const string DefaultHealthCheckEventHub = "eventHub";

    /// <summary>
    /// DefaultPercentageUsedDegraded
    /// </summary>
    public const byte DefaultHealthCheckPercentageUsedDegraded = 90;

    #endregion healthcheck

    #region header

    /// <summary>
    /// HeaderJson
    /// </summary>
    public const string HeaderJson = "application/json";
    
    /// <summary>
    /// HeaderJsonVndApi
    /// </summary>
    public const string HeaderJsonVndApi = "application/vnd.api+json";

    /// <summary>
    /// HeaderPdf
    /// </summary>
    public const string HeaderPdf = "application/pdf";

    /// <summary>
    /// HeaderTextPlain
    /// </summary>
    public const string HeaderTextPlain = "text/plain";

    /// <summary>
    /// HeaderOctetStream
    /// </summary>
    public const string HeaderOctetStream = "application/octet-stream";

    /// <summary>
    /// HeaderProblemJson
    /// </summary>
    public const string HeaderProblemJson = "application/problem+json";

    /// <summary>
    /// HeaderTextCsv
    /// </summary>
    public const string HeaderTextCsv = "text/csv";

    /// <summary>
    /// HeaderExcelXls
    /// </summary>
    public const string HeaderExcelXls = "application/vnd.ms-excel";

    /// <summary>
    /// HeaderExcelXlsx
    /// </summary>
    public const string HeaderExcelXlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    /// <summary>
    /// HeaderImageJpg
    /// </summary>
    public const string HeaderImageJpg = "image/jpg";

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
    public const string MsTeamsactivitySubtitleStop = "Application has stopped";

    /// <summary>
    /// MsTeamsThemeColorError
    /// </summary>
    public const string MsTeamsThemeColorError = "#eb090d";

    /// <summary>
    /// MsTeamsThemeColorWarning
    /// </summary>
    public const string MsTeamsThemeColorWarning = "#f7db05";

    /// <summary>
    /// MsTeamsMaxSizeInBytes
    /// </summary>
    public const int MsTeamsMaxSizeInBytes = 5_242_880;

    #endregion MsTeams

    #region paging

    /// <summary>
    /// DefaultPageSize
    /// </summary>
    public const int DefaultPageSize = 10;

    /// <summary>
    /// DefaultPageSize
    /// </summary>
    public const int DefaultPageNumber = 1;

    #endregion paging

    #region Filter&SortSeparator

    /// <summary>
    /// Comma Separator
    /// </summary>
    public const string EscapedCommaPattern = @"(?<!($|[^\\])(\\\\)*?\\),";

    /// <summary>
    /// Pipe Separator
    /// </summary>
    public const string EscapedPipePattern = @"(?<!($|[^\\])(\\\\)*?\\)\|";

    /// <summary>
    /// Comma Separator
    /// </summary>
    public static readonly string[] Operators = new[]
    {
        "!@=*",
        "!_=*",
        "!=*",
        "!@=",
        "!_=",
        "==*",
        "@=*",
        "_=*",
        "==",
        "!=",
        ">=",
        "<=",
        ">",
        "<",
        "@=",
        "_="
    };

    #endregion

    #region system

    /// <summary>
    /// format date yyyyMMddHHmmss
    /// </summary>
    public const string YyyyMMddHHmmss = "yyyyMMddHHmmss";

    /// <summary>
    /// SystemName
    /// </summary>
    public const string SystemName = "System";

    /// <summary>
    /// SystemCustomerName
    /// </summary>
    public const string SystemCustomerName = "PAMA";
    
    /// <summary>
    /// SystemId
    /// </summary>
    public static readonly Guid SystemId = new Guid("00000000-0000-0000-0000-000000000000");

    /// <summary>
    /// SystemEmail
    /// </summary>
    public const string SystemEmail = "System@unitedtractors.com";

    /// <summary>
    /// SystemClientId
    /// </summary>
    public const string SystemClientId = "datacaptureapps01";
    
    /// <summary>
    /// Max data
    /// </summary>
    public const int MaxData = 300;

    #endregion system

    #region api

    /// <summary>
    /// ApiErrorDescription
    /// </summary>
    public static class ApiErrorDescription
    {
        /// <summary>
        /// BadRequest
        /// </summary>
        public const string BadRequest = "BadRequest";

        /// <summary>
        /// Unauthorized
        /// </summary>
        public const string Unauthorized = "Unauthorized";

        /// <summary>
        /// Forbidden
        /// </summary>
        public const string Forbidden = "Forbidden";

        /// <summary>
        /// InternalServerError
        /// </summary>
        public const string InternalServerError = "InternalServerError";
    }

    #endregion api

    #region UserAttribute

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
    /// CustomerSiteFieldName
    /// </summary>
    public const string CustomerSiteFieldName = "Customer Site";

    /// <summary>
    /// ABCFieldName
    /// </summary>
    public const string ABCFieldName = "ABCInd";

    /// <summary>
    /// CustomerName
    /// </summary>
    public const string CustomerName = "Customer";

    /// <summary>
    /// WorkCenterFieldName
    /// </summary>
    public const string WorkCenterFieldName = "Work Center";

    /// <summary>
    /// All
    /// </summary>
    public const string All = "*";

    #endregion UserAttribute

    #region  validation

    /// <summary>
    /// Maximum Length Base64 in byte
    /// </summary>
    public const int MaximumLengthBase64 = 512 * 1024;

    /// <summary>
    /// Maximum File Size in byte
    /// </summary>
    public const int MaximumFileSize = 3072 * 1024;

    #endregion
}