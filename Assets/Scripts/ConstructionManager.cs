using UnityEngine;
using UnityEngine.Events;

public class ConstructionManager : MonoBehaviour
{
    private static ConstructionManager _instance;
    private Construction _constructionSelected;
    private ConstructionEditor _constructionEditor;
    private RoadMap _roadMap;
    private BuildingMap _buildingMap;
    private Grid _isometricGrid;
    private UnityEvent<Construction> _onConstructionClicked = new();

    public static ConstructionManager Instance => _instance;
    public RoadMap RoadMap => _roadMap;
    public BuildingMap BuildingMap => _buildingMap;

    public UnityEvent<Construction> OnConstructionClicked => _onConstructionClicked;

    private void Awake()
    {
        _instance = this;

        _isometricGrid = GetComponent<Grid>();
        _constructionEditor = FindObjectOfType<ConstructionEditor>();
        _roadMap = transform.Find("RoadMap").GetComponent<RoadMap>();
        _buildingMap = transform.Find("BuildingMap").GetComponent<BuildingMap>();
    }

    private void Start()
    {
        var house1 = Resources.Load<Building>("Constructions/House1");
        var house2 = Resources.Load<Building>("Constructions/House2");
        var road = Resources.Load<Road>("Constructions/Road");
        var portal = Resources.Load<Portal>("Constructions/Portal");

        _roadMap.Set(road, new Vector2Int(-3, -1));
        _roadMap.Set(road, new Vector2Int(-2, -1));
        _roadMap.Set(road, new Vector2Int(-1, -1));
        _roadMap.Set(road, new Vector2Int(0, -1));
        _roadMap.Set(road, new Vector2Int(1, -1));
        _roadMap.Set(road, new Vector2Int(-1, -3));
        _roadMap.Set(road, new Vector2Int(-1, -2));
        _roadMap.Set(road, new Vector2Int(-1, 0));
        _roadMap.Set(road, new Vector2Int(-1, 1));
        _buildingMap.Set(house1, new Vector2Int(0, 0));
        _buildingMap.Set(house2, new Vector2Int(0, -2));
        var newPortal = _buildingMap.Set(portal, new Vector2Int(3, 3)) as Portal;
        newPortal.DefaultPower = 10;

    }

    private void Update()
    {
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
        return _isometricGrid.CellToWorld(pos);
    }

    public Vector2Int WorldToCell(Vector2 worldPos)
    {
        var cellPos = _isometricGrid.WorldToCell(worldPos);
        return new Vector2Int(cellPos.x, cellPos.y);
    }

    public Construction SetConstruction(Construction construction, Vector2Int cellPos)
    {
        if (construction is Building)
        {
            return _buildingMap.Set(construction, cellPos);
        }
        else if (construction is Road)
        {
            return _roadMap.Set(construction, cellPos);
        }

        return null;
    }

    public void RemoveConstruction(Vector2Int cellPos)
    {
        _buildingMap.Remove(cellPos);
        _roadMap.Remove(cellPos);
    }

    public Construction GetConstruction(Vector2Int cellPos)
    {
        Construction result;

        result = _buildingMap.Get(cellPos);
        if (result) return result;

        result = _roadMap.Get(cellPos);
        if (result) return result;

        return null;
    }

    public bool ExistConstruction(Vector2Int cellPos)
    {
        var roadExist = _roadMap.Exist(cellPos);
        var buildingExist = _buildingMap.Exist(cellPos);
        return roadExist || buildingExist;
    }
}
