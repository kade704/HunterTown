using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class ConstructionGridmap : MonoBehaviour, IDeserializable, ISerializable
{
    public const int GRID_SIZE = 32;
    private Construction[,] _constructionMap = new Construction[GRID_SIZE, GRID_SIZE];
    private List<Construction> _constructions = new();
    private Grid _isometricGrid;

    private UnityEvent<Construction> _onConstructionBuilded = new();
    private UnityEvent<Construction> _onConstructionDestroyed = new();

    public Construction[] Constructions => _constructions.ToArray();
    public UnityEvent<Construction> OnConstructionBuilded => _onConstructionBuilded;
    public UnityEvent<Construction> OnConstructionDestroyed => _onConstructionDestroyed;


    private void Awake()
    {
        _isometricGrid = GetComponent<Grid>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            var data = Serialize();
        }
    }

    public Vector2 CellToWorld(Vector2Int cellPos)
    {
        var pos = new Vector3Int(cellPos.x, cellPos.y, 0);
        return _isometricGrid.GetCellCenterWorld(pos);
    }

    public Vector2Int WorldToCell(Vector2 worldPos)
    {
        var cellPos = _isometricGrid.WorldToCell(worldPos);
        return new Vector2Int(cellPos.x, cellPos.y);
    }

    public Construction BuildConstruction(Construction constructionPrefab, Vector2Int cellPos)
    {
        if (!constructionPrefab)
        {
            Debug.LogError("Construction is null");
            return null;
        };

        if (!CheckConstructionBuildable(constructionPrefab, cellPos))
        {
            Debug.LogError("Construction not buildable at this position: " + cellPos);
            return null;
        }

        var worldPos = CellToWorld(cellPos);
        var newConstruction = Instantiate(constructionPrefab, worldPos, Quaternion.identity, transform);
        newConstruction.CellPos = cellPos;
        newConstruction.ConstructionGridMap = this;

        for (var y = 0; y < newConstruction.Size; y++)
        {
            for (var x = 0; x < newConstruction.Size; x++)
            {
                var pos = cellPos + new Vector2Int(x, y);
                _constructionMap[pos.y, pos.x] = newConstruction;
            }
        }
        _constructions.Add(newConstruction);

        newConstruction.OnBuilded.Invoke();
        _onConstructionBuilded.Invoke(newConstruction);

        return newConstruction;
    }

    public void DestroyConstruction(Vector2Int cellPos)
    {
        var construction = GetConstructionAt(cellPos);
        if (!construction)
        {
            Debug.LogError("Construction not found at this position: " + cellPos);
            return;
        }

        for (int y = 0; y < construction.Size; y++)
        {
            for (int x = 0; x < construction.Size; x++)
            {
                var pos = construction.CellPos + new Vector2Int(x, y);
                _constructionMap[pos.y, pos.x] = null;
            }
        }
        _constructions.Remove(construction);

        construction.OnDestroyed.Invoke();
        _onConstructionDestroyed.Invoke(construction);

        Destroy(construction.gameObject);
    }

    public void DestroyConstruction(Construction construction)
    {
        if (construction == null)
        {
            Debug.LogError("Construction is null");
            return;
        }

        DestroyConstruction(construction.CellPos);
    }

    public bool CheckConstructionBuildable(Construction constructionPrefab, Vector2Int cellPos)
    {
        for (var y = 0; y < constructionPrefab.Size; y++)
        {
            for (var x = 0; x < constructionPrefab.Size; x++)
            {
                var pos = cellPos + new Vector2Int(x, y);
                if (pos.x < 0 || pos.x >= GRID_SIZE || pos.y < 0 || pos.y >= GRID_SIZE)
                {
                    return false;
                }
                if (_constructionMap[pos.y, pos.x] != null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public Construction GetConstructionAt(Vector2Int cellPos)
    {
        if (cellPos.x < 0 || cellPos.x >= GRID_SIZE || cellPos.y < 0 || cellPos.y >= GRID_SIZE)
        {
            return null;
        }

        return _constructionMap[cellPos.y, cellPos.x];
    }

    public bool IsConstructionExistAt(Vector2Int cellPos)
    {
        return GetConstructionAt(cellPos) != null;
    }

    public JToken Serialize()
    {
        var data = new JArray();
        foreach (var construction in _constructions)
        {
            var obj = new JObject
            {
                ["id"] = construction.ID,
                ["posX"] = construction.CellPos.x,
                ["posY"] = construction.CellPos.y
            };

            var serializables = construction.GetComponents<ISerializable>();
            foreach (var serializable in serializables)
            {
                obj[serializable.GetType().Name] = serializable.Serialize();
            }

            data.Add(obj);
        }
        return data;
    }

    public void Deserialize(JToken token)
    {
        foreach (var construction in _constructions)
        {
            Destroy(construction.gameObject);
        }

        _constructionMap = new Construction[GRID_SIZE, GRID_SIZE];

        foreach (var data in token)
        {
            var id = data["id"].Value<string>();
            var posX = data["posX"].Value<int>();
            var posY = data["posY"].Value<int>();
            var pos = new Vector2Int(posX, posY);
            var constructionPrefab = GameManager.Instance.GetSystem<ConstructionDatabase>().GetConstructionPrefab(id);
            if (!constructionPrefab)
            {
                Debug.LogError("Construction prefab not found: " + id);
                continue;
            }

            var construction = BuildConstruction(constructionPrefab, pos);
            if (!construction)
            {
                Debug.LogError("Construction not buildable at this position: " + pos);
                continue;
            }

            var deserializables = construction.GetComponents<IDeserializable>();
            foreach (var deserializable in deserializables)
            {
                deserializable.Deserialize(data[deserializable.GetType().Name]);
            }
        }
    }
}
