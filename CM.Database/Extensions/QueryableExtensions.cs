using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CM.Database.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string propertyName,
        bool desc)
    {
        var method = desc ? "OrderByDescending" : "OrderBy";
        var type = typeof(TEntity);
        var property = type.GetProperty(propertyName);
        var parameter = Expression.Parameter(type, "p");
        if (property == null) return null!;
            
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);
        var resultExpression = Expression.Call(typeof(Queryable), method, new[] {type, property.PropertyType},
            source.Expression, Expression.Quote(orderByExpression));
        return source.Provider.CreateQuery<TEntity>(resultExpression);
    }
        
    public static IQueryable<TEntity> Filter<TEntity>(this IQueryable<TEntity> source, string filter)
    {
        var method = typeof(Queryable).GetMethods().First(m =>
        {
            var parameters = m.GetParameters().ToList();
            return m.Name == "Where" && m.IsGenericMethodDefinition &&
                   parameters.Count == 2;
        });
        var type = typeof(TEntity);

        var propNameConversionDictionary = new Dictionary<string, string>(typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(info => new KeyValuePair<string, string>(info.Name.ToLower(), info.Name)));
        var parameterValueMap = DetermineProperty(filter, propNameConversionDictionary);
        var propertyTypesDictionary = parameterValueMap.Select(property => type.GetProperty(property.Key)).ToList();
            
        // parameter for the lambda (this is the entity that is being queried, not the search parameters)
        var parameter = Expression.Parameter(typeof(TEntity));
            
        // property expressions for 'where' predicate .. taken from filter values provided
        var localBinaryExpressions = new List<BinaryExpression>();

        // generate equal expressions for search query parameters
        foreach (var prop in propertyTypesDictionary)
        {
            var equalExpression = Expression.Equal(Expression.MakeMemberAccess(parameter, prop!), Expression.Constant(parameterValueMap[prop!.Name]));
            localBinaryExpressions.Add(equalExpression);
        }

        // This still needs work.
        var andalso = Expression.AndAlso(localBinaryExpressions.First(), localBinaryExpressions.Last());
        var lamda = Expression.Lambda(andalso, parameters: parameter);

        var genericMethod = method.MakeGenericMethod(typeof(TEntity));

        return ((IQueryable<TEntity>)genericMethod.Invoke(genericMethod, new object[] {source, lamda})!);
    }
        
    public static Dictionary<string, string> DetermineProperty(string filter, Dictionary<string, string> dictionary)
    {
        //var regex = new Regex(@"\w*:\s\w*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        var regex = new Regex(@"\w*:\s*.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        var matches = regex.Matches(filter);
        var searchCriteriaDictionary = new Dictionary<string,string>();

        foreach(Match match in matches)
        {
            var items = match.Value.Split(':');
            if (items.Length == 2)
            {
                searchCriteriaDictionary.Add(dictionary[items[0].ToLower()], items[1].Trim());
            }
        }

        return searchCriteriaDictionary;
    }
}