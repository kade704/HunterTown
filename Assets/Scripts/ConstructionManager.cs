using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ConstructionManager : MonoBehaviour, IDeserializable, ISerializable
{
    private static ConstructionManager _instance;
    private Dictionary<Vector2Int, Construction> _constructionMap = new();
    private List<Construction> _constructions = new();
    private Construction _constructionSelected;
    private ConstructionEditor _constructionEditor;
    private Grid _isometricGrid;
    private UnityEvent<Construction> _onConstructionClicked = new();
    private Construction[] _constructionPrefabs;
    private UnityEvent _onConstructionBuilded = new();
    private UnityEvent _onConstructionDestroyed = new();
    private SpriteRenderer _cellCursor;

    public static ConstructionManager Instance => _instance;
    public Construction[] Constructions => _constructions.ToArray();
    public UnityEvent OnConstructionBuilded => _onConstructionBuilded;
    public UnityEvent OnConstructionDestroyed => _onConstructionDestroyed;


    public UnityEvent<Construction> OnConstructionClicked => _onConstructionClicked;

    public Construction GetConstructionPrefab(string id)
    {
        return _constructionPrefabs.Where(c => c.Id == id).FirstOrDefault();
    }

    private void Awake()
    {
        _instance = this;

        _constructionPrefabs = Resources.LoadAll<Construction>("Constructions");

        _isometricGrid = GetComponent<Grid>();
        _constructionEditor = FindObjectOfType<ConstructionEditor>();

        _cellCursor = GameObject.Find("CellCursor").GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        var json = Resources.Load<TextAsset>("Map").text;
        Deserialize(JToken.Parse(json));
    }

    private void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var cellPos = WorldToCell(mousePos);
        _cellCursor.transform.position = CellToWorld(cellPos);

        if (Input.GetMouseButtonDown(0) && !_constructionEditor.IsEditing)
        {
            if (UIManager.IsUIObjectOverPointer()) return;

            _constructionSelected = GetConstructionOverPointer();
            OnConstructionClicked.Invoke(_constructionSelected);

            if (_constructionSelected)
            {
                _constructionSelected.OnClicked.Invoke();
            }
        }
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
        _onConstructionBuilded.Invoke();

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

        _onConstructionDestroyed.Invoke();
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

    public bool IsRoadExistAt(Vector2Int cellPos)
    {
        return GetConstructionAt(cellPos) is Road;
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
}
