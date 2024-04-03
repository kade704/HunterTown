using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    private UnityEvent<string, Construction> _onConstructionInteracted = new();

    public UnityEvent<string, Construction> OnConstructionInteracted => _onConstructionInteracted;

    protected override void Awake()
    {
        base.Awake();

        var moneyText = transform.Find("MoneyText").GetComponent<Text>();
        GameManager.Instance.OnMoneyChanged.AddListener((money) =>
        {
            moneyText.text = "예산: " + money.ToString() + "원";
        });
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
