using UnityEngine;
using UnityEngine.UI;

public class UIDispatchSlot : MonoBehaviour
{
    [SerializeField] private Text _name;
    [SerializeField] private Image _sprite;
    [SerializeField] private Text _deathProbability;

    private Hunter _hunter;

    public Hunter Hunter
    {
        set
        {
            var _dispatchPanel = GetComponentInParent<UIDispatchPanel>();

            if (value)
            {
                _name.text = value.DisplayName;
                _sprite.sprite = value.Thumbnail;
                _sprite.enabled = true;

                if (_dispatchPanel.TargetPortal.DangerVisibility)
                {
                    var deathProbabilityPercent = (int)(_dispatchPanel.TargetPortal.CalcHunterDeathProbability(value) * 100);
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
                _sprite.enabled = false;
                _deathProbability.text = "";
            }

            _hunter = value;
        }
        get
        {
            return _hunter;
        }
    }


}
