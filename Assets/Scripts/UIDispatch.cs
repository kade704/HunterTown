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
    private Hunter _draggingHunter;
    private Button _dispatchButton;
    private List<UIHunterSlot> _hunterSlots = new();
    private UIDispatchSlot[] _dispatchSlots;
    private UIHunterInfo _hunterInfo;
    private UIHunterSlot _hunterSlotOverPointer;

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
                _targetPortal.Dispatch();
                HidePanel();
            }
        });
        _hunterInfo = transform.Find("HunterInfo").GetComponent<UIHunterInfo>();

        UIManager.Instance.OnConstructionInteracted.AddListener((id, construction) =>
        {
            if (id == "dispatch")
            {
                _targetPortal = construction as Portal;

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

                for (int i = 0; i < 4; i++)
                {
                    _dispatchSlots[i].Hunter = _targetPortal.HuntersToDispatch[i];
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
                _cursor.sprite = clickedHunterSlot.Hunter.Sprite;
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

        if (_targetPortal)
        {
            _dispatchButton.interactable = _targetPortal.HuntersToDispatch.Where(x => x != null).Count() > 0;
        }
    }

    public void RefreshHunterSlot()
    {
        foreach (var hunterSlot in _hunterSlots)
        {
            var asd = _targetPortal.HuntersToDispatch.Where(x => x == hunterSlot.Hunter).FirstOrDefault();
            hunterSlot.Enable = asd == null;
        }
    }
}
