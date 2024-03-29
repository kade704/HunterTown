using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIConstructionSlot : MonoBehaviour
{
    private Button _button;
    private TMP_Text _displayNameText;
    private Image _iconImage;

    public Button.ButtonClickedEvent OnClick => _button.onClick;

    public Construction Construction
    {
        set
        {
            if (!value) return;

            _displayNameText.text = value.DisplayName;
            _iconImage.sprite = value.Icon;
        }
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _displayNameText = transform.Find("DisplayNameText").GetComponent<TMP_Text>();
        _iconImage = transform.Find("IconImage").GetComponent<Image>();
    }
}
