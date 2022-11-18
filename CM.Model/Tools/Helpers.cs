namespace CM.Model.Tools;

public static class Helpers
{
    public static Dictionary<string, string> ObjectToDictionary(object obj)
    {
        var infos = obj.GetType().GetProperties();
        var dict = new Dictionary<string, string>();

        foreach (var info in infos)
        {
            var value = info.GetValue(obj, null)!.ToString() ?? string.Empty;
            
            dict.Add(info.Name, value);
        }

        return dict;
    }
}