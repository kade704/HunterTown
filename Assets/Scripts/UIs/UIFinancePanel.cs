using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIFinancePanel : MonoBehaviour
{
    [SerializeField] private UIFinanceSlot _financeSlotPrefab;

    private Button _closeButton;
    private CanvasGroup _canvasGroup;
    private Transform _financeSlotContainer;
    private Text _populationText;
    private Text _expenseText;
    private Text _incomeText;
    private Text _amountText;


    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _closeButton = transform.Find("CloseButton").GetComponent<Button>();
        _financeSlotContainer = transform.Find("FinanceSlots/Viewport/Container");
        _populationText = transform.Find("PopulationText").GetComponent<Text>();
        _expenseText = transform.Find("ExpenseText").GetComponent<Text>();
        _incomeText = transform.Find("IncomeText").GetComponent<Text>();
        _amountText = transform.Find("AmountText").GetComponent<Text>();
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

        var expense = constructionGroup.Sum(element => element.Prefab.MaintenanceCost * element.Count);

        _expenseText.text = $"월간 지출 <color=#ff0000>-{expense}원/월</color>";

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

        var populationSystem = GameManager.Instance.GetSystem<PopulationSystem>();

        _populationText.text = $"인구수({populationSystem.Population})\n* 45원/명";

        var income = populationSystem.Population * 45;
        _incomeText.text = $"월간 수익 <color=#00ff00>+{populationSystem.Population * 45}원/월</color>";

        var amount = income - expense;
        var amountStr = amount >= 0 ? $"<color=#00ff00>+{amount}</color>" : $"<color=#ff0000>{amount}</color>";
        _amountText.text = $"월 매출 {amountStr}원/월";
    }
}
