using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;

namespace CM.App.Helper.Extensions;

public static class LocalStorageServiceExtension
{
    public static async Task SaveItemEncryptedAsync<T>(ILocalStorageService localStorageService, string key, T item)
    {
        var itemJson = JsonSerializer.Serialize(item);
        var itemJsonBytes = Encoding.UTF8.GetBytes(itemJson);
        var base64Json = Convert.ToBase64String(itemJsonBytes);
        await localStorageService.SetItemAsStringAsync(key, base64Json);
    }
    
    public static async Task<T> ReadEncryptedItemAsync<T>(ILocalStorageService localStorageService, string key)
    {
        var base64Json = await localStorageService.GetItemAsync<string>(key);
        var itemJsonBytes = Convert.FromBase64String(base64Json);
        var itemJson = Encoding.UTF8.GetString(itemJsonBytes);
        var item = JsonSerializer.Deserialize<T>(itemJson);

        return item;
    }
}