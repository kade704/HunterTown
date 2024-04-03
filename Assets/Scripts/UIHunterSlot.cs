using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHunterSlot : MonoBehaviour
{
    private Text _name;
    private Image _sprite;
    private Hunter _hunter;
    private Button _button;
    private bool _enable = true;

    public bool Enable
    {
        get
        {
            return _enable;
        }
        set
        {
            _button.interactable = value;
            _enable = value;
        }
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _name = transform.Find("Name").GetComponent<Text>();
        _sprite = transform.Find("Sprite").GetComponent<Image>();
    }


    public Hunter Hunter
    {
        set
        {
            _name.text = value.DisplayName;
            _sprite.sprite = value.Sprite;
            _hunter = value;
        }
        get
        {
            return _hunter;
        }
    }
}
