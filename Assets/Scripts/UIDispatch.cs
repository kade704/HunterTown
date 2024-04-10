using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIDispatch : UIPanel
{
    private Portal _targetPortal;
    private Button _closeButton;
    private UIHunterSlot _hunterSlotRef;
    private Transform _hunterSlotParent;
    private Image _cursor;
    private Text _powerText;
    private Text _dangerText;
    private Text _difficultyText;
    private Text _rankText;
    private Text _successText;
    private Hunter _draggingHunter;
    private Button _dispatchButton;
    private List<UIHunterSlot> _hunterSlots = new();
    private UIDispatchSlot[] _dispatchSlots;
    private UIAbilitySlot[] _abilitySlots;
    private UIHunterInfo _hunterInfo;
    private UIHunterSlot _hunterSlotOverPointer;
    private UIAbilityInfo _abilityInfo;
    private UIAbilitySlot _abilitySlotOverPointer;

    public Portal TargetPortal => _targetPortal;

    protected override void Awake()
    {
        base.Awake();

        _hunterSlotRef = transform.Find("HunterSlotRef").GetComponent<UIHunterSlot>();
        _hunterSlotParent = transform.Find("HunterSlots");
        _cursor = transform.Find("Cursor").GetComponent<Image>();
        _closeButton = transform.Find("CloseButton").GetComponent<Button>();
        _closeButton.onClick.AddListener(() =>
        {
            HidePanel();
        });
        _dispatchSlots = transform.GetComponentsInChildren<UIDispatchSlot>();
        _dispatchButton = transform.Find("DispatchButton").GetComponent<Button>();
        _dispatchButton.onClick.AddListener(() =>
        {
            if (_targetPortal)
            {
                var hunters = _dispatchSlots.Select(x => x.Hunter).OfType<Hunter>().ToArray();
                _targetPortal.Dispatch(hunters);
                HidePanel();
            }
        });
        _abilityInfo = transform.Find("AbilityInfo").GetComponent<UIAbilityInfo>();
        _abilitySlots = transform.GetComponentsInChildren<UIAbilitySlot>();
        _hunterInfo = transform.Find("HunterInfo").GetComponent<UIHunterInfo>();
        _powerText = transform.Find("PowerText").GetComponent<Text>();
        _dangerText = transform.Find("DangerText").GetComponent<Text>();
        _difficultyText = transform.Find("DifficultyText").GetComponent<Text>();
        _rankText = transform.Find("RankText").GetComponent<Text>();
        _successText = transform.Find("SuccessText").GetComponent<Text>();

        UIManager.Instance.OnConstructionInteracted.AddListener((id, construction) =>
        {
            if (id == "dispatch")
            {
                _targetPortal = construction as Portal;

                _powerText.text = "능력치: " + (_targetPortal.PowerVisibility ? _targetPortal.Power.ToString("F1") : "???");
                _dangerText.text = "위험도: " + (_targetPortal.DangerVisibility ? _targetPortal.Danger.ToString("F1") : "???");
                _difficultyText.text = "복잡도: " + (_targetPortal.DifficultyVisibility ? _targetPortal.Difficulty.ToString("F1") : "???");
                _rankText.text = _targetPortal.Rank.ToString();

                _hunterSlots.Clear();
                foreach (Transform child in _hunterSlotParent)
                {
                    Destroy(child.gameObject);
                }

                foreach (var hunter in HunterManager.Instance.Hunters)
                {
                    var hunterSlot = Instantiate(_hunterSlotRef, _hunterSlotParent);
                    hunterSlot.Hunter = hunter;
                    _hunterSlots.Add(hunterSlot);
                }

                foreach (var dispatchSlot in _dispatchSlots)
                {
                    dispatchSlot.Hunter = null;
                }

                for (int i = 0; i < 3; i++)
                {
                    var abilitySlot = _abilitySlots[i];

                    abilitySlot.Ability = _targetPortal.Abilities[i];
                    abilitySlot.Hidden = !_targetPortal.AbilityVisibilities[i];
                }

                ShowPanel();
            }
            else
            {
                HidePanel();
            }
        });
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var clickedHunterSlot = UIManager.GetUIObjectTypeOverPointer<UIHunterSlot>();
            if (clickedHunterSlot && clickedHunterSlot.Enable)
            {
                _cursor.enabled = true;
                _cursor.sprite = clickedHunterSlot.Hunter.Thumbnail;
                _draggingHunter = clickedHunterSlot.Hunter;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            var droppedDispatchSlot = UIManager.GetUIObjectTypeOverPointer<UIDispatchSlot>();
            if (droppedDispatchSlot && _draggingHunter)
            {
                RefreshHunterSlot();

                droppedDispatchSlot.Hunter = _draggingHunter;
            }

            _draggingHunter = null;
            _cursor.enabled = false;
        }
        else if (Input.GetMouseButton(0))
        {
            if (_draggingHunter)
            {
                _cursor.transform.position = Input.mousePosition;
            }
        }

        var hunterSlotOverPointer = UIManager.GetUIObjectTypeOverPointer<UIHunterSlot>();
        if (hunterSlotOverPointer != _hunterSlotOverPointer)
        {
            if (hunterSlotOverPointer)
            {
                _hunterInfo.Hunter = hunterSlotOverPointer.Hunter;
            }
            else
            {
                _hunterInfo.Hunter = null;
            }
            _hunterSlotOverPointer = hunterSlotOverPointer;
        }
        if (_hunterSlotOverPointer)
        {
            _hunterInfo.transform.position = Input.mousePosition;
        }

        var abilitySlotOverPointer = UIManager.GetUIObjectTypeOverPointer<UIAbilitySlot>();
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

        if (_targetPortal)
        {
            var hunters = _dispatchSlots.Select(x => x.Hunter).OfType<Hunter>().ToArray();
            var readyToDispatch = hunters.Count() > 0;
            _dispatchButton.interactable = readyToDispatch;
            if (readyToDispatch)
            {
                var success = (_targetPortal.CalcDispatchSuccessProbability(hunters) * 100f).ToString("F1");
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

    public void RefreshHunterSlot()
    {
        foreach (var hunterSlot in _hunterSlots)
        {
            var asd = _dispatchSlots.Where(x => x.Hunter == hunterSlot.Hunter).FirstOrDefault();
            hunterSlot.Enable = asd == null;
        }
    }
}
