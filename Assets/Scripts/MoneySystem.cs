using System.Linq;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class MoneySystem : MonoBehaviour, ISerializable, IDeserializable
{
    [ReadOnly] private int _money;
    [ReadOnly] private int _income;
    [ReadOnly] private int _expense;

    private NotificationSystem.Message _minusMessage;

    private UnityEvent<int> _onMoneyChanged = new();
    private UnityEvent<int> _onAmountChanged = new();

    public UnityEvent<int> OnMoneyChanged => _onMoneyChanged;
    public UnityEvent<int> OnAmountChanged => _onAmountChanged;

    public int Money
    {
        get => _money;
        set
        {
            _money = value;
            OnMoneyChanged.Invoke(value);
        }
    }

    public int Amount
    {
        get => _income - _expense;
    }

    private void Start()
    {
        _onMoneyChanged.AddListener(MoneyChanged);

        var constructionGridMap = GameManager.Instance.GetSystem<ConstructionGridmap>();
        var populationSystem = GameManager.Instance.GetSystem<PopulationSystem>();
        foreach (var construction in constructionGridMap.Constructions)
        {
            _expense += construction.GetComponent<Construction>().MaintenanceCost;
        }

        _income += populationSystem.Population * 45;

        constructionGridMap.OnConstructionBuilded.AddListener((construction) =>
        {
            _expense += construction.GetComponent<Construction>().MaintenanceCost;
            _onAmountChanged.Invoke(Amount);
        });

        constructionGridMap.OnConstructionDestroyed.AddListener((construction) =>
        {
            _expense -= construction.GetComponent<Construction>().MaintenanceCost;
            _onAmountChanged.Invoke(Amount);
        });

        populationSystem.OnPopulationChanged.AddListener((population) =>
        {
            _income = population * 45;
            _onAmountChanged.Invoke(Amount);
        });

        var timeSystem = GameManager.Instance.GetSystem<TimeSystem>();
        timeSystem.Month.OnChanged.AddListener(() =>
        {
            Money -= Amount;
        });
    }

    private void MoneyChanged(int value)
    {
        var notificationSystem = GameManager.Instance.GetSystem<NotificationSystem>();
        if (value < 0)
        {
            if (_minusMessage == null)
            {
                _minusMessage = notificationSystem.NotifyError("잔고가 마이너스입니다. 다음달에 파산됩니다.", false);
            }
        }
        else
        {
            if (_minusMessage != null)
            {
                notificationSystem.RemoveMessage(_minusMessage);
                _minusMessage = null;
            }
        }
    }

    public void Deserialize(JToken token)
    {
        Money = token["money"].Value<int>();
    }

    public JToken Serialize()
    {
        return new JObject
        {
            ["money"] = Money,
        };
    }

}
