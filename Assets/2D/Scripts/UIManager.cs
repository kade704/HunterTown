using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    private void Awake()
    {
        var moneyText = transform.Find("MoneyText").GetComponent<TMP_Text>();
        GameManager_.Instance.OnMoneyChanged.AddListener((money) =>
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

    public static bool IsPointerOverUI()
    {
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults.Count > 0;
    }
}
