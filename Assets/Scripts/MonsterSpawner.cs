using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private Monster _monsterPrefab;

    private List<Monster> _monsters = new();

    public Monster[] Monsters => _monsters.ToArray();

    public Monster SpawnMonster(Vector2Int cellPos)
    {
        var worldPos = GameManager.Instance.GetSystem<ConstructionGridmap>().CellToWorld(cellPos);
        var newMonster = Instantiate(_monsterPrefab, worldPos, Quaternion.identity, transform);
        _monsters.Add(newMonster);

        return newMonster;
    }

    public void DestroyMonster(Monster monster)
    {
        _monsters.Remove(monster);
        Destroy(monster.gameObject);
    }


}
