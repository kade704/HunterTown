using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIFade : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private bool _isFadedIn = false;

    public bool IsFadedIn => _isFadedIn;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn(float speed = 10)
    {
        StopAllCoroutines();
        StartCoroutine(FadeInRoutine(speed));
    }

    private IEnumerator FadeInRoutine(float speed = 10)
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
        while (_canvasGroup.alpha < 1)
        {
            _canvasGroup.alpha += Time.deltaTime * speed;
            yield return null;
        }
        _canvasGroup.alpha = 1;
        _isFadedIn = true;
    }

    public void FadeOut(float speed = 10)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutRoutine(speed));
    }

    private IEnumerator FadeOutRoutine(float speed = 10)
    {
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
        while (_canvasGroup.alpha > 0)
        {
            _canvasGroup.alpha -= Time.deltaTime * speed;
            yield return null;
        }
        _canvasGroup.alpha = 0;
        _isFadedIn = false;
    }
}
