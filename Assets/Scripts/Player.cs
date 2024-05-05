using System.Collections;
using System.Linq;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, ISerializable, IDeserializable
{
    [ReadOnly] private int _money;
    [ReadOnly] private int _population;
    [ReadOnly] private int _maxPopulation;

    private UnityEvent<int> _onMoneyChanged = new();
    private UnityEvent<int> _onPopulationChanged = new();
    private UnityEvent<int> _onMaxPopulationChanged = new();

    public UnityEvent<int> OnMoneyChanged => _onMoneyChanged;
    public UnityEvent<int> OnPopulationChanged => _onPopulationChanged;
    public UnityEvent<int> OnMaxPopulationChanged => _onMaxPopulationChanged;

    public int Money
    {
        get => _money;
        set
        {
            _money = value;
            OnMoneyChanged.Invoke(value);
        }
    }

    public int Population
    {
        get => _population;
        set
        {
            _population = value;
            _onPopulationChanged.Invoke(value);
        }
    }

    public int MaxPopulation
    {
        get => _maxPopulation;
        set
        {
            _maxPopulation = value;
            _onMaxPopulationChanged.Invoke(value);
        }
    }


    private void Start()
    {
        RecalculateMaxPopulation();
        GameManager.Instance.GetSystem<ConstructionGridmap>().OnConstructionBuilded.AddListener(_ =>
        {
            RecalculateMaxPopulation();
        });

        GameManager.Instance.GetSystem<ConstructionGridmap>().OnConstructionDestroyed.AddListener(_ =>
        {
            RecalculateMaxPopulation();
        });

        StartCoroutine(PopulationRoutine());
    }

    private void RecalculateMaxPopulation()
    {
        MaxPopulation = FindObjectsOfType<Residence>().Select(x => x.PopulationIncrease).Sum();
    }

    public IEnumerator PopulationRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5, 10));

            var people = Random.Range(0, 5);
            if (_population + people <= _maxPopulation)
                Population += people;
        }
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
