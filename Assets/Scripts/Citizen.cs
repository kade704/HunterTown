using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Citizen : MonoBehaviour
{
    private AvatarMovement _walkable;
    private SortingGroup _sortingGroup;

    private void Awake()
    {
        _walkable = GetComponent<AvatarMovement>();
        _sortingGroup = GetComponent<SortingGroup>();
    }

    private void Start()
    {
        StartCoroutine(WanderRoutine());
    }

    private void Update()
    {
        _sortingGroup.sortingOrder = 300 - Mathf.FloorToInt(transform.position.y * 10) + 1;
    }

    private IEnumerator WanderRoutine()
    {
        var timeSystem = GameManager.Instance.GetSystem<TimeSystem>();
        var roads = FindObjectsOfType<Road>();
        var startRoad = roads[Random.Range(0, roads.Length)];

        var start = startRoad.Construction.CellPos;

        transform.position = startRoad.transform.position;

        while (true)
        {
            var moveTime = timeSystem.Hour.Total + Random.Range(5, 15);
            while (timeSystem.Hour.Total < moveTime)
            {
                yield return null;
            }

            roads = FindObjectsOfType<Road>();
            if (roads.Length == 0) continue;

            var end = roads[Random.Range(0, roads.Length)].Construction.CellPos;

            var path = PathFinder.SearchPath(start, end);

            yield return _walkable.MoveRoutine(path);

            start = end;
        }
    }

}
