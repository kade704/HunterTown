using System.Collections;
using UnityEngine;

public class PortalGenerator : MonoBehaviour
{
    int nextHour = 5;

    private void Start()
    {
        Timer.Instance.Hour.OnChanged.AddListener(() =>
        {
            var hour = Timer.Instance.Hour.total;
            if (hour >= nextHour)
            {
                nextHour = hour + Random.Range(50, 100);
                AddRandomPortal();
            }
        });
    }

    private void AddRandomPortal()
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

        var powerMin = 50f + (day * day / (25 * ((1 + 10) - (10 * 1.2f)))) * 0.8f;
        var powerMax = 50f + (day * day / (25 * ((1 + 10) - (10 * 1.2f)))) * 1.2f;

        var dangerMin = 15f + (day * day / (100 * ((1 + 10) - (10 * 1.2f)))) * 0.8f;
        var dangerMax = 15f + (day * day / (100 * ((1 + 10) - (10 * 1.2f)))) * 1.2f;

        var difficultyMin = 10 + (month * month + (10 * 0.5f));
        var difficultyMax = 10 + (month * month + 5 + (10 * 0.5f));

        newPortal.DefaultPower = Random.Range(powerMin, powerMax);
        newPortal.DefaultDanger = Random.Range(dangerMin, dangerMax);
        newPortal.DefaultDifficulty = Random.Range(difficultyMin, difficultyMax);

        UILogger.Instance.Log(UILogger.LogType.Info, $"({cellPos.x}, {cellPos.y})에 포탈이 생성되었습니다.");
    }
}
