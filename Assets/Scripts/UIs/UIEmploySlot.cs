using UnityEngine;
using UnityEngine.UI;

public class UIEmploySlot : MonoBehaviour
{
    [SerializeField] Button _employButton;
    [SerializeField] Text _nameText;
    [SerializeField] Text _hpText;
    [SerializeField] Text _damageText;
    private EmployHunter _employHunter;
    private CanvasGroup _canvasGroup;

    public Button EmployButton => _employButton;
    public EmployHunter EmployHunter
    {
        get => _employHunter;
        set
        {
            _employHunter = value;
            _nameText.text = _employHunter.Name;
            _hpText.text = $"방어력: {_employHunter.HP}";
            _damageText.text = $"공격력: {_employHunter.Damage}";
        }
    }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        UIUtil.ShowCanvasGroup(_canvasGroup);
    }

    public void Hide()
    {
        UIUtil.HideCanvasGroup(_canvasGroup);
    }
}
