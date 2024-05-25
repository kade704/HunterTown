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
}
