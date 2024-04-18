using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int _money;

    private static GameManager _instance;
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

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        Money = _money;
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Game");
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

        

        Debug.Log("Save Game");
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
