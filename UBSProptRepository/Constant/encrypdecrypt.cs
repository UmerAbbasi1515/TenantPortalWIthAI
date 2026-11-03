using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

public class AesEncryptionHelper
{
    private static readonly string Key = "dofkrfaosrdedofkrfaosrdedofkrfao"; // 32 bytes for AES-256
    private static readonly string Iv = "zxcvbnmdfrasdfgh"; // 16 bytes for AES

    public static string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = Encoding.UTF8.GetBytes(Iv);

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    public static async Task<string> EncryptDataAsync(object data)
    {
        // Convert the data object to JSON
        string jsonData = JsonConvert.SerializeObject(data);

        // Encrypt the JSON string
        return await Task.Run(() => Encrypt(jsonData));
    }

    public static string EncryptSingleField(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = Encoding.UTF8.GetBytes(Iv);

            using var encryptor = aes.CreateEncryptor();
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }
    }


    public static async Task<string> DecryptDataAsync(string encryptedData)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = Encoding.UTF8.GetBytes(Iv);

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using (var msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedData)))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var reader = new StreamReader(csDecrypt))
                    {
                        return await reader.ReadToEndAsync();
                    }
                }
            }
        }
    }

}
