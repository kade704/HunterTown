using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UIConstructionBuildPanel : MonoBehaviour
{
    [SerializeField] private Button _destructionButton;
    [SerializeField] private UIFade _residencePanel;
    [SerializeField] private UIFade _parkPanel;
    [SerializeField] private UIFade _roadPanel;
    [SerializeField] private Button _residenceButton;
    [SerializeField] private Button _parkButton;
    [SerializeField] private Button _roadButton;

    private void Start()
    {
        _destructionButton.onClick.AddListener(() =>
        {
            var constructionBuilder = FindObjectOfType<ConstructionBuilder>();
            constructionBuilder.SetDestructionMode(true);
        });

        _residenceButton.onClick.AddListener(() =>
        {
            _residencePanel.FadeIn();
            _parkPanel.FadeOut();
            _roadPanel.FadeOut();
        });

        _parkButton.onClick.AddListener(() =>
        {
            _residencePanel.FadeOut();
            _parkPanel.FadeIn();
            _roadPanel.FadeOut();
        });

        _roadButton.onClick.AddListener(() =>
        {
            _residencePanel.FadeOut();
            _parkPanel.FadeOut();
            _roadPanel.FadeIn();
        });
    }

    public void BuildConstruction(Construction constructionPrefab)
    {
        var constructionBuilder = FindObjectOfType<ConstructionBuilder>();
        constructionBuilder.ConstructionPrefab = constructionPrefab;

        _residencePanel.FadeOut();
        _parkPanel.FadeOut();
        _roadPanel.FadeOut();
    }
}
