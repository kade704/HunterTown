using System.Collections;
using UnityEngine;

public class PortalGenerator : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(AddRandomPortalRoutine());
    }

    private IEnumerator AddRandomPortalRoutine()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            AddRandomPortal();
            yield return new WaitForSeconds(60);
        }
    }

    private void AddRandomPortal()
    {
        var portal = Resources.Load<Portal>("Constructions/Portal");

        Vector2Int cellPos;
        do
        {
            cellPos = new Vector2Int(Random.Range(-5, 5), Random.Range(-5, 5));
        } while (ConstructionManager.Instance.GetConstructionAt(cellPos) != null);


        var newPortal = ConstructionManager.Instance.BuildConstruction(portal, cellPos) as Portal;
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

        UILogger.Instance.Log(UILogger.LogType.Info, $"({cellPos.x}, {cellPos.y})에 포탈이 생성되었습니다.");
    }
}
