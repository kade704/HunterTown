using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    protected virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ShowPanel(float speed = 10)
    {
        StopAllCoroutines();
        StartCoroutine(IEnuShowPanel(speed));
    }

    private IEnumerator IEnuShowPanel(float speed = 10)
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
        while (_canvasGroup.alpha <= 1)
        {
            _canvasGroup.alpha += Time.deltaTime * speed;
            yield return null;
        }
        _canvasGroup.alpha = 1;
    }

    public void HidePanel(float speed = 10)
    {
        StopAllCoroutines();
        StartCoroutine(IEnuHidePanel(speed));
    }

    private IEnumerator IEnuHidePanel(float speed = 10)
    {
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
        while (_canvasGroup.alpha >= 0)
        {
            _canvasGroup.alpha -= Time.deltaTime * speed;
            yield return null;
        }
        _canvasGroup.alpha = 0;
    }
}
