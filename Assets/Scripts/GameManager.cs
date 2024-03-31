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
