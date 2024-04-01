using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIDispatchSlot : MonoBehaviour
{
    private UIDispatch _uiDispatch;
    private Text _name;
    private Image _sprite;
    private Hunter _hunter;
    private Text _deathProbability;

    private void Awake()
    {
        _uiDispatch = GetComponentInParent<UIDispatch>();
        _name = transform.Find("Name").GetComponent<Text>();
        _sprite = transform.Find("Sprite").GetComponent<Image>();
        _deathProbability = transform.Find("DeathProbability").GetComponent<Text>();
    }

    public Hunter Hunter
    {
        set
        {
            _name.text = value.DisplayName;
            _sprite.sprite = value.Sprite;
            _hunter = value;

            var deathPercent = (int)(CalcDeathProbability(value) * 100);
            _deathProbability.text = $"사망 확률: {deathPercent}%";
        }
        get
        {
            return _hunter;
        }
    }

    public float CalcDeathProbability(Hunter hunter)
    {
        var portal = _uiDispatch.TargetPortal;
        float unit_Die_Per = portal.Unit_Die_Base_Count(hunter, true);
        if (portal.Abilities.Contains(Portal.Ability.Peace))
        {
            unit_Die_Per += 0.02f;
        }
        if (portal.Abilities.Contains(Portal.Ability.Lush_Forest))
        {
            unit_Die_Per -= 0.03f;
        }
        if (unit_Die_Per < 0)
        {
            unit_Die_Per = 0;
        }
        return unit_Die_Per;
    }
}
