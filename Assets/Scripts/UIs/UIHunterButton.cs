using UnityEngine;
using UnityEngine.UI;

public class UIHunterButton : MonoBehaviour
{
    private Image _spriteImage;
    private Hunter _hunter;
    private Button _button;

    public Button.ButtonClickedEvent OnClick => _button.onClick;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _spriteImage = transform.Find("SpriteImage").GetComponent<Image>();
    }

    public Hunter Hunter
    {
        set
        {
            _spriteImage.sprite = value.Thumbnail;
            _hunter = value;
        }
        get
        {
            return _hunter;
        }
    }
}
