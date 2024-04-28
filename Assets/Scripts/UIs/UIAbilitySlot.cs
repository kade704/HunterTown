using UnityEngine;
using UnityEngine.UI;

public class UIAbilitySlot : MonoBehaviour
{
    private Text _name;
    private Image _icon;
    private Ability _ability;
    private Sprite _questionMark;
    private bool _hidden;
    private void Awake()
    {
        _name = transform.Find("Name").GetComponent<Text>();
        _icon = transform.Find("Sprite").GetComponent<Image>();
        _questionMark = Resources.Load<Sprite>("question");
    }

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
