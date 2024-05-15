using System.Collections;
using UnityEngine;

public class BattleMonster : MonoBehaviour
{
    private Animator _animator;
    private AvatarCustomize _avatarCustomize;
    private Vector2 _startPosition;

    public AvatarCustomize AvatarCustomize => _avatarCustomize;
    public Vector2 StartPosition => _startPosition;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _avatarCustomize = GetComponent<AvatarCustomize>();
    }

    void Start()
    {
        _startPosition = transform.position;
    }

    public void Initialize()
    {
        _animator.SetFloat("RunState", 0);
        _animator.SetTrigger("Respawn");
        _avatarCustomize.HideAvatar();
    }

    public IEnumerator AttackBeginRoutine(Vector2 position)
    {
        _animator.SetFloat("RunState", 0.5f);
        yield return MotionUtil.MoveToRoutine(transform, position, 3f);

        _animator.SetFloat("RunState", 0.5f);
        _animator.SetTrigger("Attack");
    }

    public IEnumerator AttackEndRoutine()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        _animator.SetFloat("RunState", 0.5f);
        yield return MotionUtil.MoveToRoutine(transform, _startPosition, 3f);
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        _animator.SetFloat("RunState", 0);
    }

    public IEnumerator RespawnRoutine()
    {
        _animator.SetTrigger("Respawn");
        transform.position = _startPosition + new Vector2(1, 0);
        _animator.SetFloat("RunState", 0.5f);
        yield return MotionUtil.MoveToRoutine(transform, _startPosition, 1);
        _animator.SetFloat("RunState", 0);
    }

    public void Death()
    {
        _animator.SetTrigger("Die");
    }
}
