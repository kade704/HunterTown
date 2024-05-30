using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class MoneySystem : MonoBehaviour, ISerializable, IDeserializable
{
    [ReadOnly] private int _money;
    [ReadOnly] private int _expenditure;

    private Dictionary<string, int> _expenditureMap = new();
    private NotificationSystem.Message _minusMessage;

    private UnityEvent<int> _onMoneyChanged = new();
    private UnityEvent<int> _onExpenditureChanged = new();

    public Dictionary<string, int> ExpenditureMap => _expenditureMap;
    public UnityEvent<int> OnMoneyChanged => _onMoneyChanged;
    public UnityEvent<int> OnExpenditureChanged => _onExpenditureChanged;

    public int Money
    {
        get => _money;
        set
        {
            _money = value;
            OnMoneyChanged.Invoke(value);
        }
    }

    public int Expenditure
    {
        get => _expenditure;
        set
        {
            _expenditure = value;
            OnExpenditureChanged.Invoke(value);
        }
    }

    private void Start()
    {
        _onMoneyChanged.AddListener(MoneyChanged);

        var constructionGridMap = FindObjectOfType<ConstructionGridmap>();
        constructionGridMap.OnConstructionBuilded.AddListener((construction) =>
        {
            if (_expenditureMap.ContainsKey(construction.ID) == false)
                _expenditureMap[construction.ID] = 0;
            _expenditureMap[construction.ID]++;
            _expenditure += construction.GetComponent<Construction>().MaintenanceCost;
        });

        constructionGridMap.OnConstructionDestroyed.AddListener((construction) =>
        {
            _expenditureMap[construction.ID]--;
            _expenditure -= construction.GetComponent<Construction>().MaintenanceCost;
        });

        Expenditure = constructionGridMap.Constructions.Sum(construction => construction.GetComponent<Construction>().MaintenanceCost);

        var timeSystem = GameManager.Instance.GetSystem<TimeSystem>();
        timeSystem.Month.OnChanged.AddListener(() =>
        {
            Money -= Expenditure;
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
