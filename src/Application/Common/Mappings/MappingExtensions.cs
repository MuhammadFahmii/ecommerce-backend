// ------------------------------------------------------------------------------------
// MappingExtensions.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JsonApiSerializer.JsonApi;
using Microsoft.EntityFrameworkCore;

namespace netca.Application.Common.Mappings
{
    /// <summary>
    /// MappingExtensions
    /// </summary>
    public static class MappingExtensions
    {
        /// <summary>
        /// PaginatedListAsync
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="totalItems"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        public static Task<DocumentRootJson<List<TDestination>>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
            => JsonApiExtensionPaginated.CreateAsync(queryable, pageNumber, pageSize);
        
        /// <summary>
        /// ProjectToListAsync
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="configuration"></param>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration)
            => queryable.ProjectTo<TDestination>(configuration).ToListAsync();
    }
}
