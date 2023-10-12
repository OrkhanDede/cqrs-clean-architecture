using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.Models;
using Domain.Common.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> IncludeAll<T>(this IQueryable<T> queryable, params Expression<Func<T, object>>[] includeProperties) where T : class, IEntity, new()
        {
            foreach (var includeProperty in includeProperties)
                queryable = queryable.Include(includeProperty);
            return queryable;
        }
        public static IQueryable<T> IncludeAll<T>(this IQueryable<T> queryable, params string[] includeProperties) where T : class, IEntity, new()
        {
            foreach (var includeProperty in includeProperties)
                queryable = queryable.Include(includeProperty);
            return queryable;
        }
        public static IQueryable<T> FindPaged<T>(this IQueryable<T> query, PagingParameters filter)
        {
            if (filter != null && !filter.IsAll)
                return query.Skip((filter.Page - 1) * filter.Limit).Take(filter.Limit);
            return query;
        }
        public static IEnumerable<T> FindPaged<T>(this IEnumerable<T> query, PagingParameters filter)
        {
            if (filter != null && !filter.IsAll)
                return query.Skip((filter.Page - 1) * filter.Limit).Take(filter.Limit);
            return query;
        }

        public static IQueryable<TEntity> ApplyQueryFilter<TEntity>(this IQueryable<TEntity> query, bool isFilterApplied = true) where TEntity : class, IEntity, new()
        {

            if (!isFilterApplied)
                return query.IgnoreQueryFilters();

            return query;
        }
        public static IQueryable<TEntity> ApplyAsNoTracking<TEntity>(this IQueryable<TEntity> query, bool isApplied = true) where TEntity : class, IEntity, new()
        {

            if (isApplied)
                return query.AsNoTracking();

            return query;
        }
    }
}
