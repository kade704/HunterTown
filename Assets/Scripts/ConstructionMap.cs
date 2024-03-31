using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ConstructionMap : MonoBehaviour
{
    private Dictionary<Vector2Int, Construction> _constructionBuilded = new();
    private UnityEvent _onConstructionSet = new();
    private UnityEvent _onConstructionRemoved = new();

    public Construction[] Constructions => _constructionBuilded.Values.ToArray();
    public UnityEvent OnConstructionSet => _onConstructionSet;
    public UnityEvent OnConstructionRemoved => _onConstructionRemoved;


    public virtual Construction Get(Vector2Int cellPos)
    {
        return _constructionBuilded.GetValueOrDefault(cellPos);
    }

    public virtual Construction Set(Construction construction, Vector2Int cellPos)
    {
        if (!construction) return null;

        if (Exist(cellPos)) return null;

        var worldPos = ConstructionManager.Instance.CellToWorld(cellPos);
        worldPos.y += 0.25f;

        var newConstruction = Instantiate(construction, worldPos, Quaternion.identity, transform);
        newConstruction.CellPos = cellPos;

        _constructionBuilded[cellPos] = newConstruction;

        _onConstructionSet.Invoke();

        return newConstruction;
    }

    public virtual void Remove(Vector2Int cellPos)
    {
        var construction = _constructionBuilded.GetValueOrDefault(cellPos);
        if (construction)
        {
            Destroy(construction.gameObject);
            _constructionBuilded.Remove(cellPos);

            _onConstructionRemoved.Invoke();
        }
    }

    public virtual bool Exist(Vector2Int cellPos)
    {
        return Get(cellPos);
    }
}
