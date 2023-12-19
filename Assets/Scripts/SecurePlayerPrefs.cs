using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SecurePlayerPrefs : MonoBehaviour
{
    public static SecurePlayerPrefs Instance;

    private const string password = "parolasifre";
    private const string salt = "karistirici";
    private const int iterations = 1000;
    private const int keySize = 128;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public static void SetEncryptedInt(string key, int value)
    {
        string encryptedValue = EncryptInt(value, password, salt, iterations, keySize);
        PlayerPrefs.SetString(key, encryptedValue);
    }

    public static int GetEncryptedInt(string key)
    {
        string encryptedValue = PlayerPrefs.GetString(key);
        return DecryptInt(encryptedValue, password, salt, iterations, keySize);
    }

    private static string EncryptInt(int value, string password, string salt, int iterations, int keySize)
    {
        byte[] valueBytes = BitConverter.GetBytes(value);
        using (Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), iterations))
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key.GetBytes(keySize / 8);
                aesAlg.GenerateIV();

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(valueBytes, 0, valueBytes.Length);
                    }
                    return Convert.ToBase64String(aesAlg.IV.Concat(msEncrypt.ToArray()).ToArray());
                }
            }
        }
    }

    private static int DecryptInt(string cipherText, string password, string salt, int iterations, int keySize)
    {
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        byte[] iv = cipherBytes.Take(16).ToArray();
        byte[] encryptedData = cipherBytes.Skip(16).ToArray();

        using (Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), iterations))
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key.GetBytes(keySize / 8);
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] decryptedBytes = new byte[4];
                        csDecrypt.Read(decryptedBytes, 0, decryptedBytes.Length);
                        return BitConverter.ToInt32(decryptedBytes, 0);
                    }
                }
            }
        }
    }
}
