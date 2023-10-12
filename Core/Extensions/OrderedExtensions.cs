﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Extensions
{


    public static class OrderedExtensions
    {
        public static IQueryable<TSource> OrderByDynamic<TSource>(
            this IQueryable<TSource> query, Expression<Func<TSource, object>> predicate, bool isDescending)
        {

            if (isDescending)
                query = query.OrderByDescending(predicate);
            else query = query.OrderBy(predicate);
            return query;
        }
        public static IOrderedQueryable<TSource> OrderByDynamic<TSource>(
            this IQueryable<TSource> query, string propertyName, bool isDescending)
        {
            var entityType = typeof(TSource);

            //Create x=>x.PropName
            var propertyInfo = entityType.GetProperty(propertyName);
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyName);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            //Get System.Linq.Queryable.OrderBy() method.
            var enumarableType = typeof(System.Linq.Queryable);

            var methodName = isDescending ? "OrderByDescending" : "OrderBy";
            var method = enumarableType.GetMethods()
                .Where(m => m.Name == methodName && m.IsGenericMethodDefinition)
                .Where(m =>
                {
                    var parameters = m.GetParameters().ToList();
                    //Put more restriction here to ensure selecting the right overload                
                    return parameters.Count == 2;//overload that has 2 parameters
                }).Single();
            //The linq's OrderBy<TSource, TKey> has two generic types, which provided here
            MethodInfo genericMethod = method
                .MakeGenericMethod(entityType, propertyInfo.PropertyType);

            /*Call query.OrderBy(selector), with query and selector: x=> x.PropName
              Note that we pass the selector as Expression to the method and we don't compile it.
              By doing so EF can extract "order by" columns and generate SQL for it.*/
            var newQuery = (IOrderedQueryable<TSource>)genericMethod
                .Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }

    }//Cls
}
