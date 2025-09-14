using System.Collections;
using UnityEngine;

public class UILogo : MonoBehaviour
{
    private Vector2 _startPosition;

    private void Start()
    {
        _startPosition = transform.localPosition;
        StartCoroutine(FloatingRoutine());
    }

    private IEnumerator FloatingRoutine()
    {
        while (true)
        {
            transform.localPosition = _startPosition + new Vector2(0, Mathf.Sin(Time.time) * 10);
            yield return null;
        }
    }
}
