using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class PortalGenerator : MonoBehaviour
{
    [AssetsOnly]
    [SerializeField]
    private Construction _portalPrefab;

    [AssetSelector]
    [SerializeField]
    private Ability[] _portalAbilities;

    private int _nextSpawnHour = 10;
    private ConstructionGridmap _constructionGridMap;

    public Ability[] PortalAbilities => _portalAbilities;

    private void Awake()
    {
        _constructionGridMap = FindObjectOfType<ConstructionGridmap>();
    }

    private void Start()
    {
        GameManager.Instance.GetSystem<TimeSystem>().Hour.OnChanged.AddListener(() =>
        {
            var hour = GameManager.Instance.GetSystem<TimeSystem>().Hour.Total;
            if (hour >= _nextSpawnHour)
            {
                _nextSpawnHour = hour + Random.Range(500, 1000);
                SpawnRandomPortal();
            }
        });
    }

    private void SpawnRandomPortal()
    {
        Vector2Int cellPos;
        do
        {
            cellPos = new Vector2Int(Random.Range(0, 32), Random.Range(0, 32));
        } while (!_constructionGridMap.CheckConstructionBuildable(_portalPrefab, cellPos));

        var newPortal = _constructionGridMap.BuildConstruction(_portalPrefab, cellPos).GetComponent<Portal>();

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

        GameManager.Instance.GetSystem<NotificationSystem>().NotifyWarning($"포탈이 <b>({cellPos.x}, {cellPos.y})</b>위치에 생성되었습니다.");
    }

    public void RemovePortal(Portal portal)
    {
        _constructionGridMap.DestroyConstruction(portal.GetComponent<Construction>());
    }
}
