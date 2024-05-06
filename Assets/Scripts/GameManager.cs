using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private Dictionary<string, MonoBehaviour> _systems = new();

    private bool _loadFromSave = false;

    public static GameManager Instance => _instance;



    public T GetSystem<T>() where T : MonoBehaviour
    {
        var systemType = typeof(T).Name;
        if (_systems.ContainsKey(systemType))
        {
            if (!_systems[systemType])
            {
                _systems[systemType] = FindObjectOfType<T>();
            }
            return _systems[systemType] as T;
        }
        else
        {
            var system = FindObjectOfType<T>();
            _systems.Add(systemType, system);
            return system;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (scene.name == "Game")
            {
                InitializeGame();
                GetSystem<AudioController>().PlayAmbience("CityAmbience");
            }
        };

        LoadOptions();
        GoMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            LoadGame();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            SaveGame();
        }
    }

    public void NewGame()
    {
        _loadFromSave = false;
        SceneManager.LoadScene("Game");
    }

    private void InitializeGame()
    {
        var savePath = Application.persistentDataPath + "/SaveData.json";
        if (!File.Exists(savePath) || !_loadFromSave)
        {
            var player = GetSystem<Player>();
            player.Money = 10000;

            var constructionGridMap = GetSystem<ConstructionGridmap>();
            var street = GetSystem<ConstructionDatabase>().GetConstructionPrefab("#street");
            var company = GetSystem<ConstructionDatabase>().GetConstructionPrefab("#company");
            constructionGridMap.BuildConstruction(company, new Vector2Int(32, 32));
            constructionGridMap.BuildConstruction(street, new Vector2Int(31, 31));
            constructionGridMap.BuildConstruction(street, new Vector2Int(31, 32));
            constructionGridMap.BuildConstruction(street, new Vector2Int(31, 33));
            constructionGridMap.BuildConstruction(street, new Vector2Int(32, 31));
            constructionGridMap.BuildConstruction(street, new Vector2Int(33, 31));

            var hunterSpawner = GetSystem<HunterSpawner>();
            hunterSpawner.SpawnRandomHunter();
            hunterSpawner.SpawnRandomHunter();
            hunterSpawner.SpawnRandomHunter();
            hunterSpawner.SpawnRandomHunter();
            hunterSpawner.SpawnRandomHunter();
        }
        else
        {
            var saveData = JObject.Parse(File.ReadAllText(savePath));
            GetSystem<Player>().Deserialize(saveData["player"]);
            GetSystem<HunterSpawner>().Deserialize(saveData["hunters"]);
            GetSystem<ConstructionGridmap>().Deserialize(saveData["constructions"]);

            GetSystem<LoggerSystem>().LogInfo("게임 불러옴.");
        }
    }

    public void LoadGame()
    {
        _loadFromSave = true;
        SceneManager.LoadScene("Game");
    }

    public void SaveGame()
    {
        if (SceneManager.GetActiveScene().name != "Game")
        {
            Debug.LogWarning("You can only save the game while playing.");
            return;
        }

        var savePath = Application.persistentDataPath + "/SaveData.json";
        var root = new JObject
        {
            ["player"] = GetSystem<Player>().Serialize(),
            ["hunters"] = GetSystem<HunterSpawner>().Serialize(),
            ["constructions"] = GetSystem<ConstructionGridmap>().Serialize()
        };
        File.WriteAllText(savePath, root.ToString());

        GetSystem<LoggerSystem>().LogInfo("게임 저장됨.");
    }

    public void SaveOptions()
    {
        var parser = new INIParser();
        parser.Open(Application.persistentDataPath + "/Options.ini");
        parser.WriteValue("Audio", "MasterVolume", GetSystem<AudioController>().MasterVolume.ToString());
        parser.WriteValue("Audio", "MusicVolume", GetSystem<AudioController>().MusicVolume.ToString());
        parser.WriteValue("Audio", "SFXVolume", GetSystem<AudioController>().SFXVolume.ToString());
        parser.WriteValue("Audio", "AmbienceVolume", GetSystem<AudioController>().AmbienceVolume.ToString());
        parser.Close();

        Debug.Log("Options saved.");
    }

    public void LoadOptions()
    {
        var parser = new INIParser();
        parser.Open(Application.persistentDataPath + "/Options.ini");
        GetSystem<AudioController>().MasterVolume = (float)parser.ReadValue("Audio", "MasterVolume", 1.0);
        GetSystem<AudioController>().MusicVolume = (float)parser.ReadValue("Audio", "MusicVolume", 0.5);
        GetSystem<AudioController>().SFXVolume = (float)parser.ReadValue("Audio", "SFXVolume", 0.8);
        GetSystem<AudioController>().AmbienceVolume = (float)parser.ReadValue("Audio", "AmbienceVolume", 0.5);
        parser.Close();

        Debug.Log("Options loaded.");
    }

    public void GoMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GoOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
