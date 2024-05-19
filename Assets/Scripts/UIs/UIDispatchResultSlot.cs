using UnityEngine;
using UnityEngine.UI;

public class UIDispatchResultSlot : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Text _name;
    [SerializeField] private Text _result;

    public Hunter Hunter
    {
        set
        {
            if (value)
            {
                _icon.sprite = value.Thumbnail;
                _name.text = value.DisplayName;
            }
            else
            {
                _icon.sprite = null;
                _name.text = "[헌터 미배치]";
            }
        }
    }

    public string Result
    {
        set
        {
            _result.text = value;
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }
}
