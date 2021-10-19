// ------------------------------------------------------------------------------------
// Queryable.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace netca.Application.Common.Models
{
    /// <summary>
    /// Queryable
    /// </summary>
    public static class Queryable
    {
        private static readonly ILogger Logger = AppLoggingExtensions.CreateLogger("Queryable");
        private static Type _type;
        /// <summary>
        /// Query
        /// </summary>
        /// <param name="source"></param>
        /// <param name="queryModel"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> Query<T>(this IQueryable<T> source, QueryModel queryModel)
        {

            if (source == null)
                throw new ArgumentNullException(nameof(source));

            _type = typeof(T);

            source = Filter(source, queryModel.GetFiltersParsed() ?? new List<FilterQuery>());

            source = Sort(source, queryModel.GetSortsParsed());

            if (queryModel.PageNumber < 1) { queryModel.PageNumber = Constants.DefaultPageNumber; }
            if (queryModel.PageSize < 1) { queryModel.PageSize = Constants.DefaultPageSize; }

            source = Limit(source, queryModel.PageNumber ?? Constants.DefaultPageNumber, queryModel.PageSize ?? Constants.DefaultPageSize);

            return source;
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="source"></param>
        /// <param name="queryModel"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> QueryWithoutLimit<T>(this IQueryable<T> source, QueryModel queryModel)
        {

            if (source == null)
                throw new ArgumentNullException(nameof(source));

            _type = typeof(T);

            source = Filter(source, queryModel.GetFiltersParsed() ?? new List<FilterQuery>());

            source = Sort(source, queryModel.GetSortsParsed());

            return source;
        }

        private static IQueryable<T> Filter<T>(IQueryable<T> source, IList<FilterQuery> filter)
        {
            try
            {
                if (filter != null && filter.Any())
                {
                    var where = SwitchLogic(filter);
                    if (!string.IsNullOrEmpty(where))
                    {
                        var values = filter.Select(f => f.Value).ToArray();
                        Logger.LogDebug($"Filter {_type} with {where} {values}");
                        source = source.Where(where, values);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogWarning($"Failed to filter {e.Message}");
            }
            return source;
        }

        private static string SwitchLogic(IList<FilterQuery> filter)
        {
            string where = null;
            for (var i = 0; i < filter.Count; i++)
            {
                var logic = filter[i].Logic ?? "AND";
                string f;
                if (logic.StartsWith("("))
                {
                    f = i == 0 ? $"({Transform(logic[1..], filter[i], i)}" : $"{Transform(logic[1..] + " (", filter[i], i)}";
                }
                else if (logic.EndsWith(")"))
                {
                    f = $"{Transform(logic[..^1], filter[i], i)})";
                }
                else
                {
                    f = Transform(logic, filter[i], i);
                }
                if (f != null)
                {
                    where = $"{where} {f}";
                }
            }

            return where;
        }

        private static IQueryable<T> Limit<T>(IQueryable<T> source, int pageNumber, int pageSize)
        {
            Logger.LogDebug($"Try to skip {pageSize * (pageNumber - 1)} and take {pageSize}");
            return source.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        }

        private static readonly IDictionary<string, string>
            Operators = new Dictionary<string, string> {
                { "eq", "=" },
                { "neq", "!=" },
                { "lt", "<" },
                { "lte", "<=" },
                { "gt", ">" },
                { "gte", ">=" },
                { "startswith", "StartsWith" },
                { "endswith", "EndsWith" },
                { "contains", "Contains" },
                { "doesnotcontain", "Contains" },
                { "==", "=" },
                { "!=", "!=" },
                { "<", "<" },
                { "<=", "<=" },
                { ">", ">" },
                { ">=", ">=" },
                { "_=", "StartsWith" },
                { "=_", "EndsWith" },
                { "@=", "Contains" },
                { "!@=", "Contains" }
            };

        /// <summary>
        /// Transform
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="filter"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static string Transform(string logic, FilterQuery filter, int index)
        {
            if (filter.Value == null || string.IsNullOrEmpty(filter.Field) ||
                string.IsNullOrEmpty(filter.Value.ToString())) return null;
            try
            {
                if (filter.Operator != null)
                {
                    return TransformLogic(Operators[filter.Operator.ToLower()], logic, filter, index);
                }
            }
            catch (Exception e)
            {
                Logger.LogWarning($"Operator {filter.Operator} not part of the Dictionary {e.Message}");
            }
            return null;
        }

        private static string TransformLogic(string comparison, string logic, FilterQuery filter, int index)
        {
            if (filter.Operator == "doesnotcontain")
            {
                if (index > 0)
                {
                    return string.Format("{0} ({1} != null && !{1}.ToString().{2}(@{3}))", logic,
                        filter.Field, comparison, index);
                }
                return string.Format("({0} != null && !{0}.ToString().{1}(@{2}))",
                    filter.Field, comparison, index);
            }
            if (comparison != "StartsWith" && comparison != "EndsWith" && comparison != "Contains")
                return index > 0
                    ? $" {logic} {filter.Field} {comparison} @{index}"
                    : $"{filter.Field} {comparison} @{index}";
            if (index > 0)
            {
                return string.Format("{0} ({1} != null && {1}.ToString().{2}(@{3}))", logic,
                    filter.Field, comparison, index);
            }
            return string.Format("({0} != null && {0}.{1}(@{2}))",
                filter.Field, comparison, index);   
        }

        /// <summary>
        /// Sorting query by column
        /// </summary>
        /// <param name="source"></param>
        /// <param name="sort"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static IQueryable<T> Sort<T>(this IQueryable<T> source, IReadOnlyCollection<Sort> sort)
        {
            if (sort == null || !sort.Any()) return source;
            try
            {
                var ordering = string.Join(",", sort.Select(s => $"{s.Field} {s.Direction}"));
                Logger.LogDebug($"Try to sort {_type} with {ordering}");
                return source.OrderBy(ordering);
            }
            catch (ParseException e)
            {
                Logger.LogWarning($"sortBy include field not part of the {_type} {e.Message}");
            }

            return source;
        }
    }
}
