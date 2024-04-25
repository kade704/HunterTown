using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _money;

    private static GameManager _instance;
    private Dictionary<string, MonoBehaviour> _systems = new();
    private UnityEvent<int> _onMoneyChanged = new();

    public static GameManager Instance => _instance;

    public UnityEvent<int> OnMoneyChanged => _onMoneyChanged;


    public int Money
    {
        get { return _money; }
        set
        {
            _money = value;
            OnMoneyChanged.Invoke(value);
        }
    }

    public T GetSystem<T>() where T : MonoBehaviour
    {
        var systemType = typeof(T).Name;
        if (_systems.ContainsKey(systemType))
        {
            return _systems[systemType].GetComponent<T>();
        }

        var system = gameObject.AddComponent<T>();
        _systems.Add(systemType, system);
        return system;
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
            }
        };

        Money = _money;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }

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
        SceneManager.LoadScene("Game");
    }

    private void InitializeGame()
    {
        var savePath = Application.persistentDataPath + "/SaveData.json";
        if (!File.Exists(savePath))
        {
            Money = 1000;

            var mapJson = Resources.Load<TextAsset>("Map").text;

            var constructionGridMap = FindObjectOfType<ConstructionGridMap>();
            constructionGridMap.Deserialize(JToken.Parse(mapJson));

            var hunterSpawner = FindObjectOfType<HunterSpawner>();
            hunterSpawner.SpawnRandomHunter();
            hunterSpawner.SpawnRandomHunter();
            hunterSpawner.SpawnRandomHunter();
            hunterSpawner.SpawnRandomHunter();
            hunterSpawner.SpawnRandomHunter();
        }
        else
        {
            var saveData = JObject.Parse(File.ReadAllText(savePath));
            Money = saveData["money"].Value<int>();
            FindObjectOfType<HunterSpawner>().Deserialize(saveData["hunters"]);
            FindObjectOfType<ConstructionGridMap>().Deserialize(saveData["constructions"]);

            GetSystem<LoggerSystem>().LogInfo("게임 불러옴.");
        }
    }

    public void LoadGame()
    {
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
            ["money"] = Money,
            ["hunters"] = FindObjectOfType<HunterSpawner>().Serialize(),
            ["constructions"] = FindObjectOfType<ConstructionGridMap>().Serialize()
        };
        File.WriteAllText(savePath, root.ToString());

        GetSystem<LoggerSystem>().LogInfo("게임 저장됨.");
    }

    public void ShowOptions()
    {
        Debug.Log("Show Options");
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}
