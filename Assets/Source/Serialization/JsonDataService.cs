using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

// Reference YouTube video: https://www.youtube.com/watch?v=mntS45g8OK4
namespace Source.Serialization
{
    public class JsonDataService : IDataService
    {
        private const string KEY = "X57ex9a7vKtfrDiehKljY74zPh7d/3r9rBLQ9RqC75g=";
        private const string IV = "g/pNdp3Vwd+MEm/0FmpqZg==";
        
        public bool SaveData<T>(string relativePath, T data, bool encrypted)
        {
            string path = Application.persistentDataPath + relativePath;

            try
            {
                if (File.Exists(path))
                {
                    Debug.Log("Data exists. Deleting old file and writing a new one. ");
                    File.Delete(path);
                }
                else
                {
                    Debug.Log("Writing file for the first time. ");
                }

                using var stream = File.Create(path);
                if (encrypted)
                {
                    WriteEncryptedData(data, stream);
                }
                else
                {
                    stream.Close();
                    File.WriteAllText(path, JsonConvert.SerializeObject(data));
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
                return false;
            }
        }

        private void WriteEncryptedData<T>(T data, FileStream stream)
        {
            using var aesProvider = Aes.Create();
            aesProvider.Key = Convert.FromBase64String(KEY);
            aesProvider.IV = Convert.FromBase64String(IV);
            
            using var cryptoTransform = aesProvider.CreateEncryptor();
            using var cryptoStream = new CryptoStream(
                stream,
                cryptoTransform,
                CryptoStreamMode.Write
            );
            
            // Generate new value if needed
            // Debug.Log($"Key: {Convert.ToBase64String(aesProvider.Key)}");
            // Debug.Log($"Initialization Vector: {Convert.ToBase64String(aesProvider.IV)}");
            cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));
        }

        public T LoadData<T>(string relativePath, bool encrypted)
        {
            string path = Application.persistentDataPath + relativePath;

            if (!File.Exists(path))
            {
                Debug.LogError($"Cannot load file at {path}. File does not exist. ");
                throw new FileNotFoundException($"{path} does not exist!");
            }

            try
            {
                T data;
                if (encrypted)
                {
                    data = ReadEncryptedData<T>(path);
                }
                else
                { 
                    data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
                }
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
                throw e;
            }
        }

        private T ReadEncryptedData<T>(string path)
        {
            byte[] fileBytes = File.ReadAllBytes(path);
            using var aesProvider = Aes.Create();
            aesProvider.Key = Convert.FromBase64String(KEY);
            aesProvider.IV = Convert.FromBase64String(IV);

            using var cryptoTransform = aesProvider.CreateDecryptor(
                aesProvider.Key,
                aesProvider.IV
                );
            
            using var decryptionStream = new MemoryStream(fileBytes);
            using var cryptoStream = new CryptoStream(
                decryptionStream,
                cryptoTransform,
                CryptoStreamMode.Read
            );

            using var reader = new StreamReader(cryptoStream);
            string result = reader.ReadToEnd();
            
            Debug.Log($"Decrypted result (if the following is not legible, probably wrong key or iv: {result}");
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}