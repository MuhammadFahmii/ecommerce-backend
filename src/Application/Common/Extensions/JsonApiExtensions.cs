// ------------------------------------------------------------------------------------
// JsonApiExtensions.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using JsonApiSerializer;
using JsonApiSerializer.JsonApi;
using JsonApiSerializer.JsonApi.WellKnown;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace netca.Application.Common.Extensions;

/// <summary>
/// JsonApiExtensionPaginated
/// </summary>
public static class JsonApiExtensionPaginated
{
    /// <summary>
    /// CreateAsync
    /// </summary>
    /// <param name="source"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<DocumentRootJson<List<T>>> CreateAsync<T>(
        IQueryable<T> source, int pageNumber, int pageSize)
    {
        return await JsonApiExtensions.ToJsonApiProjectTo(source, pageNumber, pageSize);
    }
}

/// <summary>
/// JsonApiExtensions
/// </summary>
public static class JsonApiExtensions
{
    /// <summary>
    /// ToJsonApiProjectTo
    /// </summary>
    /// <param name="data"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<DocumentRootJson<List<T>>> ToJsonApiProjectTo<T>(IQueryable<T> data, int pageNumber = 1,
        int pageSize = 1)
    {
        var totalItems = await data.CountAsync();
        var items = await data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        var totalPages = totalItems > 0 ? (int)Math.Ceiling(totalItems / (double)pageSize) : 0;
        var hasPreviousPage = pageNumber > 1;
        var hasNextPage = pageNumber < totalPages;
        var nextPageNumber = hasNextPage ? pageNumber + 1 : totalPages;
        var previousPageNumber = hasPreviousPage ? pageNumber - 1 : 1;
        var meta = new Meta
        {
            { "totalItems", totalItems },
            { "pageNumber", pageNumber },
            { "pageSize", pageSize },
            { "totalPages", totalPages },
            { "hasPreviousPage", hasPreviousPage },
            { "hasNextPage", hasNextPage },
            { "nextPageNumber", nextPageNumber },
            { "previousPageNumber", previousPageNumber },
        };
        return new DocumentRootJson<List<T>>
        {
            Data = items,
            Meta = meta,
            Status = new Status(),
        };
    }

    /// <summary>
    /// ToJsonApiPaginated.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="meta"></param>
    /// <param name="totalItems"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public static DocumentRootJson<T> ToJsonApiPaginated<T>(T data, Meta meta, int totalItems = 1, int pageNumber = 1,
        int pageSize = 1)
    {
        var totalPages = totalItems > 0 ? (int)Math.Ceiling(totalItems / (double)pageSize) : 0;
        var hasPreviousPage = pageNumber > 1;
        var hasNextPage = pageNumber < totalPages;
        var nextPageNumber = hasNextPage ? pageNumber + 1 : totalPages;
        var previousPageNumber = hasPreviousPage ? pageNumber - 1 : 1;
        meta.Add(key:"totalItems", value:totalItems);
        meta.Add(key:"pageNumber", value:pageNumber);
        meta.Add(key:"pageSize", value:pageSize);
        meta.Add(key:"totalItems", value:totalPages);
        meta.Add(key:"totalPages", value:totalItems);
        meta.Add(key:"hasPreviousPage", value:hasPreviousPage);
        meta.Add(key:"hasNextPage", value:hasNextPage);
        meta.Add(key:"nextPageNumber", value:nextPageNumber);
        meta.Add(key:"previousPageNumber", value:previousPageNumber);
        
        return new DocumentRootJson<T>
        {
            Data = data,
            Meta = meta,
            Status = new Status(),
        };
    }

    /// <summary>
    /// ToJsonApi
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static DocumentRootJson<T> ToJsonApi<T>(T data)
    {
        return new DocumentRootJson<T>
        {
            Data = data,
            Meta = new Meta(),
            Status = new Status(),
        };
    }

    /// <summary>
    /// ToJsonApi
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public static DocumentRootJson<T> ToJsonApi<T>(T data, Status status)
    {
        return new DocumentRootJson<T>
        {
            Data = data,
            Meta = new Meta(),
            Status = status,
        };
    }

    /// <summary>
    /// SyncSerializerSettings
    /// </summary>
    /// <returns></returns>
    public static JsonApiSerializerSettings SyncSerializerSettings()
    {
        return new JsonApiSerializerSettings
        {
            Formatting = Formatting.Indented,
        };
    }

    /// <summary>
    /// SerializerSettings
    /// </summary>
    /// <returns></returns>
    public static JsonApiSerializerSettings SerializerSettings()
    {
        return new JsonApiSerializerSettings();
    }
}

/// <summary>
/// DocumentRootJson.
/// </summary>
/// <typeparam name="TData"></typeparam>
public class DocumentRootJson<TData> : IDocumentRoot<TData>
{
    /// <summary>
    /// Gets or sets data.
    /// </summary>
    /// <value>
    /// Data.
    /// </value>
    [JsonProperty(Order = 2)]
    public TData Data { get; set; } = default!;

    /// <summary>
    /// Gets or sets included
    /// </summary>
    /// <value>
    /// Included
    /// </value>
    [JsonProperty(Order = 3)]
    public List<JObject>? Included { get; set; }

    /// <summary>
    /// Gets or sets meta
    /// </summary>
    /// <value></value>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore, Order = 1)]
    public Meta Meta { get; set; } = new();

    /// <summary>
    /// Gets or sets responseTime in ms.
    /// </summary>
    /// <value>
    /// ResponseTime in ms.
    /// </value>
    [JsonProperty(Order = 1000)]
    public long ResponseTime { get; set; }

    /// <summary>
    /// Gets or sets status.
    /// </summary>
    /// <value>
    /// Status.
    /// </value>
    [JsonProperty(Order = 10000)]
    public Status? Status { get; set; }
}

/// <summary>
/// Status.
/// </summary>
public class Status
{
    /// <summary>
    /// Gets or sets code.
    /// </summary>
    /// <value>
    /// Code.
    /// </value>
    public int Code { get; set; }

    /// <summary>
    /// Gets or sets desc.
    /// </summary>
    /// <value>
    /// Desc.
    /// </value>
    public object? Desc { get; set; }
}