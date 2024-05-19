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

    private void Start()
    {
        StartCoroutine(PopulationRoutine());
    }


    public IEnumerator PopulationRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5, 10));

            if (Population < MaxPopulation)
            {
                var people = Random.Range(0, 5);
                people = Mathf.Min(people, MaxPopulation - Population);
                Population += people;
            }
            else
            {
                var people = Random.Range(-10, 0);
                people = Mathf.Max(people, -Population);
                Population += people;
            }
        }
    }

    public void Deserialize(JToken token)
    {
        Money = token["money"].Value<int>();
        Population = token["population"].Value<int>();
        MaxPopulation = token["maxPopulation"].Value<int>();
    }

    public JToken Serialize()
    {
        return new JObject
        {
            ["money"] = Money,
            ["population"] = Population,
            ["maxPopulation"] = MaxPopulation,
        };
    }

}
