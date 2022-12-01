namespace CM.Core.Services.Encryption;

public interface IEncryptionService
{
    string Encrypt(string text, string? salt = null);
    string Decrypt(string hashed, string? salt = null);
    string OneWayEncrypt(string text, string? salt = null);
    string CreateSalt();
}