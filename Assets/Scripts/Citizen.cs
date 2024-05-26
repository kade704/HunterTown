using System.Collections;
using System.Linq;
using UnityEngine;

public class Citizen : MonoBehaviour
{
    private AvatarMovement _avatarMovement;
    private AvatarCustomize _avatarCustomize;

    private void Awake()
    {
        _avatarMovement = GetComponent<AvatarMovement>();
        _avatarCustomize = GetComponent<AvatarCustomize>();
    }

    private void Start()
    {
        RandomCustomize();
        StartCoroutine(WanderRoutine());
    }

    public void RandomCustomize()
    {
        var database = GameManager.Instance.GetSystem<CustomizeDatabase>();

        var humanBody = database.BaseBodies.Where(x => x.id.Contains("human")).ToArray();

        _avatarCustomize.BaseBody = humanBody[Random.Range(0, humanBody.Length)];
        _avatarCustomize.Hair = database.Hairs[Random.Range(0, database.Hairs.Length)];
        _avatarCustomize.HairColor = Random.ColorHSV(0, 1, 0, 1, 0, 1);
        _avatarCustomize.TopCloth = database.TopCloths[Random.Range(0, database.TopCloths.Length)];
        _avatarCustomize.BottomCloth = database.BottomCloths[Random.Range(0, database.BottomCloths.Length)];
        _avatarCustomize.Eye = database.Eyes[Random.Range(0, database.Eyes.Length)];
        _avatarCustomize.EyeColor = Random.ColorHSV(0, 1, 0, 1, 0, 1);
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

            var path = GameManager.Instance.GetSystem<PathFinder>().SearchPath(start, end);

            yield return _avatarMovement.MoveRoutine(path);

            start = end;
        }
    }

}
