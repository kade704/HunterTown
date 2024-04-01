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
        var newPortal = ConstructionManager.Instance.SetConstruction(portal, new Vector2Int(3, 3)) as Portal;
        newPortal.DefaultPower = Random.Range(5, 15);
        newPortal.DefaultDanger = Random.Range(5, 15);

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
    }
}
