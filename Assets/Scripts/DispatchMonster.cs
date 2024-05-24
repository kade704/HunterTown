using System.Collections;
using System.Linq;
using UnityEngine;

public class DispatchMonster : MonoBehaviour
{
    private Animator _animator;
    private AvatarCustomize _avatarCustomize;
    private Vector2 _startPosition;

    public Vector2 StartPosition => _startPosition;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _avatarCustomize = GetComponent<AvatarCustomize>();
    }

    void Start()
    {
        _startPosition = transform.position;
        RandomCustomize();
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
        RandomCustomize();
        _animator.SetTrigger("Respawn");
        yield return MotionUtil.MoveToRoutine(transform, _startPosition - new Vector2(1, 0), 1);
    }

    public IEnumerator LeaveRoutine()
    {
        yield return MotionUtil.MoveToRoutine(transform, _startPosition - new Vector2(4, 0), 1);
    }

    public void RandomCustomize()
    {
        var database = GameManager.Instance.GetSystem<CustomizeDatabase>();

        var monsterBody = database.BaseBodies.Where(x => x.id.Contains("monster")).ToArray();

        _avatarCustomize.BaseBody = monsterBody[Random.Range(0, monsterBody.Length)];
        _avatarCustomize.Eye = database.Eyes[Random.Range(0, database.Eyes.Length)];
        _avatarCustomize.EyeColor = Random.ColorHSV(0, 1, 0f, 0.4f, 0.8f, 1f);
        _avatarCustomize.Weapon = database.Weapons[Random.Range(0, database.Weapons.Length)];
    }

    public void Die()
    {
        print("asdasdasdad");
        _animator.SetTrigger("Die");
    }
}
