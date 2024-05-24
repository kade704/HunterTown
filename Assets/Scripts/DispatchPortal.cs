using System.Collections;
using UnityEngine;

public class DispatchPortal : MonoBehaviour
{
    [SerializeField] private Sprite[] _spriteFrames;

    private Vector2 _startPosition;
    private SpriteRenderer _portalRenderer;

    private void Awake()
    {
        _portalRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _startPosition = transform.localPosition;

        StartCoroutine(AnimateRoutine());
    }

    private IEnumerator AnimateRoutine()
    {
        var frameIndex = 0;
        while (true)
        {
            _portalRenderer.sprite = _spriteFrames[frameIndex];
            frameIndex = (frameIndex + 1) % _spriteFrames.Length;
            yield return new WaitForSeconds(0.3f);
        }
    }

    public IEnumerator LeaveRoutine()
    {
        yield return MotionUtil.MoveToRoutine(transform, (Vector2)transform.position - new Vector2(1, 0), 1);
    }

    public void MoveRight()
    {
        transform.localPosition = _startPosition;
    }

    public void MoveLeft()
    {
        transform.localPosition = new Vector2(-_startPosition.x, _startPosition.y);
    }
}
