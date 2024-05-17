using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private Building _target;
    private AvatarMovement _avatarMovement;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _avatarMovement = GetComponent<AvatarMovement>();
    }

    private void Start()
    {
        StartCoroutine(DestructRoutine());
    }

    private IEnumerator DestructRoutine()
    {
        _animator.SetFloat("RunState", 0.5f);

        _target = FindNearestBuilding();
        var start = GameManager.Instance.GetSystem<ConstructionGridmap>().WorldToCell(transform.position);
        var path = PathFinder.SearchPath(start, _target.Construction.CellPos);
        yield return _avatarMovement.MoveRoutine(path);

        _animator.SetFloat("RunState", 0);
    }

    private Building FindNearestBuilding()
    {
        var buildings = FindObjectsOfType<Building>();
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
