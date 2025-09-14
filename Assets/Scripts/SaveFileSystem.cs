using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Security.Cryptography;

public class SaveFileSystem : MonoBehaviour
{
    private byte[] _key;
    private byte[] _iv;

    private int _selectedSave = 1;

    private void Start()
    {
        GenerateKeyAndIV();
    }

    public bool HasSaveFile()
    {
        var savePath = Application.persistentDataPath + $"/SaveData{_selectedSave}.sav";
        return File.Exists(savePath);
    }

    public bool HasSaveFile(int saveIndex)
    {
        var savePath = Application.persistentDataPath + $"/SaveData{saveIndex}.sav";
        return File.Exists(savePath);
    }

    public void SetSelectedSave(int save)
    {
        _selectedSave = save;
    }

    public string GetSaveTimeString(int saveIndex)
    {
        var savePath = Application.persistentDataPath + $"/SaveData{saveIndex}.sav";
        if (!File.Exists(savePath))
        {
            return null;
        }
        return File.GetLastWriteTime(savePath).ToString();
    }

    public void LoadGame()
    {
        var savePath = Application.persistentDataPath + $"/SaveData{_selectedSave}.sav";
        if (!File.Exists(savePath))
        {
            Debug.LogWarning($"Save file Index {_selectedSave} does not exist.");
            return;
        }

        var decryptedData = Decrypt(File.ReadAllText(savePath));

        if (!CheckSaveTextCorrect(decryptedData))
        {
            Debug.LogWarning($"Save file Index {_selectedSave} is corrupted.");
            return;
        }

        var saveData = JObject.Parse(decryptedData);
        GameManager.Instance.GetSystem<TimeSystem>().Deserialize(saveData["time"]);
        GameManager.Instance.GetSystem<MoneySystem>().Deserialize(saveData["money"]);
        GameManager.Instance.GetSystem<PopulationSystem>().Deserialize(saveData["population"]);
        GameManager.Instance.GetSystem<HunterSpawner>().Deserialize(saveData["hunters"]);
        GameManager.Instance.GetSystem<ConstructionGridmap>().Deserialize(saveData["constructions"]);

        GameManager.Instance.GetSystem<NotificationSystem>().NotifyInfo("게임 불러옴.");
    }

    public void SaveGame()
    {
        if (SceneManager.GetActiveScene().name != "Game")
        {
            Debug.LogWarning("You can only save the game while playing.");
            return;
        }

        var savePath = Application.persistentDataPath + $"/SaveData{_selectedSave}.sav";
        var root = new JObject
        {
            ["time"] = GameManager.Instance.GetSystem<TimeSystem>().Serialize(),
            ["money"] = GameManager.Instance.GetSystem<MoneySystem>().Serialize(),
            ["population"] = GameManager.Instance.GetSystem<PopulationSystem>().Serialize(),
            ["hunters"] = GameManager.Instance.GetSystem<HunterSpawner>().Serialize(),
            ["constructions"] = GameManager.Instance.GetSystem<ConstructionGridmap>().Serialize()
        };
        var encryptedData = Encrypt(root.ToString());
        File.WriteAllText(savePath, encryptedData);

        GameManager.Instance.GetSystem<NotificationSystem>().NotifyInfo("게임 저장됨.");
    }

    private string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = _key;
            aes.IV = _iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }
                return System.Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    private string Decrypt(string cipherText)
    {
        byte[] buffer = System.Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = _key;
            aes.IV = _iv;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }

    private bool CheckSaveTextCorrect(string text)
    {
        try
        {
            var obj = JObject.Parse(text);
            return obj["time"] != null && obj["money"] != null && obj["population"] != null &&
                   obj["hunters"] != null && obj["constructions"] != null;
        }
        catch
        {
            return false;
        }
    }

    private void GenerateKeyAndIV()
    {
        byte[] key = new byte[32];
        byte[] iv = new byte[16];
        byte b = 0x30;
        byte offset = 5;
        for (int i = 0; i < 32; i++)
        {
            key[i] = b;
            b += offset;
            b ^= 0x2A;
            offset = (byte)((offset + 3) % 7 + 1);
        }
        for (int i = 0; i < 16; i++)
        {
            iv[i] = b;
            b -= offset;
            b ^= 0x17;
            offset = (byte)((offset + 2) % 5 + 1);
        }

        _key = key;
        _iv = iv;
    }
}
