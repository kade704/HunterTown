using UnityEngine;
using UnityEngine.UI;

public class UIEmploySlot : MonoBehaviour
{
    private Button _employButton;
    private Text _nameText;
    private Text _hpText;
    private Text _damageText;
    private Text _totalText;
    private EmployHunter _employHunter;


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
            _totalText.text = $"총 능력치: {_employHunter.HP + _employHunter.Damage}";
        }
    }

    private void Awake()
    {
        _employButton = transform.Find("EmployButton").GetComponent<Button>();
        _nameText = transform.Find("NameText").GetComponent<Text>();
        _hpText = transform.Find("HPText").GetComponent<Text>();
        _damageText = transform.Find("DamageText").GetComponent<Text>();
        _totalText = transform.Find("TotalText").GetComponent<Text>();
    }
}
