// ------------------------------------------------------------------------------------
// RedisService.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Models;
using netca.Application.Dtos;
using StackExchange.Redis;

namespace netca.Infrastructure.Services.Cache;

/// <summary>
/// RedisService
/// </summary>
public class RedisService : BaseService, IRedisService
{
    private readonly ILogger<RedisService> _logger;
    private readonly AppSetting _appSetting;

    /// <summary>
    /// Initializes a new instance of the <see cref="RedisService"/> class.
    /// RedisService
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="appSetting"></param>
    public RedisService(ILogger<RedisService> logger, AppSetting appSetting)
    {
        _logger = logger;
        _appSetting = appSetting;
        ConnectionString = _appSetting.Redis.Server;
    }

    private static string? ConnectionString { get; set; }

    private readonly Lazy<ConnectionMultiplexer> _lazyConnection =
        new(() => ConnectionMultiplexer.Connect(ConnectionString!));

    /// <summary>
    /// Gets connections
    /// </summary>
    /// <value></value>
    private ConnectionMultiplexer Connections => _lazyConnection.Value;

    private IDatabase Database => Connections.GetDatabase(_appSetting.Redis.DatabaseNumber);

    private IEnumerable<IServer> Servers
    {
        get
        {
            var endpoints = Connections.GetEndPoints();
            var servers = endpoints.Select(endpoint => Connections.GetServer(endpoint)).ToList();
            return servers;
        }
    }

    /// <summary>
    /// ListLeftPushAsync
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public async Task<long> ListLeftPushAsync(string key, string value)
    {
        key += _appSetting.Redis.InstanceName;
        return await Database.ListLeftPushAsync(key, value);
    }

    /// <summary>
    /// ListLeftPopAsync
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<string> ListLeftPopAsync(string key)
    {
        key += _appSetting.Redis.InstanceName;
        return await Database.ListLeftPopAsync(key);
    }

    /// <summary>
    /// DeleteAsync
    /// </summary>
    /// <param name="key"></param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task DeleteAsync(string key)
    {
        if (!string.IsNullOrWhiteSpace(key))
        {
            await Database.KeyDeleteAsync(key);
            _logger.LogDebug("Delete Redis with key {K}", key);
        }
    }

    /// <summary>
    /// GetAsync
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<string> GetAsync(string key)
    {
        _logger.LogDebug("Process Redis key : {K}", key);
        return await Database.StringGetAsync(key)!;
    }

    /// <summary>
    /// GetAllValueWithKeyAsync
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<IEnumerable<RedisDto>> GetAllValueWithKeyAsync(string key)
    {
        var data = new List<RedisDto>();
        try
        {
            _logger.LogTrace("Process Get all data from Redis");

            foreach (var k in Servers.SelectMany(redisServer => redisServer.Keys(pattern: key)))
            {
                var value = await this.GetAsync(k);
                data.Add(new RedisDto
                {
                    Key = k,
                    Value = value
                });
            }

            _logger.LogTrace("Success Get all data from Redis");
            return data.OrderBy(o => o.Key);
        }
        catch (Exception e)
        {
            _logger.LogError("Error Get all from Redis {M}", e.Message);
            return data;
        }
    }

    /// <summary>
    /// SaveAsync
    /// </summary>
    /// <param name="key"></param>
    /// <param name="sub"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public async Task<string> SaveAsync(string key, string sub, string value)
    {
        var resultKey = "";
        try
        {
            TimeSpan? expiry = null;
            if (sub.ToLower().Equals(Constants.RedisSubKeyHttpRequest.ToLower()))
            {
                expiry = TimeSpan.FromMinutes(_appSetting.Redis.RequestExpiryInMinutes);
            }

            if (sub.ToLower().Equals(Constants.RedisSubKeyMessageConsume.ToLower()) ||
                sub.ToLower().Equals(Constants.RedisSubKeyMessageProduce.ToLower()))
            {
                expiry = TimeSpan.FromDays(_appSetting.Redis.MessageExpiryInDays);
            }

            sub += _appSetting.Redis.InstanceName;
            resultKey = GenerateKey(key, sub);
            await Database.StringSetAsync(resultKey, value, expiry);
            _logger.LogDebug("Save to Redis with key {Key}", resultKey);
        }
        catch
        {
            _logger.LogError("Failed Save to Redis, key: {Key}", resultKey);
        }

        return resultKey;
    }
}