using System.Linq;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class PopulationSystem : MonoBehaviour, ISerializable, IDeserializable
{
    [ReadOnly] private int _population;
    [ReadOnly] private int _maxPopulation;
    [ReadOnly] private int _populationGrowth = 5;

    private UnityEvent<int> _onPopulationChanged = new();
    private UnityEvent<int> _onMaxPopulationChanged = new();
    private UnityEvent<int> _onPopulationGrowthChanged = new();

    public UnityEvent<int> OnPopulationChanged => _onPopulationChanged;
    public UnityEvent<int> OnMaxPopulationChanged => _onMaxPopulationChanged;
    public UnityEvent<int> OnPopulationGrowthChanged => _onPopulationGrowthChanged;


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
        });

        MaxPopulation = constructionGridMap.Constructions.Where(construction => construction.GetComponent<Residence>() != null)
            .Sum(construction => construction.GetComponent<Residence>().IncreasePopulation);

        PopulationGrowth = constructionGridMap.Constructions.Where(construction => construction.GetComponent<Park>() != null)
            .Sum(construction => construction.GetComponent<Park>().IncreasePopulationGrowth) + 1;

        var timeSystem = GameManager.Instance.GetSystem<TimeSystem>();
        timeSystem.Day.OnChanged.AddListener(() =>
        {
            if (Population < MaxPopulation)
            {
                Population += PopulationGrowth;
            }
            Population = Mathf.Min(Population, MaxPopulation);
        });
    }

    public void Deserialize(JToken token)
    {
        Population = token["population"].Value<int>();
    }

    public JToken Serialize()
    {
        return new JObject
        {
            ["population"] = Population,
        };
    }
}
