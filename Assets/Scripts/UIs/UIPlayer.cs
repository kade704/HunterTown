using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    [SerializeField] private Text _money;
    [SerializeField] private Text _population;

    private void Start()
    {
        UpdateMoneyText();

        var player = GameManager.Instance.GetSystem<Player>();
        player.OnMoneyChanged.AddListener((money) =>
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
    }

    private void UpdateMoneyText()
    {
        var player = GameManager.Instance.GetSystem<Player>();
        _money.text = $"예산: {player.Money}G";
    }

    private void UpdatePopulationText()
    {
        var player = GameManager.Instance.GetSystem<Player>();
        _population.text = $"인구: {player.Population} / {player.MaxPopulation}";
    }
}
