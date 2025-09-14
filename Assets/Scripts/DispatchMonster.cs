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

    public void RandomCustomize()
    {
        var database = GameManager.Instance.GetSystem<CustomizeDatabase>();

        var monsterBody = database.BaseBodies.Where(x => x.id.Contains("monster")).ToArray();

        _avatarCustomize.BaseBody = monsterBody[Random.Range(0, monsterBody.Length)];
        _avatarCustomize.Eye = database.Eyes[Random.Range(0, database.Eyes.Length)];
        _avatarCustomize.EyeColor = Random.ColorHSV(0, 1, 0f, 0.4f, 0.8f, 1f);
        _avatarCustomize.Weapon = database.Weapons[Random.Range(0, database.Weapons.Length)];
    }

    public void SetMovement(bool value)
    {
        _animator.SetFloat("RunState", value ? 1 : 0);
    }

    public void SetFlip(bool value)
    {
        transform.localScale = new Vector3(value ? -1 : 1, 1, 1);
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
