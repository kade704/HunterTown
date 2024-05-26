using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class Monster : MonoBehaviour
{
    [SerializeField] private int _lifeHour;
    [SerializeField] private SpriteRenderer _progressRenderer;

    private int _startHour;
    private Durable _target;
    private AvatarMovement _avatarMovement;
    private Animator _animator;
    private bool _isDead = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _avatarMovement = GetComponent<AvatarMovement>();
    }

    private void Start()
    {
        StartCoroutine(DestructRoutine());

        _startHour = GameManager.Instance.GetSystem<TimeSystem>().Hour.Total;
    }

    private void Update()
    {
        var timeSystem = GameManager.Instance.GetSystem<TimeSystem>();
        var value = 1 - ((float)timeSystem.Hour.Total - _startHour) / _lifeHour;

        _progressRenderer.material.SetFloat("_Value", value);

        if (value <= 0 && !_isDead)
        {
            StopAllCoroutines();
            StartCoroutine(DeathRoutine());
        }
    }

    private IEnumerator DestructRoutine()
    {
        var constructionGridMap = GameManager.Instance.GetSystem<ConstructionGridmap>();
        var pathFinder = GameManager.Instance.GetSystem<PathFinder>();
        while (true)
        {
            _animator.SetFloat("RunState", 0.5f);

            _target = FindNearestDurable();
            if (!_target) yield break;

            var start = constructionGridMap.WorldToCell(transform.position);
            var path = pathFinder.SearchPath(start, _target.Construction.CellPos);
            yield return _avatarMovement.MoveRoutine(path, 3);

            _animator.SetFloat("RunState", 0);

            while (_target != null && _target.Durability > 0)
            {
                _animator.SetTrigger("Attack");
                _target.Durability -= 1;
                yield return new WaitForSeconds(1);
            }

            if (_target == null)
            {
                yield return new WaitForSeconds(1);
                continue;
            }

            constructionGridMap.DestroyConstruction(_target.Construction);
            GameManager.Instance.GetSystem<AudioController>().PlaySFX("Destruction");

            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator DeathRoutine()
    {
        _isDead = true;
        _animator.SetTrigger("Die");
        yield return new WaitForSeconds(2);
        GameManager.Instance.GetSystem<MonsterSpawner>().DestroyMonster(this);
    }

    private Durable FindNearestDurable()
    {
        var durables = FindObjectsOfType<Durable>();
        Durable nearestDurable = null;
        var minDistance = float.MaxValue;

        foreach (var durable in durables)
        {
            var distance = Vector2.Distance(transform.position, durable.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestDurable = durable;
            }
        }

        return nearestDurable;
    }
}
