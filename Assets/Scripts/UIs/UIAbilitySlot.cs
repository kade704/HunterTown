using UnityEngine;
using UnityEngine.UI;

public class UIAbilitySlot : MonoBehaviour
{
    [SerializeField] private Text _name;
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _questionMark;

    private Ability _ability;

    private bool _hidden;
    public bool Hidden
    {
        set
        {
            if (value)
            {
                _name.text = "가려짐";
                _icon.enabled = true;
                _icon.sprite = _questionMark;

            }
            else
            {
                Ability = _ability;
            }

            _hidden = value;
        }
        get
        {
            return _hidden;
        }
    }

    public Ability Ability
    {
        set
        {
            if (value)
            {
                _name.text = value.DisplayName;
                _icon.sprite = value.Icon;
                _icon.enabled = true;
            }
            else
            {
                _name.text = "비어있음";
                _icon.enabled = false;
            }
            _ability = value;
        }
        get
        {
            return _ability;
        }
    }
}
