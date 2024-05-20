using System;
using System.Collections;
using System.Linq;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, ISerializable, IDeserializable
{
    [ReadOnly] private int _money;
    [ReadOnly] private int _expenditure;
    [ReadOnly] private int _population;
    [ReadOnly] private int _maxPopulation;
    [ReadOnly] private int _populationGrowth = 5;


    private UnityEvent<int> _onMoneyChanged = new();
    private UnityEvent<int> _onExpenditureChanged = new();
    private UnityEvent<int> _onPopulationChanged = new();
    private UnityEvent<int> _onMaxPopulationChanged = new();
    private UnityEvent<int> _onPopulationGrowthChanged = new();

    public UnityEvent<int> OnMoneyChanged => _onMoneyChanged;
    public UnityEvent<int> OnExpenditureChanged => _onExpenditureChanged;
    public UnityEvent<int> OnPopulationChanged => _onPopulationChanged;
    public UnityEvent<int> OnMaxPopulationChanged => _onMaxPopulationChanged;
    public UnityEvent<int> OnPopulationGrowthChanged => _onPopulationGrowthChanged;


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

    public int Population
    {
        get => _population;
        set
        {
            _population = value;
            OnPopulationChanged.Invoke(value);
        }
    }

    public int MaxPopulation
    {
        get => _maxPopulation;
        set
        {
            _maxPopulation = value;
            OnMaxPopulationChanged.Invoke(value);
        }
    }

    public int PopulationGrowth
    {
        get => _populationGrowth;
        set
        {
            _populationGrowth = value;
            OnPopulationGrowthChanged.Invoke(value);
        }
    }

    private void Start()
    {
        var constructionGridMap = FindObjectOfType<ConstructionGridmap>();
        constructionGridMap.OnConstructionBuilded.AddListener((construction) =>
        {
            if (construction.GetComponent<Residence>() != null)
            {
                MaxPopulation += construction.GetComponent<Residence>().IncreasePopulation;
            }
            Population = Mathf.Min(Population, MaxPopulation);

            if (construction.GetComponent<Park>() != null)
            {
                PopulationGrowth += construction.GetComponent<Park>().IncreasePopulationGrowth;
            }

            _expenditure += construction.GetComponent<Construction>().MaintenanceCost;
        });

        constructionGridMap.OnConstructionDestroyed.AddListener((construction) =>
        {
            if (construction.GetComponent<Residence>() != null)
            {
                MaxPopulation -= construction.GetComponent<Residence>().IncreasePopulation;
            }
            Population = Mathf.Min(Population, MaxPopulation);

            if (construction.GetComponent<Park>() != null)
            {
                PopulationGrowth -= construction.GetComponent<Park>().IncreasePopulationGrowth;
            }

            _expenditure -= construction.GetComponent<Construction>().MaintenanceCost;
        });

        MaxPopulation = constructionGridMap.Constructions.Where(construction => construction.GetComponent<Residence>() != null)
            .Sum(construction => construction.GetComponent<Residence>().IncreasePopulation);

        PopulationGrowth = constructionGridMap.Constructions.Where(construction => construction.GetComponent<Park>() != null)
            .Sum(construction => construction.GetComponent<Park>().IncreasePopulationGrowth) + 1;

        Expenditure = constructionGridMap.Constructions.Sum(construction => construction.GetComponent<Construction>().MaintenanceCost);

        var timeSystem = GameManager.Instance.GetSystem<TimeSystem>();
        timeSystem.Day.OnChanged.AddListener(() =>
        {
            Money -= Expenditure;

            if (Population < MaxPopulation)
            {
                Population += PopulationGrowth;
            }
            Population = Mathf.Min(Population, MaxPopulation);
        });
    }
    public void Deserialize(JToken token)
    {
        Money = token["money"].Value<int>();
        Population = token["population"].Value<int>();
    }

    public JToken Serialize()
    {
        return new JObject
        {
            ["money"] = Money,
            ["population"] = Population,
        };
    }

}
