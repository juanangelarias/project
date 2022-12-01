using System.Security.Cryptography;
using System.Text;
using CM.Common.Configuration.Models;

namespace CM.Core.Services.Encryption;

public class EncryptionService : IEncryptionService
{
    private readonly Keys _keys;

    public EncryptionService(Keys keys)
    {
        _keys = keys;
    }

    public string Encrypt(string clearText, string? encryptionKey = null)
    {
        encryptionKey ??= _keys.Encryption;
        
        var clearBytes = Encoding.Unicode.GetBytes(clearText);
        using var encryptor = Aes.Create();
        var rand = new Random();
            
        var iv = new byte[15];
        rand.NextBytes(iv);
        var pdb = new Rfc2898DeriveBytes(encryptionKey!, iv);
        encryptor.Key = pdb.GetBytes(32);
        encryptor.IV = pdb.GetBytes(16);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        {
            cs.Write(clearBytes, 0, clearBytes.Length);
            cs.Close();
        }
        clearText = Convert.ToBase64String(iv) + Convert.ToBase64String(ms.ToArray());

        return clearText;
    }
    
    public string Decrypt(string cipherText, string? encryptionKey = null)
    {
        encryptionKey ??= _keys.Encryption;
        
        var iv = Convert.FromBase64String(cipherText.Substring(0, 20));
        cipherText = cipherText.Substring(20).Replace(" ", "+");
        var cipherBytes = Convert.FromBase64String(cipherText);
        using var encryptor = Aes.Create();
        var pdb = new Rfc2898DeriveBytes(encryptionKey, iv);
        encryptor.Key = pdb.GetBytes(32);
        encryptor.IV = pdb.GetBytes(16);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        {
            cs.Write(cipherBytes, 0, cipherBytes.Length);
            cs.Close();
        }
        cipherText = Encoding.Unicode.GetString(ms.ToArray());

        return cipherText;
    }

    /*public string Encrypt(string text, string? salt = null)
    {
        salt ??= _keys.Encryption;
        
        var iv = new byte[16];
        byte[] array;

        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(salt!);
            aes.IV = iv;

            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);

            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream = new(memoryStream, encrypt, CryptoStreamMode.Write);
            using (StreamWriter streamWriter = new(cryptoStream))
            {
                streamWriter.Write(text);
            }

            array = memoryStream.ToArray();
        }

        var hashed = Convert.ToBase64String(array);
        var replacedHashed = ReplaceSpecialCharacters(hashed);
        
        return replacedHashed;
    }

    public string Decrypt(string hashed, string? salt = null)
    {
        var hashedOriginal = RestoreSpecialCharacters(hashed);
        
        salt ??= _keys.Encryption;
        
        var iv = new byte[16];
        var buffer = Convert.FromBase64String(hashedOriginal);

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(salt!);
        aes.IV = iv;
        var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);

        using MemoryStream memoryStream = new(buffer);
        using CryptoStream cryptoStream = new(memoryStream, decrypt, CryptoStreamMode.Read);
        using StreamReader streamReader = new(cryptoStream);
        
        return streamReader.ReadToEnd();
    }*/

    public string OneWayEncrypt(string text, string? key = null)
    {
        key ??= _keys.Encryption;

        var hasher = SHA256.Create();

        var textWithSaltBytes = Encoding.UTF8.GetBytes(string.Concat(text, key));
        var hashedBytes = hasher.ComputeHash(textWithSaltBytes);
        hasher.Clear();

        return Convert.ToBase64String(hashedBytes);
    }

    public string CreateSalt()
    {
        var salt = string.Empty;
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*?<>";
        var seed = DateTime.Now.Day * 1000000000 + DateTime.Now.Hour * 10000000 + DateTime.Now.Minute * 100000 +
                   DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
        var rnd = new Random(seed);
        for (var i = 0; i < 64; i++)
        {
            var start = rnd.Next(0, 73);
            salt += chars.Substring(start, 1);
        }

        return salt;
    }

    /*private string ReplaceSpecialCharacters(string input)
    {
        var output = input;

        output = output.Replace("%", "%25");
        output = output.Replace(" ", "%20");
        output = output.Replace("!", "%21");
        output = output.Replace("#", "%23");
        output = output.Replace("$", "%24");
        output = output.Replace("&", "%26");
        output = output.Replace("'", "%27");
        output = output.Replace("(", "%28");
        output = output.Replace(")", "%29");
        output = output.Replace("*", "%2A");
        output = output.Replace("+", "%2B");
        output = output.Replace(",", "%2C");
        output = output.Replace("/", "%2F");
        output = output.Replace(":", "%3A");
        output = output.Replace(";", "%3B");
        output = output.Replace("=", "%3D");
        output = output.Replace("?", "%3F");
        output = output.Replace("@", "%40");
        output = output.Replace("[", "%5B");
        output = output.Replace("]", "%5D");

        return output;
    }

    private string RestoreSpecialCharacters(string input)
    {
        var output = input;

        output = output.Replace("%20", " ");
        output = output.Replace("%21", "!");
        output = output.Replace("%23", "#");
        output = output.Replace("%24", "$");
        output = output.Replace("%26", "&");
        output = output.Replace("%27", "'");
        output = output.Replace("%28", "(");
        output = output.Replace("%29", ")");
        output = output.Replace("%2A", "*");
        output = output.Replace("%2B", "+");
        output = output.Replace("%2C", ",");
        output = output.Replace("%2F", "/");
        output = output.Replace("%3A", ":");
        output = output.Replace("%3B", ";");
        output = output.Replace("%3D", "=");
        output = output.Replace("%3F", "?");
        output = output.Replace("%40", "@");
        output = output.Replace("%5B", "[");
        output = output.Replace("%5D", "]");
        output = output.Replace("%25", "%");

        return output;
    }*/
}