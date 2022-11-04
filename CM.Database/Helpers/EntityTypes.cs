using System.Reflection;

namespace CM.Database.Helpers;

public static class EntityTypes
{
    public static IEnumerable<Type> GetEntityTypes<T>()
    {
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface);
    }
}