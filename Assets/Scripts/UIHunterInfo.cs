using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIFade))]
public class UIHunterInfo : MonoBehaviour
{
    private UIFade _fade;
    private Text _name;
    private Text _hp;
    private Text _damage;

    private void Awake()
    {
        _fade = GetComponent<UIFade>();

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
                _hp.text = "생존력: " + value.DefaultHp;
                _damage.text = "공격력: " + value.DefaultDamage;

                _fade.FadeIn();
            }
            else
            {
                _fade.FadeOut();
            }
        }
    }
}
