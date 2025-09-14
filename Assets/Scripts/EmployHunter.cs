using System.Collections;
using System.Linq;
using UnityEngine;

public class EmployHunter : MonoBehaviour
{
    private AvatarCustomize _avatarCustomize;
    private Animator _animator;
    private Vector2 _startPosition;
    private string _name;
    private string _description;
    private int _hp;
    private int _damage;

    public AvatarCustomize AvatarCustomize => _avatarCustomize;
    public string Name
    {
        get => _name;
        set => _name = value;
    }
    public string Description
    {
        get => _description;
        set => _description = value;
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

    public void RandomCustomize()
    {
        var database = GameManager.Instance.GetSystem<CustomizeDatabase>();

        var humanBody = database.BaseBodies.Where(x => x.id.Contains("human")).ToArray();

        _avatarCustomize.BaseBody = humanBody[Random.Range(0, humanBody.Length)];
        _avatarCustomize.TopCloth = database.TopCloths[Random.Range(0, database.TopCloths.Length)];
        _avatarCustomize.BottomCloth = database.BottomCloths[Random.Range(0, database.BottomCloths.Length)];
        _avatarCustomize.Armor = database.Armors[Random.Range(0, database.Armors.Length)];
        _avatarCustomize.Helmet = database.Helmets[Random.Range(0, database.Helmets.Length)];
        _avatarCustomize.Eye = database.Eyes[Random.Range(0, database.Eyes.Length)];
        _avatarCustomize.EyeColor = Random.ColorHSV(0, 1, 0, 1, 0, 1);
        _avatarCustomize.Weapon = database.Weapons[Random.Range(0, database.Weapons.Length)];
    }

    public IEnumerator EnterRoutine()
    {
        transform.position = new Vector2(-2.5f, 30);
        _animator.SetFloat("RunState", 1);
        yield return MotionUtil.MoveToRoutine(transform, _startPosition, 2);
        _animator.SetFloat("RunState", 0);
    }

    public IEnumerator ExitRoutine()
    {
        _animator.SetFloat("RunState", 1);
        yield return MotionUtil.MoveToRoutine(transform, new Vector2(2.5f, 30), 2);
        _animator.SetFloat("RunState", 0);
    }
}
