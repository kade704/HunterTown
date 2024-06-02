using UnityEngine;
using UnityEngine.UI;

public class UIMoney : MonoBehaviour
{
    private Text _moneyText;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _moneyText = transform.Find("Text").GetComponent<Text>();
    }

    private void Start()
    {
        UpdateMoneyText();

        var moneySystem = GameManager.Instance.GetSystem<MoneySystem>();
        moneySystem.OnMoneyChanged.AddListener((money) =>
        {
            UpdateMoneyText();
        });

        moneySystem.OnAmountChanged.AddListener((expenditure) =>
        {
            UpdateMoneyText();
        });

        _button.onClick.AddListener(() =>
        {
            GameManager.Instance.GetSystem<UIFinancePanel>().Show();
        });
    }

    private void UpdateMoneyText()
    {
        var player = GameManager.Instance.GetSystem<MoneySystem>();
        var amountStr = player.Amount >= 0 ? $"<color=#00ff00>(+{player.Amount}원/월)</color>" : $"<color=#ff0000>({player.Amount}원/월)</color>";
        _moneyText.text = $"{player.Money}원 {amountStr}";
    }
}
