using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIConstructionSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Construction _constructionPrefab;

    private Button _button;
    private UIConstructionBuildPanel _constructionBuildPanel;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _constructionBuildPanel.SetInformation(_constructionPrefab);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _constructionBuildPanel.SetInformation(null);
    }

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
