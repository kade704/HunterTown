using System.Collections;
using UnityEngine;

public class DispatchHunter : MonoBehaviour
{
    private Animator _animator;
    private AvatarCustomize _avatarCustomize;
    private Vector2 _startPosition;
    private Hunter _hunter;
    private bool _death;
    private int _incleaseHP;
    private int _incleaseDamage;

    public AvatarCustomize AvatarCustomize => _avatarCustomize;
    public Vector2 StartPosition => _startPosition;

    public bool Death
    {
        get => _death;
        set => _death = value;
    }

    public int IncleaseHP
    {
        get => _incleaseHP;
        set => _incleaseHP = value;
    }

    public int IncleaseDamage
    {
        get => _incleaseDamage;
        set => _incleaseDamage = value;
    }

    public Hunter Hunter
    {
        get => _hunter;
        set
        {
            _hunter = value;

            if (_hunter == null)
            {
                _avatarCustomize.HideAvatar();
            }
            else
            {
                _avatarCustomize.CopyAvatar(_hunter.GetComponent<AvatarCustomize>());
                _avatarCustomize.ShowAvatar();
            }
        }
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _avatarCustomize = GetComponent<AvatarCustomize>();
    }

    private void Start()
    {
        _startPosition = transform.position;
    }

    public void Initialize()
    {
        _death = false;
        _animator.SetFloat("RunState", 0);
        _animator.SetTrigger("Respawn");
        _avatarCustomize.HideAvatar();
    }

    public IEnumerator EnterPortalRoutine(Transform portal)
    {
        _animator.SetFloat("RunState", 0.5f);
        yield return MotionUtil.MoveToRoutine(transform, portal.position, 1);
        _animator.SetFloat("RunState", 0);
        _avatarCustomize.HideAvatar();
    }

    public IEnumerator ExitPortalRoutine()
    {
        _avatarCustomize.ShowAvatar();
        _animator.SetFloat("RunState", 0.5f);
        yield return MotionUtil.MoveToRoutine(transform, _startPosition, 1);
        _animator.SetFloat("RunState", 0);
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

    public void SetMovement(bool value)
    {
        _animator.SetFloat("RunState", value ? 0.5f : 0);
    }

    public void Die()
    {
        _animator.SetTrigger("Die");
    }
}
