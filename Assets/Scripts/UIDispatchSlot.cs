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
    private Button _removeButton;
    private int _index;

    private void Awake()
    {
        _uiDispatch = GetComponentInParent<UIDispatch>();
        _name = transform.Find("Name").GetComponent<Text>();
        _sprite = transform.Find("Sprite").GetComponent<Image>();
        _deathProbability = transform.Find("DeathProbability").GetComponent<Text>();
        _removeButton = transform.Find("RemoveButton").GetComponent<Button>();
        _removeButton.onClick.AddListener(() =>
        {
            Hunter = null;
        });
        _index = transform.GetSiblingIndex();
    }

    public Hunter Hunter
    {
        set
        {
            if (value)
            {
                _name.text = value.DisplayName;
                _sprite.sprite = value.Sprite;
                _sprite.enabled = true;

                var deathProbabilityPercent = (int)(_uiDispatch.TargetPortal.CalcHunterDeathProbability(value) * 100);
                _deathProbability.text = $"사망 확률: {deathProbabilityPercent}%";
            }
            else
            {
                _name.text = "[헌터 배치]";
                _sprite.enabled = false;

                _deathProbability.text = $"사망 확률: ?%";
            }

            _hunter = value;
            _uiDispatch.TargetPortal.HuntersToDispatch[_index] = value;
            _uiDispatch.RefreshHunterSlot();

        }
        get
        {
            return _hunter;
        }
    }


}
