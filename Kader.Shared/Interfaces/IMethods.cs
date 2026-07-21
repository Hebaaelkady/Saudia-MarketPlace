namespace Kader.Infrastructure.Shared.Interfaces
{
    public interface IMethods
    {
        string Encrypt(string plainText);
        string Decrypt(string encryptedText);
    }
}
