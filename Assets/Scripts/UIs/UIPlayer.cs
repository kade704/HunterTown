using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    [SerializeField] private Text _money;
    [SerializeField] private Text _population;

    private void Start()
    {
        UpdateMoneyText();
        UpdatePopulationText();

        var player = GameManager.Instance.GetSystem<Player>();
        player.OnMoneyChanged.AddListener((money) =>
        {
            UpdateMoneyText();
        });

        player.OnExpenditureChanged.AddListener((expenditure) =>
        {
            UpdateMoneyText();
        });

        player.OnPopulationChanged.AddListener((population) =>
        {
            UpdatePopulationText();
        });

        player.OnMaxPopulationChanged.AddListener((population) =>
        {
            UpdatePopulationText();
        });

        player.OnPopulationGrowthChanged.AddListener((populationGrowth) =>
        {
            UpdatePopulationText();
        });
    }

    private void UpdateMoneyText()
    {
        var player = GameManager.Instance.GetSystem<Player>();
        _money.text = $"{player.Money}원 <color=#ff0000>({-player.Expenditure}원)</color>";
    }

    private void UpdatePopulationText()
    {
        var player = GameManager.Instance.GetSystem<Player>();
        _population.text = $"{player.Population}명/{player.MaxPopulation}명 (<color=#00ff00>+{player.PopulationGrowth}</color>명)";
    }
}
