using UnityEngine;
using UnityEngine.UI;

public class UIDispatchResultSlot : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Text _name;
    [SerializeField] private Text _result;

    public void SetResult(Sprite icon, string name, string result)
    {
        _icon.sprite = icon;
        _name.text = name;
        _result.text = result;
    }
}
