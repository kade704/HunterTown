using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ConstructionGridMap : MonoBehaviour, IDeserializable, ISerializable
{
    private Dictionary<Vector2Int, Construction> _constructionMap = new();
    private List<Construction> _constructions = new();
    private Construction _constructionSelected;
    private Grid _isometricGrid;
    private GridCursor _constructionCursor;


    private UnityEvent<Construction> _onConstructionClicked = new();
    private Construction[] _constructionPrefabs;
    private UnityEvent<Construction> _onConstructionBuilded = new();
    private UnityEvent<Construction> _onConstructionDestroyed = new();

    public Construction[] Constructions => _constructions.ToArray();
    public UnityEvent<Construction> OnConstructionBuilded => _onConstructionBuilded;
    public UnityEvent<Construction> OnConstructionDestroyed => _onConstructionDestroyed;


    public UnityEvent<Construction> OnConstructionClicked => _onConstructionClicked;

    public Construction GetConstructionPrefab(string id)
    {
        return _constructionPrefabs.Where(c => c.Id == id).FirstOrDefault();
    }

    private void Awake()
    {
        _constructionCursor = FindObjectOfType<GridCursor>();

        _constructionPrefabs = Resources.LoadAll<Construction>("Constructions");

        _isometricGrid = GetComponent<Grid>();
    }

    private void Start()
    {
        var json = Resources.Load<TextAsset>("Map").text;
        Deserialize(JToken.Parse(json));
    }

    private void Update()
    {
    }

    private Construction GetConstructionOverPointer()
    {
        var cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var colliders = Physics2D.OverlapPointAll(cursor);
        foreach (var collider in colliders)
        {
            var construction = collider.GetComponent<Construction>();
            if (construction) return construction;
        }

        return null;
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

        var size = newConstruction.Size;
        for (var y = 0; y < size.y; y++)
        {
            for (var x = 0; x < size.x; x++)
            {
                var pos = cellPos + new Vector2Int(x, y);
                _constructionMap[pos] = newConstruction;
            }
        }
        _constructions.Add(newConstruction);
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

        _onConstructionDestroyed.Invoke(construction);

        var size = construction.Size;
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                var pos = construction.CellPos + new Vector2Int(x, y);
                _constructionMap.Remove(pos);
            }
        }
        _constructions.Remove(construction);

        Destroy(construction.gameObject);
    }

    public void DestroyConstruction(Construction construction)
    {
        if (!construction)
        {
            Debug.LogError("Construction is null");
            return;
        }

        DestroyConstruction(construction.CellPos);
    }

    public bool CheckConstructionBuildable(Construction constructionPrefab, Vector2Int cellPos)
    {
        var size = constructionPrefab.Size;
        for (var y = 0; y < size.y; y++)
        {
            for (var x = 0; x < size.x; x++)
            {
                var pos = cellPos + new Vector2Int(x, y);
                if (_constructionMap.GetValueOrDefault(pos))
                    return false;
            }
        }

        return true;
    }

    public Construction GetConstructionAt(Vector2Int cellPos)
    {
        return _constructionMap.GetValueOrDefault(cellPos);
    }

    public bool IsConstructionExistAt(Vector2Int cellPos)
    {
        return GetConstructionAt(cellPos) != null;
    }

    public JToken Serialize()
    {
        var constructions = new JArray();
        foreach (var construction in _constructions)
        {
            var obj = new JObject
            {
                ["id"] = construction.Id,
                ["posX"] = construction.CellPos.x,
                ["posY"] = construction.CellPos.y
            };
            constructions.Add(obj);
        }
        return constructions;
    }

    public void Deserialize(JToken token)
    {
        foreach (var construction in token)
        {
            var name = construction["id"].Value<string>();
            var posX = construction["posX"].Value<int>();
            var posY = construction["posY"].Value<int>();
            var pos = new Vector2Int(posX, posY);
            var constructionPrefab = GetConstructionPrefab(name);
            if (!constructionPrefab)
            {
                Debug.LogError("Construction prefab not found: " + name);
                continue;
            }
            BuildConstruction(constructionPrefab, pos);
        }
    }
}
