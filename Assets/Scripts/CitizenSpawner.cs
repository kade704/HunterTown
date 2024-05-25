using System.Collections.Generic;
using UnityEngine;

public class CitizenSpawner : MonoBehaviour
{
    [SerializeField] private Citizen _citizenPrefab;

    private List<Citizen> _citizens = new();

    private const int POPULATION_PER_CITIZEN = 5;

    private void Start()
    {
        GameManager.Instance.GetSystem<Player>().OnPopulationChanged.AddListener(OnPopulationChanged);
    }

    private void OnPopulationChanged(int population)
    {
        var targetCitizenCount = population / POPULATION_PER_CITIZEN;
        if (targetCitizenCount > _citizens.Count)
        {
            var citizen = Instantiate(_citizenPrefab, transform);
            _citizens.Add(citizen);
        }
        else if (targetCitizenCount < _citizens.Count)
        {
            for (int i = 0; i < _citizens.Count - targetCitizenCount; i++)
            {
                _citizens.RemoveAt(i);
                Destroy(_citizens[i].gameObject);
            }
        }
    }
}
