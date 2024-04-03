using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    private List<Portal> _portals = new();

    private void Start()
    {
        AddRandomPortal();
    }

    public void AddRandomPortal()
    {
        var portal = Resources.Load<Portal>("Constructions/Portal");

        Vector2Int cellPos;
        do
        {
            cellPos = new Vector2Int(Random.Range(-5, 5), Random.Range(-5, 5));
        } while (ConstructionManager.Instance.GetConstruction(cellPos) != null);


        var newPortal = ConstructionManager.Instance.SetConstruction(portal, cellPos) as Portal;
        newPortal.DefaultPower = Random.Range(5, 15);
        newPortal.DefaultDanger = Random.Range(20, 100);

        var abilityCount = System.Enum.GetNames(typeof(Portal.Ability)).Length;
        for (int i = 0; i < 3; i++)
        {
            var choose = Random.Range(-1, abilityCount);
            if (choose != -1)
            {
                newPortal.Abilities.Add((Portal.Ability)choose);
            }
        }
        _portals.Add(newPortal);

        UILogger.Instance.LogInfo($"({cellPos.x}, {cellPos.y})에 포탈이 생성되었습니다.");
    }
}
