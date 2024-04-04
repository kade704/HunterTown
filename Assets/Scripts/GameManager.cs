using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
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

    public int GameMinute
    {
        get
        {
            return (int)(Time.time % 60);
        }
    }

    public int GameHour
    {
        get
        {
            return (int)(Time.time / 60);
        }
    }

    public int GameDay
    {
        get
        {
            return (int)(Time.time / 60 / 24) + 1;
        }
    }


    public int GameMonth
    {
        get
        {
            return (int)(Time.time / 60);
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var npc = Resources.Load<GameObject>("NPC");
            Instantiate(npc);
        }
    }
}
