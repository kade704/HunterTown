using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private readonly Dictionary<string, MonoBehaviour> _systems = new();

    private int _selectedSave = 1;

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

        GoMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SaveGame();
        }
    }

    private void InitializeGame()
    {
        var savePath = Application.persistentDataPath + $"/SaveData{_selectedSave}.json";
        if (!File.Exists(savePath))
        {
            var player = GetSystem<MoneySystem>();
            player.Money = 10000;

            var constructionGridMap = GetSystem<ConstructionGridmap>();
            var company = GetSystem<ConstructionDatabase>().GetConstructionPrefab("#company");
            company.GetComponent<Company>().RemainEmployeeCount = 4;
            constructionGridMap.BuildConstruction(company, new Vector2Int(16, 16));

            GetSystem<UITutorialPanel>().Show();
        }
        else
        {
            var saveData = JObject.Parse(File.ReadAllText(savePath));
            GetSystem<TimeSystem>().Deserialize(saveData["time"]);
            GetSystem<MoneySystem>().Deserialize(saveData["money"]);
            GetSystem<PopulationSystem>().Deserialize(saveData["population"]);
            GetSystem<HunterSpawner>().Deserialize(saveData["hunters"]);
            GetSystem<ConstructionGridmap>().Deserialize(saveData["constructions"]);

            GetSystem<NotificationSystem>().NotifyInfo("게임 불러옴.");
        }
    }

    public void NewGame(int save)
    {
        _selectedSave = save;
        SceneManager.LoadScene("Intro");
    }

    public void LoadGame(int save)
    {
        _selectedSave = save;
        SceneManager.LoadScene("Game");
    }

    public void SaveGame()
    {
        if (SceneManager.GetActiveScene().name != "Game")
        {
            Debug.LogWarning("You can only save the game while playing.");
            return;
        }

        var savePath = Application.persistentDataPath + $"/SaveData{_selectedSave}.json";
        var root = new JObject
        {
            ["time"] = GetSystem<TimeSystem>().Serialize(),
            ["money"] = GetSystem<MoneySystem>().Serialize(),
            ["population"] = GetSystem<PopulationSystem>().Serialize(),
            ["hunters"] = GetSystem<HunterSpawner>().Serialize(),
            ["constructions"] = GetSystem<ConstructionGridmap>().Serialize()
        };
        File.WriteAllText(savePath, root.ToString());

        GetSystem<NotificationSystem>().NotifyInfo("게임 저장됨.");
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
