using UnityEngine;
using UnityEngine.UI;

public class UIConstructionSlot : MonoBehaviour
{
    [SerializeField] private Construction _constructionPrefab;

    private Button _button;
    private UIConstructionBuildPanel _constructionBuildPanel;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _constructionBuildPanel = FindObjectOfType<UIConstructionBuildPanel>();
    }

    private void Start()
    {
        _button.onClick.AddListener(() =>
        {
            _constructionBuildPanel.BuildConstruction(_constructionPrefab);
        });
    }
}
