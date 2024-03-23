using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConstructionManager : MonoBehaviour {
    private static ConstructionManager _instance;
    private Tilemap _roadTilemap;
    private Transform _constructionParent;
    private Construction _constructionSelected;
    private ConstructionCursor _constructionCursor;
    private UIConstructionInfo _uiConstructionInfo;
    private ConstructionEditor _constructionEditor;
    private Grid _isometricGrid;
    private Dictionary<Vector2Int, Construction> _constructionBuilded = new();

    public static ConstructionManager Instance => _instance;

    private void Awake() {
        _instance = this;

        _isometricGrid = GetComponent<Grid>();
        _constructionParent = transform.Find("Buildings").transform;
        _constructionCursor = FindObjectOfType<ConstructionCursor>();
        _uiConstructionInfo = FindObjectOfType<UIConstructionInfo>();
        _constructionEditor = FindObjectOfType<ConstructionEditor>();
        _roadTilemap = transform.Find("RoadTilemap").GetComponent<Tilemap>();
        print(_roadTilemap);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && !_constructionEditor.IsEditing) {
            if (UIManager.IsPointerOverUI()) return;

            var construction = GetConstructionOverPointer();

            _constructionSelected = construction;

            _constructionCursor.Construction = construction;
            if (construction) {
                _constructionCursor.Error = false;
                _constructionCursor.transform.position = construction.transform.position;
                _constructionCursor.Direction = construction.Direction_;
                _constructionCursor.SetOutline(true);
            }

            _uiConstructionInfo.Construction = construction;
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

    public Construction GetConstruction(Vector2Int cellPos) {
        return _constructionBuilded.GetValueOrDefault(cellPos);
    }

    public void SetConstruction(Construction construction, Vector2Int cellPos, Construction.Direction direction) {
        if (HasConstruction(cellPos)) return;

        if (construction.ConstructionType == Construction.Type.BUILDING) {
            var pos = new Vector3Int(cellPos.x, cellPos.y, 0);
            var worldPos = _isometricGrid.CellToWorld(pos);
            worldPos.y += 0.25f;

            var newConstruction = Instantiate(construction, worldPos, Quaternion.identity, _constructionParent);
            newConstruction.CellPos = cellPos;
            newConstruction.Direction_ = direction;

            _constructionBuilded[cellPos] = newConstruction;
        } else if (construction.ConstructionType == Construction.Type.ROAD) {
            _roadTilemap.SetTile(new Vector3Int(cellPos.x, cellPos.y), construction.RoadTile);
        }
    }

    public void RemoveConstruction(Vector2Int cellPos) {
        var construction = _constructionBuilded[cellPos];
        if (construction) {
            if (_constructionSelected == construction) {
                _uiConstructionInfo.Construction = null;
                _constructionCursor.Construction = null;
                _constructionSelected = null;
            }

            Destroy(construction.gameObject);
            _constructionBuilded[cellPos] = null;
        }
    }

    public bool HasConstruction(Vector2Int cellPos) {
        return GetConstruction(cellPos);
    }

    public Vector2 CellToWorld(Vector2Int cellPos) {
        var pos = new Vector3Int(cellPos.x, cellPos.y, 0);
        return _isometricGrid.CellToWorld(pos);
    }

    public Vector2Int WorldToCell(Vector2 worldPos) {
        var cellPos = _isometricGrid.WorldToCell(worldPos);
        return new Vector2Int(cellPos.x, cellPos.y);
    }
}
