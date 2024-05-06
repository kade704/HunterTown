using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    [SerializeField] private Text _money;
    [SerializeField] private Text _population;

    private void Start()
    {
        UpdateMoneyText();
        StartCoroutine(UpdatePopulationRoutine());

        var player = GameManager.Instance.GetSystem<Player>();
        player.OnMoneyChanged.AddListener((money) =>
        {
            UpdateMoneyText();
        });
    }

    private IEnumerator UpdatePopulationRoutine()
    {
        while (true)
        {
            var residences = FindObjectsOfType<Residence>();
            var population = residences.Select(r => r.CurrentOccupancy).Sum();
            var maxPopulation = residences.Select(r => r.MaxOccupancy).Sum();

            _population.text = $"인구: {population}/{maxPopulation}";
            yield return new WaitForSeconds(1);
        }
    }

    private void UpdateMoneyText()
    {
        var player = GameManager.Instance.GetSystem<Player>();
        _money.text = $"예산: {player.Money}G";
    }
}
