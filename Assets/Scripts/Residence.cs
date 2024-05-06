using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;

[RequireComponent(typeof(Construction))]
public class Residence : MonoBehaviour, ISerializable, IDeserializable
{
    [SerializeField] private int _maxOccupancy;
    private int _currentOccupancy;
    private Interactable _interactable;
    private Construction _construction;

    public int MaxOccupancy => _maxOccupancy;
    public int CurrentOccupancy => _currentOccupancy;

    private void Awake()
    {
        _construction = GetComponent<Construction>();
        _interactable = GetComponent<Interactable>();
    }

    private void Start()
    {
        StartCoroutine(OccupancyRoutine());
    }

    private void Update()
    {
        _interactable.Description = $"{_construction.Description}\n\n인구: {_currentOccupancy}/{_maxOccupancy}";
    }

    public IEnumerator OccupancyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5, 10));

            var people = Random.Range(0, 5);
            if (_currentOccupancy + people <= _maxOccupancy)
                _currentOccupancy += people;
        }
    }

    public JToken Serialize()
    {
        return new JObject
        {
            ["occupancy"] = CurrentOccupancy,
        };
    }

    public void Deserialize(JToken token)
    {
        _currentOccupancy = token["occupancy"].Value<int>();
    }
}
