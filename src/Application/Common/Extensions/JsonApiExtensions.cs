// ------------------------------------------------------------------------------------
// JsonApiExtensions.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonApiSerializer.JsonApi.WellKnown;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace JsonApiSerializer.JsonApi
{
    /// <summary>
    /// JsonApiExtensionPaginated
    /// </summary>
    public static class JsonApiExtensionPaginated
    { 
        /// <summary>
        /// CreateAsync
        /// </summary>
        /// <param name="source"></param>
        /// <param name="totalItems"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<DocumentRootJson<List<T>>> CreateAsync<T>(IQueryable<T> source, int totalItems, int pageNumber,
            int pageSize)
        {

            return await JsonApiExtensions.ToJsonApiProjectTo(source, totalItems, pageNumber, pageSize);
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
        /// <param name="totalItems"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<DocumentRootJson<List<T>>> ToJsonApiProjectTo<T>(IQueryable<T> data, int totalItems, int pageNumber = 1, int pageSize = 1)
        {
             totalItems = totalItems <= 0 ? await data.CountAsync() : totalItems;
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
                Status = new Status()
            };
        }

        /// <summary>
        /// ConvertToJsonApi
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DocumentRootJson<T> ToJsonApi<T>(T data)
        {
            return new DocumentRootJson<T>
            {
                Data = data,
                Meta = new Meta(),
                Status = new Status()
            };
        }

        /// <summary>
        /// ConvertToJsonApiResult
        /// </summary>
        /// <param name="data"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static DocumentRootJson<T> ToJsonApi<T>(T data, Status status)
        {
            return new DocumentRootJson<T>
            {
                Data = data,
                Meta = new Meta(),
                Status = status
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
                Formatting = Formatting.Indented
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
    /// DocumentRootJson
    /// </summary>
    public class DocumentRootJson<TData> : IDocumentRoot<TData>
    {
        /// <summary>
        /// Data
        /// </summary>
        /// <value></value>
        [JsonProperty(Order = 2)]
        public TData Data { get; set; }

        /// <summary>
        /// Included
        /// </summary>
        /// <value></value>
        [JsonProperty(Order = 3)]
        public List<JObject> Included { get; set; }

        /// <summary>
        /// Meta
        /// </summary>
        /// <value></value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, Order = 1)]
        public Meta Meta { get; set; } = new();

        /// <summary>
        /// ResponseTime in ms
        /// </summary>
        /// <value></value>
        [JsonProperty(Order = 1000)]
        public long ResponseTime { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        /// <value></value>
        [JsonProperty(Order = 10000)]
        public Status Status { get; set; }
    }

    /// <summary>
    /// Status
    /// </summary>
    public class Status
    {
        /// <summary>
        /// Code
        /// </summary>
        /// <value></value>
        public int Code { get; set; }

        /// <summary>
        /// Desc
        /// </summary>
        /// <value></value>
        public object Desc { get; set; }
    }
}