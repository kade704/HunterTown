using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour {
    private static ConstructionManager _instance;
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
        _constructionParent = transform.Find("Constructions").transform;
        _constructionCursor = FindObjectOfType<ConstructionCursor>();
        _uiConstructionInfo = FindObjectOfType<UIConstructionInfo>();
        _constructionEditor = FindObjectOfType<ConstructionEditor>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && !_constructionEditor.IsEditing) {
            if (UIManager.IsPointerOverUI()) return;

            var construction = GetConstructionOverPointer();

            _constructionSelected = construction;

            _constructionCursor.Construction = construction;
            if (construction) {
                _constructionCursor.transform.position = construction.transform.position;
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

    public void SetConstruction(Construction construction, Vector2Int cellPos) {
        if (HasConstruction(cellPos)) return;

        var pos = new Vector3Int(cellPos.x, cellPos.y, 0);
        var worldPos = _isometricGrid.CellToWorld(pos);
        worldPos.y += 0.25f;

        var newConstruction = Instantiate(construction, worldPos, Quaternion.identity, _constructionParent);
        newConstruction.CellPos = cellPos;

        _constructionBuilded[cellPos] = newConstruction;
    }

    public void RemoveConstruction(Vector2Int cellPos) {
        print(cellPos);

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
