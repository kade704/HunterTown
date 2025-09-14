using UnityEngine;
using UnityEngine.UI;

public class UIDispatchResultSlot : MonoBehaviour
{
    private Image _iconImage;
    private Text _nameText;
    private Text _deathText;
    private Text _hpText;
    private Text _damageText;

    private void Awake()
    {
        _iconImage = transform.Find("IconImage").GetComponent<Image>();
        _nameText = transform.Find("NameText").GetComponent<Text>();
        _deathText = transform.Find("DeathText").GetComponent<Text>();
        _hpText = transform.Find("HPText").GetComponent<Text>();
        _damageText = transform.Find("DamageText").GetComponent<Text>();
    }

    public DispatchHunter DispatchHunter
    {
        set
        {
            if (value.Hunter)
            {
                _iconImage.enabled = true;
                _iconImage.sprite = value.Hunter.Thumbnail;
                _nameText.text = value.Hunter.Interactable.DisplayName;

                if (value.WillDeath)
                {
                    _deathText.enabled = true;
                    _hpText.enabled = false;
                    _damageText.enabled = false;
                }
                else
                {
                    _deathText.enabled = false;
                    _hpText.enabled = true;
                    _damageText.enabled = true;
                    _hpText.text = $"방어력: {value.Hunter.DefaultHp} > {value.Hunter.DefaultHp + value.IncleaseHP} (+{value.IncleaseHP})";
                    _damageText.text = $"공격력: {value.Hunter.DefaultDamage} > {value.Hunter.DefaultDamage + value.IncleaseDamage} (+{value.IncleaseDamage})";
                }
            }
            else
            {
                _iconImage.enabled = false;
                _iconImage.sprite = null;
                _deathText.enabled = false;
                _hpText.enabled = false;
                _damageText.enabled = false;
                _nameText.text = "[헌터 미배치]";
            }
        }
    }
}
