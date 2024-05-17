using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIUtil
{
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

    public static void ShowCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public static void HideCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public static bool IsCanvasGroupVisible(CanvasGroup canvasGroup)
    {
        return canvasGroup.alpha == 1;
    }
}
