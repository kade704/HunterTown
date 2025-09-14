using UnityEngine;
using UnityEngine.UI;

public class UIAbilitySlot : MonoBehaviour
{
    [SerializeField] private Text _name;
    [SerializeField] private Text _effect;
    [SerializeField] private Text _description;
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
                _effect.text = "";
                _description.text = "";
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
                _effect.text = value.Effect;
                _effect.color = value.Advantage ? Color.cyan : Color.red;
                _description.text = value.Description;
                _icon.sprite = value.Icon;
                _icon.enabled = true;
            }
            else
            {
                _name.text = "비어있음";
                _effect.text = "";
                _description.text = "";
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
