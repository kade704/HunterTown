using UnityEngine;
using UnityEngine.UI;

public class UIInteractionButton : MonoBehaviour
{
    private Text _nameText;
    private Image _icon;
    private Button _button;

    public Button.ButtonClickedEvent OnClick => _button.onClick;

    public ConstructionInteraction Interaction
    {
        set
        {
            _nameText.text = value.DisplayName;
            _icon.sprite = value.Icon;
        }
    }

    private void Awake()
    {
        _nameText = transform.Find("Name").GetComponent<Text>();
        _icon = transform.Find("Icon").GetComponent<Image>();
        _button = GetComponent<Button>();
    }
}
