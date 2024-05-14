using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIDispatchPanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Text _powerText;
    [SerializeField] private Text _dangerText;
    [SerializeField] private Text _difficultyText;
    [SerializeField] private Text _rankText;
    [SerializeField] private Text _successText;
    [SerializeField] private UIDispatchSlot[] _dispatchSlots;
    [SerializeField] private Button _dispatchButton;
    [SerializeField] private UIAbilitySlot[] _abilitySlots;
    [SerializeField] private UIAbilityInfo _abilityInfo;

    private UIAbilitySlot _abilitySlotOverPointer;
    private Portal _targetPortal;

    public Portal TargetPortal => _targetPortal;

    private void Start()
    {
        _closeButton.onClick.AddListener(() =>
        {
            _panel.SetActive(false);
        });

        _dispatchButton.onClick.AddListener(() =>
        {
            if (_targetPortal)
            {
                //_targetPortal.Dispatch();

            }
            StartCoroutine(GameManager.Instance.GetSystem<Battle>().BattleRoutine());
        });

        var interactableSelector = FindObjectOfType<InteractableSelector>();
        interactableSelector.OnInteractableInteracted.AddListener((interactable, interaction) =>
        {
            if (interaction.ID == "#dispatch")
            {
                Initialize(interactable.GetComponent<Portal>());
                _panel.SetActive(true);
            }
            else
            {
                _panel.SetActive(false);
            }
        });
    }

    private void Update()
    {
        var abilitySlotOverPointer = UIUtil.GetUIObjectTypeOverPointer<UIAbilitySlot>();
        if (abilitySlotOverPointer != _abilitySlotOverPointer)
        {
            if (abilitySlotOverPointer && !abilitySlotOverPointer.Hidden)
            {
                _abilityInfo.Ability = abilitySlotOverPointer.Ability;
            }
            else
            {
                _abilityInfo.Ability = null;
            }
            _abilitySlotOverPointer = abilitySlotOverPointer;
        }
        if (_abilitySlotOverPointer)
        {
            _abilityInfo.transform.position = Input.mousePosition;
        }
    }

    private void Initialize(Portal portal)
    {
        _targetPortal = portal;

        _powerText.text = "능력치: " + (_targetPortal.PowerVisibility ? _targetPortal.Power.ToString("F1") : "???");
        _dangerText.text = "위험도: " + (_targetPortal.DangerVisibility ? _targetPortal.Danger.ToString("F1") : "???");
        _difficultyText.text = "복잡도: " + (_targetPortal.DifficultyVisibility ? _targetPortal.Difficulty.ToString("F1") : "???");
        _rankText.text = _targetPortal.Rank.ToString();

        for (int i = 0; i < 3; i++)
        {
            var abilitySlot = _abilitySlots[i];

            abilitySlot.Ability = _targetPortal.Abilities[i];
            abilitySlot.Hidden = !_targetPortal.AbilityVisibilities[i];
        }

        var battle = GameManager.Instance.GetSystem<Battle>();

        var hunters = _targetPortal.Construction.VisitedHunters;
        for (int i = 0; i < 4; i++)
        {
            if (i < hunters.Count)
            {
                _dispatchSlots[i].SetHunter(hunters[i], _targetPortal);
                battle.SetBattleHunter(i, hunters[i].GetComponent<Hunter>());
            }
            else
            {
                _dispatchSlots[i].SetHunter(null, _targetPortal);
                battle.SetBattleHunter(i, null);
            }
        }

        var readyToDispatch = hunters.Count() > 0;
        _dispatchButton.interactable = readyToDispatch;
        if (readyToDispatch)
        {
            var success = (_targetPortal.CalcDispatchSuccessProbability(hunters.ToArray()) * 100f).ToString("F1");
            if (_targetPortal.PowerVisibility)
            {
                _successText.text = $"파견 성공 확률: {success}%";
            }
            else
            {
                _successText.text = "파견 성공 확률: ?%";
            }
        }
        else
        {
            _successText.text = "";
        }
    }
}
