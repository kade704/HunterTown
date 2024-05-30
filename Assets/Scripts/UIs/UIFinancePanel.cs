using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIFinancePanel : MonoBehaviour
{
    [SerializeField] private UIFinanceSlot _financeSlotPrefab;

    private Button _closeButton;
    private CanvasGroup _canvasGroup;
    private Transform _financeSlotContainer;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _closeButton = transform.Find("CloseButton").GetComponent<Button>();
        _financeSlotContainer = transform.Find("FinanceSlots/Viewport/Container");
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
        var moneySystem = GameManager.Instance.GetSystem<MoneySystem>();

        foreach (Transform child in _financeSlotContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var element in moneySystem.ExpenditureMap)
        {
            var slot = Instantiate(_financeSlotPrefab, _financeSlotContainer);
            slot.Construction = constructionDatabase.GetConstructionPrefab(element.Key);
            slot.Count = element.Value;
        }
    }
}
