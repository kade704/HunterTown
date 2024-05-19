using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class Monster : MonoBehaviour
{
    [SerializeField] private int _lifeHour;
    [SerializeField] private SpriteRenderer _progressRenderer;

    private int _startHour;
    private Building _target;
    private AvatarMovement _avatarMovement;
    private Animator _animator;
    private SortingGroup _sortingGroup;
    private bool _isDead = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _sortingGroup = GetComponent<SortingGroup>();
        _avatarMovement = GetComponent<AvatarMovement>();
    }

    private void Start()
    {
        StartCoroutine(DestructRoutine());

        _startHour = GameManager.Instance.GetSystem<TimeSystem>().Hour.Total;
    }

    private void Update()
    {
        _sortingGroup.sortingOrder = 300 - Mathf.FloorToInt(transform.position.y * 10) + 1;

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

            _target = FindNearestBuilding();
            if (!_target) yield break;

            var start = constructionGridMap.WorldToCell(transform.position);
            var path = pathFinder.SearchPath(start, _target.Construction.CellPos);
            yield return _avatarMovement.MoveRoutine(path, 3);

            _animator.SetFloat("RunState", 0);

            while (_target != null && _target.Construction.Durability > 0)
            {
                _animator.SetTrigger("Attack");
                _target.Construction.Durability -= 1;
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

    private Building FindNearestBuilding()
    {
        var buildings = FindObjectsOfType<Building>().Where(building => building.GetComponent<Portal>() == null).ToArray();
        Building nearestBuilding = null;
        var minDistance = float.MaxValue;

        foreach (var building in buildings)
        {
            var distance = Vector2.Distance(transform.position, building.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestBuilding = building;
            }
        }

        return nearestBuilding;
    }
}
