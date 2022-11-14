using CM.Core.Services.Encryption;
using CM.Features;

namespace CM.TestApp;

public class Tools
{
    private readonly IUserFeature _userFeature;
    private readonly IEncryptionService _encryptionService;

    public Tools(
        IUserFeature userFeature,
        IEncryptionService encryptionService)
    {
        _userFeature = userFeature;
        _encryptionService = encryptionService;
    }

    public string GetKey()
    {
        return _encryptionService.CreateSalt();
    }

    public string Encrypt(string plainText, string key)
    {
        var hashed = _encryptionService.Encrypt(plainText, key);

        return hashed;
    }

    public string Decrypt(string hashed, string key)
    {
        var plainText = _encryptionService.Decrypt(hashed, key);

        return plainText;
    }
}