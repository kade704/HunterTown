using UnityEngine;
using UnityEngine.UI;

public class UIFinanceSlot : MonoBehaviour
{
    private Image _iconImage;
    private Text _nameText;
    private Text _maintenanceText;
    private Text _countText;
    private Text _totalText;

    private void Awake()
    {
        _iconImage = transform.Find("IconImage").GetComponent<Image>();
        _nameText = transform.Find("NameText").GetComponent<Text>();
        _maintenanceText = transform.Find("MaintenanceText").GetComponent<Text>();
        _countText = transform.Find("CountText").GetComponent<Text>();
        _totalText = transform.Find("TotalText").GetComponent<Text>();
    }

    public Construction Construction
    {
        set
        {
            _iconImage.sprite = value.DefaultSprite;
            _nameText.text = value.GetComponent<Interactable>().DisplayName;
            _maintenanceText.text = $"-{value.MaintenanceCost}/월";
        }
    }

    public int Count
    {
        set
        {
            _countText.text = $"{value}개";
            _totalText.text = $"{value * int.Parse(_maintenanceText.text.Split('/')[0])}/월";
        }
    }
}
