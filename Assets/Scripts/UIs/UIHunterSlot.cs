using UnityEngine;
using UnityEngine.UI;

public class UIHunterSlot : MonoBehaviour
{
    [SerializeField] private Text _name;
    [SerializeField] private Image _sprite;
    private Hunter _hunter;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Update()
    {
        _name.text = _hunter.DisplayName;
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
