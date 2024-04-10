using UnityEngine;
using UnityEngine.UI;

public class UIAbilityInfo : UIPanel
{
    private Text _name;
    private Text _effect;
    private Text _description;

    protected override void Awake()
    {
        base.Awake();

        _name = transform.Find("Name").GetComponent<Text>();
        _effect = transform.Find("Effect").GetComponent<Text>();
        _description = transform.Find("Description").GetComponent<Text>();
    }

    public Ability Ability
    {
        set
        {
            if (value)
            {
                _name.text = value.DisplayName;
                _effect.text = value.Effect;
                _effect.color = value.Advantage ? Color.blue : Color.red;
                _description.text = value.Description;

                ShowPanel();
            }
            else
            {
                HidePanel();
            }
        }
    }
}
