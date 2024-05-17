using UnityEngine;
using UnityEngine.UI;

public class UIHunterButton : MonoBehaviour
{
    [SerializeField] private Image _sprite;
    private Hunter _hunter;
    private Button _button;

    public Button.ButtonClickedEvent OnClick => _button.onClick;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Update()
    {
        _sprite.sprite = _hunter.Thumbnail;
    }

    public Hunter Hunter
    {
        set
        {
            _hunter = value;
        }
        get
        {
            return _hunter;
        }
    }
}
