using System.Reflection;

namespace CM.Entities.Helpers;

public static class EntityTypes
{
    public static IEnumerable<Type> GetEntityTypes<T>()
    {
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface);
    }
    
    private static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
    {
        return 
            assembly.GetTypes()
                .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                .ToArray();
    }
}