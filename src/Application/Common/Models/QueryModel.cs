// ------------------------------------------------------------------------------------
// QueryModel.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace netca.Application.Common.Models
{
    /// <summary>
    /// Model for receive sort, filter and pagging
    /// </summary>
    public class QueryModel
    {
        private static readonly ILogger Logger = AppLoggingExtensions.CreateLogger("QueryModel");

        /// <summary>
        /// Gets or sets filters
        /// </summary>
        public string? Filters { get; set; }

        /// <summary>
        /// Gets or sets sorts
        /// </summary>
        public string? Sorts { get; set; }

        /// <summary>
        /// Gets or sets number of req page
        /// </summary>
        /// <value>number of req page</value>
        /// <example>1</example>
        public int? PageNumber { get; set; } = Constants.DefaultPageNumber;

        /// <summary>
        /// Gets or sets limit data each page
        /// </summary>
        /// <value>limit data each page</value>
        /// <example>10</example>
        public int? PageSize { get; set; } = Constants.DefaultPageSize;

        /// <summary>
        /// Parsing Filters to list FilterQuery model
        /// </summary>
        /// <returns></returns>
        public List<FilterQuery> GetFiltersParsed()
        {
            if (Filters == null)
                return new List<FilterQuery>();

            var value = new List<FilterQuery>();

            foreach (var filter in Regex.Split(Filters, Constants.EscapedCommaPattern))
            {
                if (string.IsNullOrWhiteSpace(filter))
                    continue;

                if (filter.StartsWith("("))
                    CheckStartWith(filter, value);
                else
                    NotCheckStartWith(filter, value);
            }

            return value;
        }

        private static void CheckStartWith(string filter, ICollection<FilterQuery> value)
        {
            var filterOpAndVal = filter[(filter.LastIndexOf(")", StringComparison.Ordinal) + 1)..];
            var sub = filter.Replace(filterOpAndVal, "").Replace("(", "").Replace(")", "");

            var subFilters = Regex.Split(sub, Constants.EscapedPipePattern);

            for (var i = 0; i < subFilters.Length; i++)
            {
                var filterSplit = filterOpAndVal
                    .Split(Constants.Operators, StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => t.Trim()).ToArray();

                string logic;

                if (i == 0)
                    logic = "(AND";
                else if (i == subFilters.Length - 1)
                    logic = "OR)";
                else
                    logic = "OR";

                value.Add(new FilterQuery
                {
                    Field = subFilters[i],
                    Operator = filterSplit[0],
                    Value = filterSplit[1],
                    Logic = logic
                });

                Logger.LogDebug($"Filter = {subFilters[i]} {filterSplit[0]} {filterSplit[1]}");
            }
        }

        private static void NotCheckStartWith(string filter, ICollection<FilterQuery> value)
        {
            var filterSplit = filter
                .Split(Constants.Operators, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .ToArray();

            if (filterSplit[1].StartsWith("("))
            {
                var subFilters = Regex
                    .Split(
                        filterSplit[1][1..filterSplit[1].IndexOf(")", StringComparison.Ordinal)],
                        Constants.EscapedPipePattern
                    );

                for (var i = 0; i < subFilters.Length; i++)
                {
                    string logic;

                    if (i == 0)
                        logic = "(AND";
                    else if (i == subFilters.Length - 1)
                        logic = "OR)";
                    else
                        logic = "OR";

                    value.Add(new FilterQuery
                    {
                        Field = filterSplit[0],
                        Operator = Array.Find(Constants.Operators, filter.Contains) ?? "==",
                        Value = subFilters[i],
                        Logic = logic
                    });

                    Logger.LogDebug($"Filter = {filterSplit[0]} {Array.Find(Constants.Operators, filter.Contains) ?? "=="} {subFilters[i]}");
                }
            }
            else
            {
                value.Add(new FilterQuery
                {
                    Field = filterSplit[0],
                    Operator = Array.Find(Constants.Operators, filter.Contains) ?? "==",
                    Value = filterSplit[1],
                    Logic = "AND"
                });

                Logger.LogDebug($"Filter = {filterSplit[0]} {Array.Find(Constants.Operators, filter.Contains) ?? "=="} {filterSplit[1]}");
            }
        }

        /// <summary>
        /// Parsing Sorts to Sort model
        /// </summary>
        /// <returns></returns>
        public List<Sort> GetSortsParsed()
        {
            if (Sorts == null)
                return new List<Sort>();

            var value = new List<Sort>();

            foreach (var sort in Regex.Split(Sorts, Constants.EscapedCommaPattern))
            {
                if (string.IsNullOrWhiteSpace(sort))
                    continue;

                if (sort[..1] == "-")
                {
                    value.Add(new Sort
                    {
                        Field = sort[1..],
                        Direction = "DESC"
                    });
                }
                else
                {
                    value.Add(new Sort
                    {
                        Field = sort,
                        Direction = "ASC"
                    });
                }
            }

            return value;
        }
    }

    /// <summary>
    /// Sort
    /// </summary>
    public class Sort
    {
        /// <summary>
        /// Gets or sets field
        /// </summary>
        /// <value></value>
        public string? Field { get; set; }

        /// <summary>
        /// Gets or sets direction
        /// </summary>
        /// <value></value>
        public string? Direction { get; set; }
    }

    /// <summary>
    /// FilterQuery
    /// </summary>
    public class FilterQuery
    {
        /// <summary>
        /// Gets or sets field
        /// </summary>
        /// <value>Field name to filter</value>
        /// <example>CreatedBy</example>
        public string? Field { get; set; }

        /// <summary>
        /// Gets or sets operator eq,neq,lt,lte,gt,gte,startswith,endswith,contains,doesnotcontain
        /// </summary>
        /// <value>logical operator</value>
        /// <example>eq</example>
        public string? Operator { get; set; }

        /// <summary>
        /// Gets or sets value
        /// </summary>
        /// <value>value to search</value>
        /// <example>xx</example>
        public object? Value { get; set; }

        /// <summary>
        /// Gets or sets logic AND OR
        /// </summary>
        /// <value>logical operator</value>
        /// <example>AND</example>
        public string? Logic { get; set; }
    }
}