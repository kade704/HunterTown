using UnityEngine;
using UnityEngine.Events;

public class ConstructionManager : MonoBehaviour {
    private static ConstructionManager _instance;
    private Construction _constructionSelected;
    private ConstructionEditor _constructionEditor;
    private ConstructionMap _roadMap;
    private ConstructionMap _buildingMap;
    private Grid _isometricGrid;
    private UnityEvent<Construction> _onConstructionClicked = new();

    public static ConstructionManager Instance => _instance;
    public ConstructionMap RoadMap => _roadMap;
    public ConstructionMap BuildingMap => _buildingMap;

    public UnityEvent<Construction> OnConstructionClicked => _onConstructionClicked;

    private void Awake() {
        _instance = this;

        _isometricGrid = GetComponent<Grid>();
        _constructionEditor = FindObjectOfType<ConstructionEditor>();
        _roadMap = transform.Find("Roads").GetComponent<ConstructionMap>();
        _buildingMap = transform.Find("Buildings").GetComponent<ConstructionMap>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && !_constructionEditor.IsEditing) {
            if (UIManager.IsPointerOverUI()) return;

            _constructionSelected = GetConstructionOverPointer();
            OnConstructionClicked.Invoke(_constructionSelected);
        }
    }

    private Construction GetConstructionOverPointer() {
        var cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var colliders = Physics2D.OverlapPointAll(cursor);
        foreach (var collider in colliders) {
            var construction = collider.GetComponent<Construction>();
            if (construction) return construction;
        }

        return null;
    }

    public Vector2 CellToWorld(Vector2Int cellPos) {
        var pos = new Vector3Int(cellPos.x, cellPos.y, 0);
        return _isometricGrid.CellToWorld(pos);
    }

    public Vector2Int WorldToCell(Vector2 worldPos) {
        var cellPos = _isometricGrid.WorldToCell(worldPos);
        return new Vector2Int(cellPos.x, cellPos.y);
    }

    public bool HasConstruction(Vector2Int cellPos) {
        var roadHas = _roadMap.HasConstruction(cellPos);
        var buildingHas = _buildingMap.HasConstruction(cellPos);
        return roadHas || buildingHas;
    }
}
