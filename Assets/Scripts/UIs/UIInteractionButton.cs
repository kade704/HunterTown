using UnityEngine;
using UnityEngine.UI;

public class UIInteractionButton : MonoBehaviour
{
    private Text _nameText;
    private Button _button;

    public Button.ButtonClickedEvent OnClick => _button.onClick;

    public Interaction Interaction
    {
        set
        {
            _nameText.text = value.DisplayName;
        }
    }

    private void Awake()
    {
        _nameText = transform.Find("Name").GetComponent<Text>();
        _button = GetComponent<Button>();
    }
}
