using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIFinancePanel : MonoBehaviour
{
    [SerializeField] private UIFinanceSlot _financeSlotPrefab;

    private Button _closeButton;
    private CanvasGroup _canvasGroup;
    private Transform _financeSlotContainer;
    private Text _expenditureText;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _closeButton = transform.Find("CloseButton").GetComponent<Button>();
        _financeSlotContainer = transform.Find("FinanceSlots/Viewport/Container");
        _expenditureText = transform.Find("ExpenditureText").GetComponent<Text>();
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(() =>
        {
            UIUtil.HideCanvasGroup(_canvasGroup);
        });
    }

    public void Show()
    {
        UIUtil.ShowCanvasGroup(_canvasGroup);
        Initialize();
    }

    private void Initialize()
    {
        var constructionDatabase = GameManager.Instance.GetSystem<ConstructionDatabase>();
        var constructions = GameManager.Instance.GetSystem<ConstructionGridmap>().Constructions;

        var constructionGroup = constructions.GroupBy(construction => construction.ID).Select(group => new
        {
            Prefab = constructionDatabase.GetConstructionPrefab(group.Key),
            Count = group.Count()
        }).Where(element => element.Prefab.MaintenanceCost > 0).ToList();

        var expenditure = constructionGroup.Sum(element => element.Prefab.MaintenanceCost * element.Count);

        _expenditureText.text = $"총 지출   <color=#ff0000>-{expenditure}원/월</color>";

        foreach (Transform child in _financeSlotContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var element in constructionGroup)
        {
            var slot = Instantiate(_financeSlotPrefab, _financeSlotContainer);
            slot.Construction = element.Prefab;
            slot.Count = element.Count;
        }
    }
}
