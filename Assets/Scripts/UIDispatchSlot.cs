using UnityEngine;
using UnityEngine.UI;

public class UIDispatchSlot : MonoBehaviour
{

    private Text _name;
    private Image _sprite;
    private Hunter _hunter;

    private void Awake()
    {
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
