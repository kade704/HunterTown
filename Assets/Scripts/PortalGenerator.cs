using System.Collections;
using System.Linq;
using UnityEngine;

public class PortalGenerator : MonoBehaviour
{
    private Ability[] _portalAbilities;
    private int _nextSpawnHour = 5;
    private ConstructionGridmap _constructionGridMap;

    private void Awake()
    {
        _portalAbilities = Resources.LoadAll<Ability>("Abilities");
        _constructionGridMap = FindObjectOfType<ConstructionGridmap>();
    }

    private void Start()
    {
        GameManager.Instance.GetSystem<TimeSystem>().Hour.OnChanged.AddListener(() =>
        {
            var hour = GameManager.Instance.GetSystem<TimeSystem>().Hour.Total;
            if (hour >= _nextSpawnHour)
            {
                _nextSpawnHour = hour + Random.Range(50, 100);
                SpawnRandomPortal();
            }
        });
    }

    private void SpawnRandomPortal()
    {
        var portalPrefab = Resources.Load<Construction>("Constructions/Portal");

        Vector2Int cellPos;
        do
        {
            cellPos = new Vector2Int(Random.Range(-10, 10), Random.Range(-10, 10));
        } while (_constructionGridMap.GetConstructionAt(cellPos) != null);

        var newPortal = _constructionGridMap.BuildConstruction(portalPrefab, cellPos).GetComponent<Portal>();

        var day = GameManager.Instance.GetSystem<TimeSystem>().Day.Total;
        var month = GameManager.Instance.GetSystem<TimeSystem>().Month.Total;

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

        GameManager.Instance.GetSystem<LoggerSystem>().LogWarning($"포탈이 <b>({cellPos.x}, {cellPos.y})</b>위치에 생성되었습니다.");
    }
}
