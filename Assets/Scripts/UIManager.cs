using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    private UnityEvent<string, Construction> _onConstructionInteracted = new();

    public static UIManager Instance => _instance;
    public UnityEvent<string, Construction> OnConstructionInteracted => _onConstructionInteracted;

    private void Awake()
    {
        _instance = this;

        var moneyText = transform.Find("MoneyText").GetComponent<Text>();
        GameManager.Instance.OnMoneyChanged.AddListener((money) =>
        {
            moneyText.text = "예산: " + money.ToString() + "원";
        });
    }

    public static void ShowCanvasGroup(CanvasGroup group)
    {
        group.alpha = 1;
        group.blocksRaycasts = true;
        group.interactable = true;
    }

    public static void HideCanvasGroup(CanvasGroup group)
    {
        group.alpha = 0;
        group.blocksRaycasts = false;
        group.interactable = false;
    }

    public static GameObject[] GetUIObjectsOverPointer()
    {
        var eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> raysastResults = new();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults.Select(x => x.gameObject).ToArray();
    }

    public static T GetUIObjectTypeOverPointer<T>() where T : MonoBehaviour
    {
        var objects = GetUIObjectsOverPointer();
        if (objects.Length == 0) return null;

        var hunterSlot = objects
            .Where(x => x.GetComponent<T>() != null)
            .FirstOrDefault();
        if (!hunterSlot) return null;

        return hunterSlot.GetComponent<T>();
    }

    public static bool IsUIObjectOverPointer()
    {
        return GetUIObjectsOverPointer().Length > 0;
    }
}
