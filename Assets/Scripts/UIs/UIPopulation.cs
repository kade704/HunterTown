using UnityEngine;
using UnityEngine.UI;

public class UIPopulation : MonoBehaviour
{
    private Text _populationText;

    private void Awake()
    {
        _populationText = transform.Find("Text").GetComponent<Text>();
    }

    void Start()
    {
        var populationSystem = GameManager.Instance.GetSystem<PopulationSystem>();
        populationSystem.OnPopulationChanged.AddListener((population) =>
       {
           UpdatePopulationText();
       });

        populationSystem.OnMaxPopulationChanged.AddListener((population) =>
        {
            UpdatePopulationText();
        });

        populationSystem.OnPopulationGrowthChanged.AddListener((populationGrowth) =>
        {
            UpdatePopulationText();
        });
    }

    private void UpdatePopulationText()
    {
        var populationSystem = GameManager.Instance.GetSystem<PopulationSystem>();
        _populationText.text = $"{populationSystem.Population}명/{populationSystem.MaxPopulation}명 <color=#00ff00>(+{populationSystem.PopulationGrowth}명/일)</color>";
    }
}
