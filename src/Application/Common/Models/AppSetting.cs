// ------------------------------------------------------------------------------------
// AppSetting.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

namespace netca.Application.Common.Models;

/// <summary>
/// AppSetting
/// </summary>
public record AppSetting
{
    /// <summary>
    /// Gets or sets a value indicating whether isEnableAuth
    /// </summary>
    /// <value></value>
    public bool IsEnableAuth { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether isEnableDetailError
    /// </summary>
    /// <value></value>
    public bool IsEnableDetailError { get; set; }

    /// <summary>
    /// Gets or sets minimumSecondsBetweenFailureNotifications
    /// </summary>
    /// <value></value>
    public int MinimumSecondsBetweenFailureNotifications { get; set; } = 60;

    /// <summary>
    /// Gets or sets requestPerformanceInMs
    /// </summary>
    /// <value></value>
    public int RequestPerformanceInMs { get; set; } = 500;

    /// <summary>
    /// Gets or sets connectionStrings
    /// </summary>
    /// <returns></returns>
    public ConnectionStrings ConnectionStrings { get; set; } = new();

    /// <summary>
    /// Gets or sets kestrel
    /// </summary>
    /// <returns></returns>
    public Kestrel Kestrel { get; set; } = new();

    /// <summary>
    /// Gets or sets corsOrigin
    /// </summary>
    /// <value></value>
    public string CorsOrigin { get; set; } = "https://*.unitedtractors.com";

    /// <summary>
    /// Gets or sets bot
    /// </summary>
    /// <returns></returns>
    public Bot Bot { get; set; } = new();

    /// <summary>
    /// Gets or sets authorizationServer
    /// </summary>
    /// <returns></returns>
    public AuthorizationServer AuthorizationServer { get; set; } = new();

    /// <summary>
    /// Gets or sets databaseSettings
    /// </summary>
    /// <returns></returns>
    public DatabaseSettings DatabaseSettings { get; set; } = new();

    /// <summary>
    /// Gets or sets dataLifetime
    /// </summary>
    /// <returns></returns>
    public DataLifetime DataLifetime { get; set; } = new();

    /// <summary>
    /// Gets or sets app
    /// </summary>
    /// <returns></returns>
    public App App { get; set; } = new App();

    /// <summary>
    /// Gets or sets redisServer
    /// </summary>
    /// <returns></returns>
    public Redis Redis { get; set; } = new();

    /// <summary>
    /// Gets or sets backgroundJob
    /// </summary>
    public BackgroundJob BackgroundJob { get; set; } = new();
}

/// <summary>
/// Role
/// </summary>
public record Role
{
    /// <summary>
    /// Gets or sets name
    /// </summary>
    /// <value></value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets name
    /// </summary>
    /// <value></value>
    public List<string> Group { get; set; }
}

/// <summary>
/// AuthorizationServer
/// </summary>
public record AuthorizationServer
{
    /// <summary>
    /// Gets or sets gateway
    /// </summary>
    /// <value></value>
    public string Gateway { get; set; } = "https://gateway-dev.unitedtractors.com/dev/internal";

    /// <summary>
    /// Gets or sets address
    /// </summary>
    /// <value></value>
    public string Address { get; set; } = "http://usermanagementservice.dev-rke.unitedtractors.com";

    /// <summary>
    /// Gets or sets whiteListPathSegment
    /// </summary>
    /// <value></value>
    public string WhiteListPathSegment { get; set; } = "/swagger,/health";

    /// <summary>
    /// Gets or sets header
    /// </summary>
    public string Header { get; set; } = "Ocp-Apim-Subscription-Key";

    /// <summary>
    /// Gets or sets secret
    /// </summary>
    /// <value></value>
    public string Secret { get; set; } = "";

    /// <summary>
    /// Gets or sets service
    /// </summary>
    /// <value></value>
    public string Service { get; set; } = "netca";

    /// <summary>
    /// Gets or sets policy
    /// </summary>
    /// <value></value>
    public List<Policy?> Policy { get; set; } = new();

    /// <summary>
    /// Gets or sets role
    /// </summary>
    /// <value></value>
    public List<Role?> Role { get; set; }
}

/// <summary>
/// Policy
/// </summary>
public record Policy
{
    /// <summary>
    /// Gets or sets name
    /// </summary>
    /// <value></value>
    public string Name { get; set; } = "";

    /// <summary>
    /// Gets or sets a value indicating whether isCheck
    /// </summary>
    /// <value></value>
    public bool IsCheck { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether isCache
    /// </summary>
    /// <value></value>
    public bool IsCache { get; set; } = false;
}

/// <summary>
/// DatabaseSettings
/// </summary>
public record DatabaseSettings
{
    /// <summary>
    /// Gets or sets maxRetryDelay
    /// </summary>
    /// <value></value>
    public int MaxRetryDelay { get; set; } = 5;

    /// <summary>
    /// Gets or sets maxRetryCount
    /// </summary>
    /// <value></value>
    public int MaxRetryCount { get; set; } = 100;

    /// <summary>
    /// Gets or sets commandTimeout
    /// </summary>
    /// <value></value>
    public int CommandTimeout { get; set; } = 60;

    /// <summary>
    /// Gets or sets a value indicating whether migrations
    /// </summary>
    /// <value></value>
    public bool Migrations { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether seedData
    /// </summary>
    /// <value></value>
    public bool SeedData { get; set; } = false;
}

/// <summary>
/// DataLifetime
/// </summary>
public record DataLifetime
{
    /// <summary>
    /// Gets or sets changelog
    /// </summary>
    /// <value></value>
    public int Changelog { get; set; } = 180;
}

/// <summary>
/// App
/// </summary>
public record App
{
    /// <summary>
    /// Gets or sets title
    /// </summary>
    /// <value></value>
    public string Title { get; set; } = "netca API";

    /// <summary>
    /// Gets or sets description
    /// </summary>
    /// <value></value>
    public string Description { get; set; } = "This is awesome app";

    /// <summary>
    /// Gets or sets version
    /// </summary>
    /// <value></value>
    public string Version { get; set; } = "1.0.0";

    /// <summary>
    /// Gets or sets appContact
    /// </summary>
    /// <value></value>
    public AppContact AppContact { get; set; } = new();
}

/// <summary>
/// Kestrel
/// </summary>
public record Kestrel
{
    /// <summary>
    /// Gets or sets keepAliveTimeoutInM
    /// </summary>
    /// <value></value>
    public int KeepAliveTimeoutInM { get; set; } = 2;

    /// <summary>
    /// Gets or sets minRequestBodyDataRate
    /// </summary>
    /// <value></value>
    public MinRequestBodyDataRate MinRequestBodyDataRate { get; set; } = new();

    /// <summary>
    /// Gets or sets minResponseDataRate
    /// </summary>
    /// <value></value>
    public MinResponseDataRate MinResponseDataRate { get; set; } = new();
}

/// <summary>
/// MinRequestBodyDataRate
/// </summary>
public record MinRequestBodyDataRate
{
    /// <summary>
    /// Gets or sets bytesPerSecond
    /// </summary>
    /// <value></value>
    public double BytesPerSecond { get; set; } = 100;

    /// <summary>
    /// Gets or sets gracePeriod
    /// </summary>
    /// <value></value>
    public int GracePeriod { get; set; } = 10;
}

/// <summary>
/// MinResponseDataRate
/// </summary>
public record MinResponseDataRate
{
    /// <summary>
    /// Gets or sets bytesPerSecond
    /// </summary>
    /// <value></value>
    public double BytesPerSecond { get; set; } = 100;

    /// <summary>
    /// Gets or sets gracePeriod
    /// </summary>
    /// <value></value>
    public int GracePeriod { get; set; } = 10;
}

/// <summary>
/// AppContact
/// </summary>
public record AppContact
{
    /// <summary>
    /// Gets or sets company
    /// </summary>
    /// <value></value>
    public string Company { get; set; } = "PT United Tractors Tbk";

    /// <summary>
    /// Gets or sets email
    /// </summary>
    /// <value></value>
    public string Email { get; set; } = "cst_dev42@unitedtractors.com";

    /// <summary>
    /// Gets or sets uri
    /// </summary>
    /// <value></value>
    public string Uri { get; set; } = "https://www.unitedtractors.com";
}

/// <summary>
/// Bot
/// </summary>
public record Bot
{
    /// <summary>
    /// Gets or sets a value indicating whether isEnable
    /// </summary>
    /// <value></value>
    public bool IsEnable { get; set; } = false;

    /// <summary>
    /// Gets or sets serviceName
    /// </summary>
    /// <value></value>
    public string ServiceName { get; set; } = "DCA - netca";

    /// <summary>
    /// Gets or sets serviceDomain
    /// </summary>
    /// <value></value>
    public string ServiceDomain { get; set; } = "netca.dev-aks.unitedtractors.com";

    /// <summary>
    /// Gets or sets address
    /// </summary>
    /// <value></value>
    public string Address { get; set; } = "https://gateway-dev.unitedtractors.com/dev/internal/dad/msteams";

    /// <summary>
    /// Gets or sets header
    /// </summary>
    public string Header { get; set; } = "Ocp-Apim-Subscription-Key";

    /// <summary>
    /// Gets or sets secret
    /// </summary>
    /// <value></value>
    public string Secret { get; set; } = "";

    /// <summary>
    /// Gets or sets cacheMsTeam
    /// </summary>
    /// <value></value>
    public CacheMsTeam CacheMsTeam { get; set; } = new();
}

/// <summary>
/// ConnectionStrings
/// </summary>
public record ConnectionStrings
{
    /// <summary>
    /// Gets or sets netcaDb
    /// </summary>
    /// <returns></returns>
    public string DefaultConnection { get; set; } = "netca.db";
}

/// <summary>
/// Messaging
/// </summary>
public record Messaging
{
    /// <summary>
    /// Gets or sets azureEventHub
    /// </summary>
    /// <value></value>
    public List<AzureEventHub> AzureEventHub { get; set; } = new();
}

/// <summary>
/// AzureEventHub
/// </summary>
public record AzureEventHub
{
    /// <summary>
    /// Gets or sets name
    /// </summary>
    /// <value></value>
    public string Name { get; set; } = "dca";

    /// <summary>
    /// Gets or sets connectionString
    /// </summary>
    /// <value></value>
    public string ConnectionString { get; set; } = "DCA_Tracking_Service";

    /// <summary>
    /// Gets or sets storageAccount
    /// </summary>
    /// <value></value>
    public string StorageAccount { get; set; } = "DCA_Tracking_Service";

    /// <summary>
    /// Gets or sets blobContainerName
    /// </summary>
    /// <value></value>
    public string BlobContainerName { get; set; } = "DCA_Tracking_Service";

    /// <summary>
    /// Gets or sets topic
    /// </summary>
    /// <value></value>
    public List<EventHubTopic> Topics { get; set; } = new();
}

/// <summary>
/// EventHubTopic
/// </summary>
public record EventHubTopic
{
    /// <summary>
    /// Gets or sets name
    /// </summary>
    /// <value></value>
    public string Name { get; set; } = "dca_assignmentservice_assignments_v1";

    /// <summary>
    /// Gets or sets groupName
    /// </summary>
    /// <value></value>
    public string GroupName { get; set; } = "$Default";

    /// <summary>
    /// Gets or sets value
    /// </summary>
    /// <value></value>
    public string Value { get; set; } = "dca_assignmentservice_assignments_v1";
}

/// <summary>
/// Topic
/// </summary>
public record Topic
{
    /// <summary>
    /// Gets or sets topic for Consume EquipmentMonitoring from UT Portal
    /// </summary>
    /// <value></value>
    public string EquipmentMonitoring { get; set; } = "utportal_equipmentmonitoringservice_equipment_v1";
}

/// <summary>
/// Redis
/// </summary>
public record Redis
{
    /// <summary>
    /// Gets or sets server
    /// </summary>
    /// <value></value>
    public string? Server { get; set; } = "";

    /// <summary>
    /// Gets or sets instanceName
    /// </summary>
    /// <value></value>
    public string InstanceName { get; set; } = "redis";

    /// <summary>
    /// Gets or sets databaseNumber
    /// </summary>
    public int DatabaseNumber { get; set; } = 0;

    /// <summary>
    /// Gets or sets requestExpiryInMinutes
    /// </summary>
    /// <value></value>
    public int RequestExpiryInMinutes { get; set; } = 30;

    /// <summary>
    /// Gets or sets messageExpiryInDays
    /// </summary>
    /// <value></value>
    public int MessageExpiryInDays { get; set; } = 180;

    /// <summary>
    /// Gets or sets policy
    /// </summary>
    /// <value></value>
    public List<Policy> Policy { get; set; } = new();
}

/// <summary>
/// BackgroundJob
/// </summary>
public record BackgroundJob
{
    /// <summary>
    /// Gets or sets a value indicating whether isEnable
    /// </summary>
    public bool IsEnable { get; set; } = true;
    
    /// <summary>
    /// HostName
    /// </summary>
    public string HostName { get; set; } = "local";
    
    /// <summary>
    /// HostName
    /// </summary>
    public int DefaultMaxRunTime { get; set; } = 10;

    /// <summary>
    /// Gets or sets a value indicating whether usePersistentStore
    /// </summary>
    public bool UsePersistentStore { get; set; } = false;

    /// <summary>
    /// Gets or sets persistentStore
    /// </summary>
    public PersistentStore PersistentStore { get; set; } = new();

    /// <summary>
    /// Gets or sets jobs
    /// </summary>
    public List<Job> Jobs { get; set; } = new();
}

/// <summary>
/// PersistentStore
/// </summary>
public record PersistentStore
{
    /// <summary>
    /// Gets or sets connectionString
    /// </summary>
    public string ConnectionString { get; set; } = "";
    
    /// <summary>
    /// Gets or sets a value indicating whether ignoreDuplicates
    /// </summary>
    public bool UseCluster { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether ignoreDuplicates
    /// </summary>
    public bool IgnoreDuplicates { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether overWriteExistingData
    /// </summary>
    public bool OverWriteExistingData { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether ScheduleTriggerRelativeToReplacedTrigger
    /// </summary>
    public bool ScheduleTriggerRelativeToReplacedTrigger { get; set; } = true;

    /// <summary>
    /// Gets or sets maxConcurrency
    /// </summary>
    public int MaxConcurrency { get; set; } = 10;

    /// <summary>
    /// Gets or sets retryInterval
    /// </summary>
    public int RetryInterval { get; set; } = 15;

    /// <summary>
    /// Gets or sets checkinInterval
    /// </summary>
    public int CheckinInterval { get; set; } = 15000;

    /// <summary>
    /// Gets or sets checkinMisfireThreshold
    /// </summary>
    public int CheckinMisfireThreshold { get; set; } = 15000;

    /// <summary>
    /// Gets or sets misfireThreshold
    /// </summary>
    public int MisfireThreshold { get; set; } = 15000;

    /// <summary>
    /// Gets or sets tablePrefix
    /// </summary>
    public string TablePrefix { get; set; } = "QRTZ_";
}

/// <summary>
/// Job
/// </summary>
public record Job
{
    /// <summary>
    /// Gets or sets name
    /// </summary>
    public string Name { get; set; } = "";
    
    /// <summary>
    /// Gets or sets a value indicating whether isParallel
    /// </summary>
    public bool IsParallel { get; set; }
    
    /// <summary>
    /// IgnoreMisfire
    /// </summary>
    public bool IgnoreMisfire { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether isEnable
    /// </summary>
    public bool IsEnable { get; set; } = false;

    /// <summary>
    /// Gets or sets schedule
    /// </summary>
    public string Schedule { get; set; } = "";

    /// <summary>
    /// Gets or sets description
    /// </summary>
    public string Description { get; set; } = "";

    /// <summary>
    /// Gets or sets parameters
    /// </summary>
    public List<object> Parameters { get; set; } = new();
}