using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using YouBikeAPI.Models;

namespace YouBikeAPI.Utilities
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> DateBetween<T>(this IQueryable<T> source, int start, int end)
            where T : IDate
        {
            if (!(source?.Any() ?? false))
            {
                throw new InvalidOperationException("error");
            }

            var result = source.Where(s => s.CreationDate.Month <= DateTime.UtcNow.AddMonths(end).Month
                && s.CreationDate.Month > DateTime.UtcNow.AddMonths(start).Month);

            return result;
        }

        public static IQueryable<T> CustomSearch<T>(this IQueryable<T> source, string query, Dictionary<string, Object> properties)
        {
            if (!(source?.Any() ?? false))
            {
                throw new InvalidOperationException("error");
            }

            IQueryable<T> result = source;

            var config = new ParsingConfig
            {
                IsCaseSensitive = false
            };

            string predicate = string.Empty;
            BikeType type = BikeType.Electric;

            foreach (var p in properties)
            {
                //Console.WriteLine(p.Key + " " + p.Value.ToString());

                if (p.Value.ToString() == typeof(bool).ToString())
                {
                    predicate += (predicate == string.Empty) ? $"{p.Key}==(@1)"
                        : $"|| {p.Key}==(@1)";
                }
                else if (p.Value.ToString() == typeof(string).ToString())
                {
                    predicate += (predicate == string.Empty) ? $"{p.Key}.Contains(@0)"
                        : $"|| {p.Key}.Contains(@0)";
                }
                else if (p.Value.ToString() == typeof(System.Enum).ToString())
                {
                    if (Enum.IsDefined(typeof(BikeType), query))
                    {
                        type = (BikeType)Enum.Parse(typeof(BikeType), query, true);
                        predicate += (predicate == string.Empty) ? $"BikeType==(@2)"
                        : $"|| BikeType==(@2)";
                    }
                }
                else
                {
                    predicate += (predicate == string.Empty) ? $"{p.Key}.toString().Contains(@0)"
                        : $"|| {p.Key}.toString().Contains(@0)";
                }
            }

            bool modiflyQuery = false;
            if (query == "true" || query == "false")
            {
                bool.TryParse(query, out modiflyQuery);
            }

            Object[] paramss = { query, modiflyQuery, type };

            result = result.Where(config, predicate, paramss);

            return result;
        }
    }
}