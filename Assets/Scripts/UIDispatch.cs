using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIDispatch : MonoBehaviour
{
    private Portal _targetPortal;
    private CanvasGroup _canvasGroup;
    private Button _closeButton;
    private UIHunterSlot _hunterSlotRef;
    private Transform _hunterSlotParent;
    private Image _cursor;
    private Hunter _draggingHunter;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _hunterSlotRef = transform.Find("HunterSlotRef").GetComponent<UIHunterSlot>();
        _hunterSlotParent = transform.Find("HunterSlots");
        _cursor = transform.Find("Cursor").GetComponent<Image>();
        _closeButton = transform.Find("CloseButton").GetComponent<Button>();
        _closeButton.onClick.AddListener(() =>
        {
            UIManager.HideCanvasGroup(_canvasGroup);
        });

        UIManager.Instance.OnConstructionInteracted.AddListener((id, construction) =>
        {
            if (id == "dispatch")
            {
                _targetPortal = construction as Portal;

                foreach (Transform child in _hunterSlotParent)
                {
                    Destroy(child.gameObject);
                }

                foreach (var hunter in HunterManager.Instance.Hunters)
                {
                    var hunterSlot = Instantiate(_hunterSlotRef, _hunterSlotParent);
                    hunterSlot.Hunter = hunter;
                }

                UIManager.ShowCanvasGroup(_canvasGroup);
            }
            else
            {
                UIManager.HideCanvasGroup(_canvasGroup);
            }
        });
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var clickedHunterSlot = UIManager.GetUIObjectTypeOverPointer<UIHunterSlot>();
            if (clickedHunterSlot)
            {
                _cursor.sprite = clickedHunterSlot.Hunter.Sprite;
                _draggingHunter = clickedHunterSlot.Hunter;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            var droppedDispatchSlot = UIManager.GetUIObjectTypeOverPointer<UIDispatchSlot>();
            if (droppedDispatchSlot && _draggingHunter)
            {
                droppedDispatchSlot.Hunter = _draggingHunter;
            }

            _draggingHunter = null;
            _cursor.sprite = null;
            _cursor.transform.position = new Vector2(999, 999);
        }
        else if (Input.GetMouseButton(0))
        {
            if (_draggingHunter)
            {
                _cursor.transform.position = Input.mousePosition;
            }
        }
    }
}
