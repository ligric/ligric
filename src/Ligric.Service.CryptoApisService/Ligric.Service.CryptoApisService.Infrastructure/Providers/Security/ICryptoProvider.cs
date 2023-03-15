namespace Ligric.Service.CryptoApisService.Infrastructure
{
    public interface ICryptoProvider
    {
        bool VerifyHash(string password, string hash, string salt);

        string GetHash(string value, string salt);

        string GetSalt(string value);

        string Encrypt(string value);

        string Decrypt(string value);
    }
}
