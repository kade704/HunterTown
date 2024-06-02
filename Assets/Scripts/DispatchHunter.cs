using UnityEngine;

public class DispatchHunter : MonoBehaviour
{
    private Animator _animator;
    private AvatarCustomize _avatarCustomize;
    private Vector2 _startPosition;
    private Hunter _hunter;
    private bool _willDeath;
    private bool _isDeath;
    private int _incleaseHP;
    private int _incleaseDamage;

    public AvatarCustomize AvatarCustomize => _avatarCustomize;
    public Vector2 StartPosition => _startPosition;

    public bool WillDeath
    {
        get => _willDeath;
        set => _willDeath = value;
    }

    public bool IsDeath
    {
        get => _isDeath;
        set => _isDeath = value;
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

    public void SetFlip(bool value)
    {
        transform.localScale = new Vector3(value ? -1 : 1, 1, 1);
    }

    public void SetMovement(bool value)
    {
        _animator.SetFloat("RunState", value ? 1 : 0);
    }

    public void Die()
    {
        _animator.SetTrigger("Die");
    }

    public void Attack()
    {
        _animator.SetTrigger("Attack");
    }

    public void Respawn()
    {
        _animator.SetTrigger("Respawn");
    }
}
