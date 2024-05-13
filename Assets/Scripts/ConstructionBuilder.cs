using Sirenix.Utilities;
using UnityEngine;

public class ConstructionBuilder : MonoBehaviour
{
    public enum BuildMode { Select, Construct, Destruct }

    private Construction _selectedConstructionPrefab;
    private ConstructionGridmap _constructionGridMap;
    private SpriteRenderer[] _previewSprites;
    private BuildMode _buildMode = BuildMode.Select;
    private bool _isDragging = false;
    private Vector2Int[] _draggingCells = new Vector2Int[10];
    private Vector2Int _startDragPos;

    public Construction SelectedConstructionPrefab
    {
        get => _selectedConstructionPrefab;
        set => _selectedConstructionPrefab = value;
    }

    public BuildMode BulidMode
    {
        get => _buildMode;
        set
        {
            _buildMode = value;
            _previewSprites.ForEach(sprite => sprite.sprite = null);
        }
    }

    private void Awake()
    {
        _previewSprites = transform.GetComponentsInChildren<SpriteRenderer>();
        _constructionGridMap = GameManager.Instance.GetSystem<ConstructionGridmap>();
    }

    private void Update()
    {
        if (_buildMode == BuildMode.Construct)
        {
            ConstructMode();
        }
        else if (_buildMode == BuildMode.Destruct)
        {
            DestructMode();
        }
    }

    private void ConstructMode()
    {
        if (!_selectedConstructionPrefab)
        {
            _buildMode = BuildMode.Select;
            return;
        }

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var cellPos = _constructionGridMap.WorldToCell(mousePos);

        if (_selectedConstructionPrefab.GetComponent<Road>())
        {
            if (Input.GetMouseButtonDown(0) && !UIUtil.IsUIObjectOverPointer())
            {
                _isDragging = true;
                _startDragPos = cellPos;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_isDragging)
                {
                    foreach (var draggingCell in _draggingCells)
                    {
                        if (_constructionGridMap.CheckConstructionBuildable(_selectedConstructionPrefab, draggingCell))
                        {
                            GameManager.Instance.GetSystem<Player>().Money -= _selectedConstructionPrefab.Cost;
                            _constructionGridMap.BuildConstruction(_selectedConstructionPrefab, draggingCell);
                        }
                    }
                    GameManager.Instance.GetSystem<AudioController>().PlaySFX("Build");
                    _isDragging = false;
                }
            }

            if (_isDragging)
            {
                Vector2 offset = cellPos - _startDragPos;
                var angle = Vector2.SignedAngle(Vector2.right, offset);
                var step = Mathf.Min(Mathf.Max(Mathf.Abs((int)offset.x), Mathf.Abs((int)offset.y)), 9) + 1;

                var direction = Vector2Int.zero;
                if (angle > 45 && angle < 135)
                {
                    direction = Vector2Int.up;
                }
                else if (angle < -45 && angle > -135)
                {
                    direction = Vector2Int.down;
                }
                else if (angle > 135 || angle < -135)
                {
                    direction = Vector2Int.left;
                }
                else if (angle < 45 && angle > -45)
                {
                    direction = Vector2Int.right;
                }

                for (var i = 0; i < step; i++)
                {
                    var pos = _startDragPos + direction * i;
                    var buildable = CheckBuildable(pos);

                    var cursorSprite = _selectedConstructionPrefab.DefaultSprite;
                    _previewSprites[i].sprite = cursorSprite;
                    _previewSprites[i].color = !buildable ? new Color(1.0f, 0.0f, 0.0f, 0.8f) : new Color(0.0f, 1.0f, 0.5f, 0.8f);
                    _previewSprites[i].transform.position = _constructionGridMap.CellToWorld(pos);

                    _draggingCells[i] = pos;
                }
                for (var i = step; i < 10; i++)
                {
                    _previewSprites[i].sprite = null;
                    _draggingCells[i] = new Vector2Int(-1, -1);
                }
            }
            else
            {
                var buildable = CheckBuildable(cellPos);

                var cursorSprite = _selectedConstructionPrefab.DefaultSprite;
                _previewSprites[0].sprite = cursorSprite;
                _previewSprites[0].color = !buildable ? new Color(1.0f, 0.0f, 0.0f, 0.8f) : new Color(0.0f, 1.0f, 0.5f, 0.8f);
                _previewSprites[0].transform.position = _constructionGridMap.CellToWorld(cellPos);

                for (var i = 1; i < 10; i++)
                {
                    _previewSprites[i].sprite = null;
                }
            }
        }
        else
        {
            var buildable = CheckBuildable(cellPos);

            var cursorSprite = _selectedConstructionPrefab.DefaultSprite;
            _previewSprites[0].sprite = cursorSprite;
            _previewSprites[0].color = !buildable ? new Color(1.0f, 0.0f, 0.0f, 0.8f) : new Color(0.0f, 1.0f, 0.5f, 0.8f);
            _previewSprites[0].transform.position = _constructionGridMap.CellToWorld(cellPos);

            if (Input.GetMouseButtonDown(0) && !UIUtil.IsUIObjectOverPointer() && buildable)
            {
                _constructionGridMap.BuildConstruction(_selectedConstructionPrefab, cellPos);

                GameManager.Instance.GetSystem<Player>().Money -= _selectedConstructionPrefab.Cost;
                GameManager.Instance.GetSystem<AudioController>().PlaySFX("Build");
            }
        }
    }


    private void DestructMode()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var colliders = Physics2D.OverlapPointAll(mousePos);
        Construction hoveredConstruction = null;
        foreach (var collider in colliders)
        {
            hoveredConstruction = collider.GetComponent<Construction>();
            if (hoveredConstruction && !hoveredConstruction.Destroyable) hoveredConstruction = null;
            if (hoveredConstruction) break;
        }

        if (hoveredConstruction)
        {
            _previewSprites[0].transform.position = hoveredConstruction.transform.position;
            _previewSprites[0].sprite = hoveredConstruction.Sprite;
            _previewSprites[0].color = new Color(1.0f, 0.0f, 0.0f, 0.8f);

            if (Input.GetMouseButtonDown(0) && !UIUtil.IsUIObjectOverPointer())
            {
                GameManager.Instance.GetSystem<Player>().Money += hoveredConstruction.Cost / 2;
                GameManager.Instance.GetSystem<AudioController>().PlaySFX("Destruction");
                _constructionGridMap.DestroyConstruction(hoveredConstruction.CellPos);
            }
        }
        else
        {
            _previewSprites[0].sprite = null;
            _previewSprites[0].color = Color.white;
        }
    }

    private bool CheckBuildable(Vector2Int cellPos)
    {
        if (_constructionGridMap.IsConstructionExistAt(cellPos))
        {
            return false;
        }

        if (!_selectedConstructionPrefab.GetComponent<Road>())
        {
            var x = new[] { 1, -1, 0, 0 };
            var y = new[] { 0, 0, 1, -1 };
            for (var i = 0; i < 4; i++)
            {
                var pos = new Vector2Int(cellPos.x + x[i], cellPos.y + y[i]);
                if (_constructionGridMap.GetConstructionAt(pos)?.GetComponent<Road>())
                {
                    return true;
                }
            }

            return false;
        }

        return true;
    }
}
