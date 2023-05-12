namespace TransactionsHandler.Services
{
    public interface ICryptographyHelper
    {
        string ComputeSha256Hash(string text);
        string Decrypt(string encryptedText, byte[] key, byte[] iv);
        string Encrypt(string text, byte[] key, byte[] iv);
        byte[] GetIV();
    }
}