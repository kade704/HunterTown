using UnityEngine;
using UnityEngine.UI;

public class UIHunterInfo : UIPanel
{
    private Text _name;
    private Text _hp;
    private Text _damage;

    protected override void Awake()
    {
        base.Awake();

        _name = transform.Find("Name").GetComponent<Text>();
        _hp = transform.Find("HP").GetComponent<Text>();
        _damage = transform.Find("Damage").GetComponent<Text>();
    }

    public Hunter Hunter
    {
        set
        {
            if (value)
            {
                _name.text = value.DisplayName;
                _hp.text = "HP: " + value.DefaultHp;
                _damage.text = "Damage: " + value.DefaultDamage;

                ShowPanel();
            }
            else
            {
                HidePanel();
            }
        }
    }
}
