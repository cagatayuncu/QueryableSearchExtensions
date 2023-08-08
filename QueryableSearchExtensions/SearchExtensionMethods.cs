using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace QueryableSearchExtensions
{
    public static class SearchExtensionMethods
    {
        public static IQueryable<T> PropertySearchQuery<T>(this DbSet<T> dbSet, string searchTerm, params Expression<Func<T, string>>[] propertySelectors) where T : class
        {
            IQueryable<T> query = dbSet;

            List<Expression> expressions = new List<Expression>();

            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");

            MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            List<string> includeParameters = propertySelectors.Select(x => x.Body.ToString().Split('.')[1]).ToList();



            foreach (PropertyInfo prop in typeof(T).GetProperties().Where(x => x.PropertyType == typeof(string) && includeParameters.Contains(x.Name)))
            {
                MemberExpression memberExpression = Expression.PropertyOrField(parameter, prop.Name);

                ConstantExpression valueExpression = Expression.Constant(searchTerm, typeof(string));

                MethodCallExpression containsExpression = Expression.Call(memberExpression, containsMethod, valueExpression);

                expressions.Add(containsExpression);
            }

            if (expressions.Count == 0)
                return query;

            Expression orExpression = expressions[0];

            for (int i = 1; i < expressions.Count; i++)
            {
                orExpression = Expression.OrElse(orExpression, expressions[i]);
            }

            Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(
                orExpression, parameter);

            return query.Where(expression);
        }
        public static IQueryable<T> GlobalSearcshQuery<T>(this DbSet<T> dbSet, string searchTerm) where T : class
        {
            IQueryable<T> query = dbSet;

            List<Expression> expressions = new List<Expression>();

            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");

            MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            foreach (PropertyInfo prop in typeof(T).GetProperties().Where(x => x.PropertyType == typeof(string)))
            {
                MemberExpression memberExpression = Expression.PropertyOrField(parameter, prop.Name);

                ConstantExpression valueExpression = Expression.Constant(searchTerm, typeof(string));

                MethodCallExpression containsExpression = Expression.Call(memberExpression, containsMethod, valueExpression);

                expressions.Add(containsExpression);
            }

            if (expressions.Count == 0)
                return query;

            Expression orExpression = expressions[0];

            for (int i = 1; i < expressions.Count; i++)
            {
                orExpression = Expression.OrElse(orExpression, expressions[i]);
            }

            Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(
                orExpression, parameter);

            return query.Where(expression);
        }

    }
   
}