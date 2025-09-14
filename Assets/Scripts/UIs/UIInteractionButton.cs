using UnityEngine;
using UnityEngine.UI;

public class UIInteractionButton : MonoBehaviour
{
    [SerializeField] private Text _name;
    [SerializeField] private Text _key;
    [SerializeField] private Image _icon;

    private Button _button;

    public Button.ButtonClickedEvent OnClick => _button.onClick;

    public Interaction Interaction
    {
        set
        {
            _name.text = value.DisplayName;
            _key.text = value.Key.ToString();
            _icon.sprite = value.Icon;
        }
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
    }
}
