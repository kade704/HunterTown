using System.Linq;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class MoneySystem : MonoBehaviour, ISerializable, IDeserializable
{
    [ReadOnly] private int _money;
    [ReadOnly] private int _expenditure;

    private UnityEvent<int> _onMoneyChanged = new();
    private UnityEvent<int> _onExpenditureChanged = new();

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
        var constructionGridMap = FindObjectOfType<ConstructionGridmap>();
        constructionGridMap.OnConstructionBuilded.AddListener((construction) =>
        {
            _expenditure += construction.GetComponent<Construction>().MaintenanceCost;
        });

        constructionGridMap.OnConstructionDestroyed.AddListener((construction) =>
        {
            _expenditure -= construction.GetComponent<Construction>().MaintenanceCost;
        });



        Expenditure = constructionGridMap.Constructions.Sum(construction => construction.GetComponent<Construction>().MaintenanceCost);

        var timeSystem = GameManager.Instance.GetSystem<TimeSystem>();
        timeSystem.Month.OnChanged.AddListener(() =>
        {
            Money -= Expenditure;
        });
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
