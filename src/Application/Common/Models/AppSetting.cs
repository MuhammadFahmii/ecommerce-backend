// ------------------------------------------------------------------------------------
// AppSetting.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Collections.Generic;

namespace netca.Application.Common.Models
{
    /// <summary>
    /// AppSetting
    /// </summary>
     /// <summary>
    /// AppSetting
    /// </summary>
    public class AppSetting
    {
        /// <summary>
        /// IsEnableAuth
        /// </summary>
        /// <value></value>
        public bool IsEnableAuth { get; set; } = true;
        
        /// <summary>
        /// IsEnableDetailError
        /// </summary>
        /// <value></value>
        public bool IsEnableDetailError { get; set; } = false;

        /// <summary>
        /// EvaluationTimeinInS
        /// </summary>
        /// <value></value>
        public int EvaluationTimeinInS { get; set; } = 10;

        /// <summary>
        /// MinimumSecondsBetweenFailureNotifications
        /// </summary>
        /// <value></value>
        public int MinimumSecondsBetweenFailureNotifications { get; set; } = 60;

        /// <summary>
        /// RequestPerformanceInMs
        /// </summary>
        /// <value></value>
        public int RequestPerformanceInMs { get; set; } = 500;

        /// <summary>
        /// ConnectionStrings
        /// </summary>
        /// <returns></returns>
        public ConnectionStrings ConnectionStrings { get; set; } = new();

        /// <summary>
        /// Kestrel
        /// </summary>
        /// <returns></returns>
        public Kestrel Kestrel { get; set; } = new Kestrel();

        /// <summary>
        /// CorsOrigin
        /// </summary>
        /// <value></value>
        public string CorsOrigin { get; set; } = "https://*.unitedtractors.com";

        /// <summary>
        /// Bot
        /// </summary>
        /// <returns></returns>
        public Bot Bot { get; set; } = new();

        /// <summary>
        /// AuthorizationServer
        /// </summary>
        /// <returns></returns>
        public AuthorizationServer AuthorizationServer { get; set; } = new();

        /// <summary>
        /// DatabaseSettings
        /// </summary>
        /// <returns></returns>
        public DatabaseSettings DatabaseSettings { get; set; } = new();


        /// <summary>
        /// App
        /// </summary>
        /// <returns></returns>
        public App App { get; set; } = new App();
        
        /// <summary>
        /// RedisServer
        /// </summary>
        /// <returns></returns>
        public RedisServer RedisServer { get; set; } = new();
        
        /// <summary>
        /// BackgroundJob
        /// </summary>
        public BackgroundJob BackgroundJob { get; set; } = new();
    }

    /// <summary>
    /// AuthorizationServer
    /// </summary>
    public class AuthorizationServer
    {
        /// <summary>
        /// Gateway
        /// </summary>
        /// <value></value>
        public string Gateway { get; set; } = "https://gateway-dev.unitedtractors.com/dev/internal";

        /// <summary>
        /// Address
        /// </summary>
        /// <value></value>
        public string Address { get; set; } = "http://usermanagementservice.dev-rke.unitedtractors.com";

        /// <summary>
        /// WhiteListPathSegment
        /// </summary>
        /// <value></value>
        public string WhiteListPathSegment { get; set; } = "/swagger,/health";

        /// <summary>
        /// Header
        /// </summary>
        public string Header { get; set; } = "Ocp-Apim-Subscription-Key";

        /// <summary>
        /// Secret
        /// </summary>
        /// <value></value>
        public string Secret { get; set; } = "";

        /// <summary>
        /// Service
        /// </summary>
        /// <value></value>
        public string Service { get; set; } = "netca";

        /// <summary>
        /// Policy
        /// </summary>
        /// <value></value>
        public List<Policy> Policy { get; set; } = new();
    }
    /// <summary>
    /// Policy
    /// </summary>
    public class Policy
    {
        /// <summary>
        /// Name
        /// </summary>
        /// <value></value>
        public string Name { get; set; } = "";

        /// <summary>
        /// IsCheck
        /// </summary>
        /// <value></value>
        public bool IsCheck { get; set; } = false;
        
        /// <summary>
        /// IsCache
        /// </summary>
        /// <value></value>
        public bool IsCache { get; set; } = false;
    }
    
    /// <summary>
    /// DatabaseSettings
    /// </summary>
    public class DatabaseSettings
    {
        /// <summary>
        /// MaxRetryDelay
        /// </summary>
        /// <value></value>
        public int MaxRetryDelay { get; set; } = 5;

        /// <summary>
        /// MaxRetryCount
        /// </summary>
        /// <value></value>
        public int MaxRetryCount { get; set; } = 100;

        /// <summary>
        /// CommandTimeout
        /// </summary>
        /// <value></value>
        public int CommandTimeout { get; set; } = 60;

        /// <summary>
        /// Migrations
        /// </summary>
        /// <value></value>
        public bool Migrations { get; set; } = false;

        /// <summary>
        /// SeedData
        /// </summary>
        /// <value></value>
        public bool SeedData { get; set; } = false;
    }

    /// <summary>
    /// App
    /// </summary>
    public class App
    {
        /// <summary>
        /// Title
        /// </summary>
        /// <value></value>
        public string Title { get; set; } = "netca API";

        /// <summary>
        /// Description
        /// </summary>
        /// <value></value>
        public string Description { get; set; } = "This is awesome app";

        /// <summary>
        /// Version
        /// </summary>
        /// <value></value>
        public string Version { get; set; } = "1.0.0";

        /// <summary>
        /// AppContact
        /// </summary>
        /// <value></value>
        public AppContact AppContact { get; set; } = new();
    }

    /// <summary>
    /// Kestrel
    /// </summary>
    public class Kestrel
    {
        
        /// <summary>
        /// KeepAliveTimeoutInM
        /// </summary>
        /// <value></value>
        public int KeepAliveTimeoutInM { get; set; } = 2;

        /// <summary>
        /// MinRequestBodyDataRate
        /// </summary>
        /// <value></value>
        public MinRequestBodyDataRate MinRequestBodyDataRate { get; set; } = new();

        /// <summary>
        /// MinResponseDataRate
        /// </summary>
        /// <value></value>
        public MinResponseDataRate MinResponseDataRate { get; set; } = new();
    }

    /// <summary>
    /// MinRequestBodyDataRate
    /// </summary>
    public class MinRequestBodyDataRate
    {
        
        /// <summary>
        /// bytesPerSecond
        /// </summary>
        /// <value></value>
        public double BytesPerSecond { get; set; } = 100;

        /// <summary>
        /// gracePeriod
        /// </summary>
        /// <value></value>
        public int GracePeriod { get; set; } = 10;
    }

    /// <summary>
    /// MinResponseDataRate
    /// </summary>
    public class MinResponseDataRate
    {
        
        /// <summary>
        /// bytesPerSecond
        /// </summary>
        /// <value></value>
        public double BytesPerSecond { get; set; } = 100;

        /// <summary>
        /// gracePeriod
        /// </summary>
        /// <value></value>
        public int GracePeriod { get; set; } = 10;
    }

    /// <summary>
    /// AppContact
    /// </summary>
    public class AppContact
    {
        /// <summary>
        /// Company
        /// </summary>
        /// <value></value>
        public string Company { get; set; } = "PT United Tractors Tbk";

        /// <summary>
        /// Email
        /// </summary>
        /// <value></value>
        public string Email { get; set; } = "cst_dev42@unitedtractors.com";

        /// <summary>
        /// Uri
        /// </summary>
        /// <value></value>
        public string Uri { get; set; } = "https://www.unitedtractors.com";
    }
    
    /// <summary>
    /// Bot
    /// </summary>
    public class Bot
    {
        /// <summary>
        /// IsEnable
        /// </summary>
        /// <value></value>
        public bool IsEnable { get; set; } = false;

        /// <summary>
        /// ServiceName
        /// </summary>
        /// <value></value>
        public string ServiceName { get; set; } = "DCA - netca";
        
        /// <summary>
        /// ServiceDomain
        /// </summary>
        /// <value></value>
        public string ServiceDomain { get; set; } = "netca.dev-aks.unitedtractors.com";

        /// <summary>
        /// Address
        /// </summary>
        /// <value></value>
        public string Address { get; set; } = "https://gateway-dev.unitedtractors.com/dev/internal/dad/msteams";

        /// <summary>
        /// Header
        /// </summary>
        public string Header { get; set; } = "Ocp-Apim-Subscription-Key";

        /// <summary>
        /// Secret
        /// </summary>
        /// <value></value>
        public string Secret { get; set; } = "";
    }

    /// <summary>
    /// ConnectionStrings
    /// </summary>
    public class ConnectionStrings
    {
        /// <summary>
        /// netcaDb
        /// </summary>
        /// <returns></returns>
        public string DefaultConnection { get; set; } = "netca.db";
    }
    
    /// <summary>
    /// RedisServer
    /// </summary>
    public class RedisServer
    {
        /// <summary>
        /// Server
        /// </summary>
        /// <value></value>
        public string Server { get; set; } = "";

        /// <summary>
        /// InstanceName
        /// </summary>
        /// <value></value>
        public string InstanceName { get; set; } = "redis";

        /// <summary>
        /// DatabaseNumber
        /// </summary>
        public int DatabaseNumber { get; set; } = 0;

        /// <summary>
        /// RequestExpiryInMinutes
        /// </summary>
        /// <value></value>
        public int RequestExpiryInMinutes { get; set; } = 30;

        /// <summary>
        /// MessageExpiryInDays
        /// </summary>
        /// <value></value>
        public int MessageExpiryInDays { get; set; } = 180;

        /// <summary>
        /// Policy
        /// </summary>
        /// <value></value>
        public List<Policy> Policy { get; set; } = new();
    }
    
    /// <summary>
    /// BackgroundJob
    /// </summary>
    public class BackgroundJob
    {
        /// <summary>
        /// IsEnable
        /// </summary>
        public bool IsEnable { get; set; } = true;
        
        /// <summary>
        /// UsePersistentStore
        /// </summary>
        public bool UsePersistentStore { get; set; } = false;
        
        /// <summary>
        /// PersistentStore
        /// </summary>
        public PersistentStore PersistentStore { get; set; } = new();
        /// <summary>
        /// Jobs
        /// </summary>
        public List<Job> Jobs { get; set; } = new();
    }
    
    /// <summary>
    /// PersistentStore
    /// </summary>
    public class PersistentStore
    {
        /// <summary>
        /// ConnectionString
        /// </summary>
        public string ConnectionString { get; set; } = "";
        
        /// <summary>
        /// IgnoreDuplicates
        /// </summary>
        public bool IgnoreDuplicates { get; set; } = true;
        
        /// <summary>
        /// OverWriteExistingData
        /// </summary>
        public bool OverWriteExistingData { get; set; } = true;
        
        /// <summary>
        /// MaxConcurrency
        /// </summary>
        public int MaxConcurrency { get; set; } = 10;
        
        /// <summary>
        /// RetryInterval
        /// </summary>
        public int RetryInterval { get; set; } = 15;
        
        /// <summary>
        /// CheckinInterval
        /// </summary>
        public int CheckinInterval { get; set; } = 15000;
        
        /// <summary>
        /// CheckinMisfireThreshold
        /// </summary>
        public int CheckinMisfireThreshold { get; set; } = 15000;
        
        /// <summary>
        /// MisfireThreshold
        /// </summary>
        public int MisfireThreshold { get; set; } = 15000;
        
        /// <summary>
        /// TablePrefix
        /// </summary>
        public string TablePrefix { get; set; } = "QRTZ_";
    }
    
    /// <summary>
    /// Job
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// IsParallel
        /// </summary>
        public bool IsParallel { get; set; } = false;

        /// <summary>
        /// Schedule
        /// </summary>
        public string Schedule { get; set; } = "";
        
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; } = "";
    }
}
