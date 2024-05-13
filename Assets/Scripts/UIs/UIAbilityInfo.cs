using UnityEngine;
using UnityEngine.UI;

public class UIAbilityInfo : MonoBehaviour
{
    private Text _name;
    private Text _effect;
    private Text _description;

    private void Awake()
    {
        _name = transform.Find("Name").GetComponent<Text>();
        _effect = transform.Find("Effect").GetComponent<Text>();
        _description = transform.Find("Description").GetComponent<Text>();
    }

    public Ability Ability
    {
        set
        {
            if (value)
            {
                _name.text = value.DisplayName;
                _effect.text = value.Effect;
                _effect.color = value.Advantage ? Color.blue : Color.red;
                _description.text = value.Description;

                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
