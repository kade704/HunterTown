using System.Collections;
using UnityEngine;

public class EmployHunter : MonoBehaviour
{
    private AvatarCustomize _avatarCustomize;
    private Animator _animator;
    private Vector2 _startPosition;
    private string _name;
    private int _hp;
    private int _damage;

    public AvatarCustomize AvatarCustomize => _avatarCustomize;
    public string Name
    {
        get => _name;
        set => _name = value;
    }
    public int HP
    {
        get => _hp;
        set => _hp = value;
    }
    public int Damage
    {
        get => _damage;
        set => _damage = value;
    }

    private void Awake()
    {
        _avatarCustomize = GetComponent<AvatarCustomize>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _startPosition = transform.position;
    }

    public IEnumerator EnterRoutine()
    {
        transform.position = new Vector2(-2.5f, 30);
        _animator.SetFloat("RunState", 0.5f);
        yield return MotionUtil.MoveToRoutine(transform, _startPosition, 1);
        _animator.SetFloat("RunState", 0);
    }

    public IEnumerator ExitRoutine()
    {
        _animator.SetFloat("RunState", 0.5f);
        yield return MotionUtil.MoveToRoutine(transform, new Vector2(2.5f, 30), 1);
        _animator.SetFloat("RunState", 0);
    }
}
