using System.Collections;
using System.Linq;
using UnityEngine;

public class PortalGenerator : MonoBehaviour
{
    private Ability[] _portalAbilities;
    private int _nextSpawnHour = 5;

    private void Awake()
    {
        _portalAbilities = Resources.LoadAll<Ability>("Abilities");
    }

    private void Start()
    {
        Timer.Instance.Hour.OnChanged.AddListener(() =>
        {
            var hour = Timer.Instance.Hour.total;
            if (hour >= _nextSpawnHour)
            {
                _nextSpawnHour = hour + Random.Range(50, 100);
                SpawnRandomPortal();
            }
        });
    }

    private void SpawnRandomPortal()
    {
        var portal = Resources.Load<Portal>("Constructions/Portal");

        Vector2Int cellPos;
        do
        {
            cellPos = new Vector2Int(Random.Range(-10, 10), Random.Range(-10, 10));
        } while (ConstructionManager.Instance.GetConstructionAt(cellPos) != null);


        var newPortal = ConstructionManager.Instance.BuildConstruction(portal, cellPos) as Portal;

        var day = Timer.Instance.Day.total;
        var month = Timer.Instance.Month.total;

        var difficulty = 5f;

        var powerMin = 50f + (day * day / (25 * ((1 + 10) - (difficulty * 1.2f)))) * 0.8f;
        var powerMax = 50f + (day * day / (25 * ((1 + 10) - (difficulty * 1.2f)))) * 1.2f;

        var dangerMin = 15f + (day * day / (100 * ((1 + 10) - (difficulty * 1.2f)))) * 0.8f;
        var dangerMax = 15f + (day * day / (100 * ((1 + 10) - (difficulty * 1.2f)))) * 1.2f;

        var difficultyMin = 10 + (month * month + (10 * 0.5f));
        var difficultyMax = 10 + (month * month + 5 + (10 * 0.5f));

        newPortal.Power = Random.Range(powerMin, powerMax);
        newPortal.Danger = Random.Range(dangerMin, dangerMax);
        newPortal.Difficulty = Random.Range(difficultyMin, difficultyMax);

        var abilities = _portalAbilities.ToList();
        for (int i = 0; i < 3; i++)
        {
            var randomIdx = Random.Range(-1, abilities.Count);
            if (randomIdx == -1)
            {
                newPortal.Abilities[i] = null;
            }
            else
            {
                newPortal.Abilities[i] = abilities[randomIdx];
                abilities.RemoveAt(randomIdx);
            }
        }

        UILogger.Instance.Log(UILogger.LogType.Warning, $"포탈이 <b>({cellPos.x}, {cellPos.y})</b>위치에 생성되었습니다.");
    }
}
