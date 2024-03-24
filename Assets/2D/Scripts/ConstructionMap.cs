using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ConstructionMap : MonoBehaviour {

    private Dictionary<Vector2Int, Construction> _constructionBuilded = new();
    private UnityEvent _onConstructionSet = new();
    private UnityEvent _onConstructionRemoved = new();

    public Construction[] Constructions => _constructionBuilded.Values.ToArray();
    public UnityEvent OnConstructionSet => _onConstructionSet;
    public UnityEvent OnConstructionRemoved => _onConstructionRemoved;


    public Construction GetConstruction(Vector2Int cellPos) {
        return _constructionBuilded.GetValueOrDefault(cellPos);
    }

    public Construction SetConstruction(Construction construction, Vector2Int cellPos) {
        if (HasConstruction(cellPos)) return null;

        var worldPos = ConstructionManager.Instance.CellToWorld(cellPos);
        worldPos.y += 0.25f;

        var newConstruction = Instantiate(construction, worldPos, Quaternion.identity, transform);
        newConstruction.CellPos = cellPos;

        _constructionBuilded[cellPos] = newConstruction;

        _onConstructionSet.Invoke();

        return newConstruction;
    }

    public void RemoveConstruction(Vector2Int cellPos) {
        var construction = _constructionBuilded[cellPos];
        if (construction) {
            Destroy(construction.gameObject);
            _constructionBuilded.Remove(cellPos);

            _onConstructionRemoved.Invoke();
        }
    }

    public bool HasConstruction(Vector2Int cellPos) {
        return GetConstruction(cellPos);
    }
}
