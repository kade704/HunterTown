using UnityEngine;
using UnityEngine.UI;

public class UIDispatchSlot : MonoBehaviour
{
    [SerializeField] private Text _name;
    [SerializeField] private Text _deathProbability;

    private Hunter _hunter;

    public void SetHunter(Hunter hunter, Portal portal)
    {
        if (hunter)
        {
            _name.text = hunter.DisplayName;

            if (portal.DangerVisibility)
            {
                var deathProbabilityPercent = (int)(portal.CalcHunterDeathProbability(hunter) * 100);
                _deathProbability.text = $"사망 확률: {deathProbabilityPercent}%";
            }
            else
            {
                _deathProbability.text = $"사망 확률: ?%";
            }
        }
        else
        {
            _name.text = "[헌터 배치]";
            _deathProbability.text = "";
        }

        _hunter = hunter;
    }

    public Hunter Hunter => _hunter;
}
