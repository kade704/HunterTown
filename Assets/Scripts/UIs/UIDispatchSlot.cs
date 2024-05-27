using UnityEngine;
using UnityEngine.UI;

public class UIDispatchSlot : MonoBehaviour
{
    [SerializeField] private Text _name;
    [SerializeField] private Text _deathProbability;

    private Hunter _hunter;


    public Hunter Hunter
    {
        get => _hunter;
        set
        {
            _hunter = value;
            if (value)
            {
                _name.text = value.Interactable.DisplayName;
            }
            else
            {
                _name.text = "[헌터 배치]";
            }
        }
    }

    public string DeathProbability
    {
        set => _deathProbability.text = value;
    }
}
