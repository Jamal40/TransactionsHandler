using System.Security.Cryptography;
using System.Text;

namespace TransactionsHandler.Services;

public class CryptographyHelper : ICryptographyHelper
{
    public string Encrypt(string text, byte[] key, byte[] iv)
    {
        byte[] plaintextBytes = Encoding.UTF8.GetBytes(text);

        using var aes = Aes.Create();

        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;

        ICryptoTransform encryptor = aes.CreateEncryptor();

        byte[] ciphertextBytes = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);

        return Convert.ToBase64String(ciphertextBytes);
    }

    public string Decrypt(string encryptedText, byte[] key, byte[] iv)
    {
        byte[] encryptedTextBytes = Convert.FromBase64String(encryptedText);

        using var aes = Aes.Create();

        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;

        ICryptoTransform decryptor = aes.CreateDecryptor();

        byte[] plaintextBytes = decryptor.TransformFinalBlock(encryptedTextBytes, 0, encryptedTextBytes.Length);

        return Encoding.UTF8.GetString(plaintextBytes);
    }

    public string ComputeSha256Hash(string text)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(text));
        return Convert.ToHexString(bytes);
    }

    public byte[] GetIV()
    {
        using var rngCsp = RandomNumberGenerator.Create();
        byte[] iv = new byte[16];
        rngCsp.GetBytes(iv);
        return iv;
    }
}
