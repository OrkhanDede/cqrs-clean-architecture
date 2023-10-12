using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Core.Models;

namespace Core.Extensions
{
    public static class EntitySortingExtensions
    {
   

        private static string ToOrderString(this List<SortParameters> sp)
        {
            return String.Join(",", sp.Select(c => c.ToOrderString()).ToArray()); ;
        }
        private static string ToOrderString(this SortParameters sp)
        {
            return $"{sp.Prop} {sp.Dir}";
        }
        public static IQueryable<T> SortBy<T>(this IQueryable<T> query, SortParameters sortParameters)
        {

            List<SortParameters> sortParametersList = new List<SortParameters>();
            if (sortParameters != null && !string.IsNullOrEmpty(sortParameters.Prop) && !string.IsNullOrEmpty(sortParameters.Dir))
            {
                if (sortParameters.Prop != "Id")
                    sortParametersList.Add(sortParameters);
            }

            sortParametersList.Add(new SortParameters()
            {
                Prop = "Id",
                Dir = "asc"
            });

            var orderString = sortParametersList.ToOrderString();
            query = query.OrderBy(orderString);
            return query;

        }
        public static IQueryable<T> SortBy<T>(this IQueryable<T> query, List<SortParameters> sortParameterList)
        {
            if (sortParameterList == null || !sortParameterList.Any())
            {
                sortParameterList ??= new List<SortParameters>();
                sortParameterList.Add(new SortParameters()
                {
                    Prop = "Id",
                    Dir = "desc"
                });
            }
            query = query.OrderBy(sortParameterList.ToOrderString());
            return query;
        }      
        public static IEnumerable<T> SortBy<T>(this IEnumerable<T> query, SortParameters sortParameters)
        {

            List<SortParameters> sortParametersList = new List<SortParameters>();
            if (sortParameters != null && !string.IsNullOrEmpty(sortParameters.Prop) && !string.IsNullOrEmpty(sortParameters.Dir))
            {
                if (sortParameters.Prop != "Id")
                    sortParametersList.Add(sortParameters);
            }

            sortParametersList.Add(new SortParameters()
            {
                Prop = "Id",
                Dir = "asc"
            });

            var orderString = sortParametersList.ToOrderString();
            query = query.AsQueryable().OrderBy(orderString);
            return query;

        }
        public static IEnumerable<T> SortBy<T>(this IEnumerable<T> query, List<SortParameters> sortParameterList)
        {
            if (sortParameterList == null || !sortParameterList.Any())
            {
                sortParameterList ??= new List<SortParameters>();
                sortParameterList.Add(new SortParameters()
                {
                    Prop = "Id",
                    Dir = "desc"
                });
            }
            query = query.AsQueryable().OrderBy(sortParameterList.ToOrderString());
            return query;
        }



    }
}
