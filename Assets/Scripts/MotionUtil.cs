using System.Collections;
using UnityEngine;

public class MotionUtil : MonoBehaviour
{
    public static IEnumerator MoveToRoutine(Transform transform, Vector2 target, float speed)
    {
        while (Vector2.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }

    public static IEnumerator MoveLinearRoutine(Transform transform, Vector2 target, float time)
    {
        float startTime = Time.time;
        Vector2 startPosition = transform.position;

        while (Time.time - startTime < time)
        {
            float t = (Time.time - startTime) / time;
            transform.position = Vector2.Lerp(startPosition, target, t);
            yield return null;
        }
    }

    public static IEnumerator MoveEaseInRoutine(Transform transform, Vector2 target, float time, float power = 2)
    {
        float startTime = Time.time;
        Vector2 startPosition = transform.position;

        while (Time.time - startTime < time)
        {
            float t = Time.time - startTime;
            t = Mathf.Clamp01(t / time);
            t = Mathf.Pow(t, power);

            transform.position = Vector2.Lerp(startPosition, target, t);
            yield return null;
        }
    }

    public static IEnumerator MoveEaseOutRoutine(Transform transform, Vector2 target, float time, float power = 2)
    {
        float startTime = Time.time;
        Vector2 startPosition = transform.position;

        while (Time.time - startTime < time)
        {
            float t = Time.time - startTime;
            t = Mathf.Clamp01(t / time);
            t = 1 - Mathf.Pow(1 - t, power);

            transform.position = Vector2.Lerp(startPosition, target, t);
            yield return null;
        }
    }
}
