using UnityEngine;
using UnityEngine.UI;

public class UIHunterButton : MonoBehaviour
{
    [SerializeField] private Image _sprite;
    private Hunter _hunter;
    private Button _button;
    private Outline _outline;

    public Button.ButtonClickedEvent OnClick => _button.onClick;
    public Outline Outline => _outline;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _outline = GetComponent<Outline>();
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
