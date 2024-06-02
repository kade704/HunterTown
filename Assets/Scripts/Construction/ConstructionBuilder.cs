using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

public class ConstructionBuilder : MonoBehaviour
{
    public enum BuildMode { Select, Construct, Destruct }

    private Construction _selectedConstructionPrefab;
    private ConstructionGridmap _constructionGridMap;
    private SpriteRenderer[] _previewSprites;
    private BuildMode _buildMode = BuildMode.Select;
    private bool _isDragging = false;
    private Vector2Int[] _draggingCells = new Vector2Int[32];
    private Vector2Int _startDragPos;
    private UnityEvent<BuildMode> _onBuildModeChanged = new();

    public Construction SelectedConstructionPrefab
    {
        get => _selectedConstructionPrefab;
        set => _selectedConstructionPrefab = value;
    }
    public UnityEvent<BuildMode> OnBuildModeChanged => _onBuildModeChanged;

    public BuildMode BulidMode
    {
        get => _buildMode;
        set
        {
            _buildMode = value;
            _onBuildModeChanged.Invoke(_buildMode);

            _previewSprites.ForEach(sprite => sprite.sprite = null);
            GameManager.Instance.GetSystem<InteractableSelector>().EnableSelect = _buildMode == BuildMode.Select;
        }
    }

    private void Awake()
    {
        _previewSprites = transform.GetComponentsInChildren<SpriteRenderer>();
        _constructionGridMap = GameManager.Instance.GetSystem<ConstructionGridmap>();
    }

    private void Start()
    {
        for (int i = 0; i < _draggingCells.Length; i++)
        {
            _draggingCells[i] = new Vector2Int(-1, -1);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !UIUtil.IsUIObjectOverPointer())
        {
            BulidMode = BuildMode.Select;
            _isDragging = false;
            return;
        }

        if (BulidMode == BuildMode.Construct)
        {
            ConstructMode();
        }
        else if (BulidMode == BuildMode.Destruct)
        {
            DestructMode();
        }
    }

    private void ConstructMode()
    {
        if (!_selectedConstructionPrefab)
        {
            BulidMode = BuildMode.Select;
            return;
        }

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var cellPos = _constructionGridMap.WorldToCell(mousePos);

        if (_selectedConstructionPrefab.GetComponent<Road>() != null)
        {
            var road = _selectedConstructionPrefab.GetComponent<Road>();

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
                        if (draggingCell != new Vector2Int(-1, -1) && CheckRoadBuildable(draggingCell))
                        {
                            if (_constructionGridMap.IsConstructionExistAt(draggingCell))
                            {
                                _constructionGridMap.DestroyConstruction(draggingCell);
                            }

                            _constructionGridMap.BuildConstruction(_selectedConstructionPrefab, draggingCell);

                            GameManager.Instance.GetSystem<MoneySystem>().Money -= _selectedConstructionPrefab.Cost;
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
                var step = Mathf.Min(Mathf.Max(Mathf.Abs((int)offset.x), Mathf.Abs((int)offset.y)), 63) + 1;

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
                    var buildable = CheckRoadBuildable(pos);

                    _previewSprites[i].enabled = true;
                    _previewSprites[i].sprite = GetRoadSprite(road, direction, i, step);
                    _previewSprites[i].color = !buildable ? new Color(1.0f, 0.0f, 0.0f, 0.8f) : new Color(0.0f, 1.0f, 0.5f, 0.8f);
                    _previewSprites[i].transform.position = _constructionGridMap.CellToWorld(pos);

                    _draggingCells[i] = pos;
                }
                for (var i = step; i < _previewSprites.Length; i++)
                {
                    _previewSprites[i].enabled = false;
                    _draggingCells[i] = new Vector2Int(-1, -1);
                }
            }
            else
            {
                var buildable = CheckRoadBuildable(cellPos);

                var cursorSprite = _selectedConstructionPrefab.DefaultSprite;
                _previewSprites[0].sprite = cursorSprite;
                _previewSprites[0].color = !buildable ? new Color(1.0f, 0.0f, 0.0f, 0.8f) : new Color(0.0f, 1.0f, 0.5f, 0.8f);
                _previewSprites[0].transform.position = _constructionGridMap.CellToWorld(cellPos);

                for (var i = 1; i < _previewSprites.Length; i++)
                {
                    _previewSprites[i].sprite = null;
                }
            }
        }
        else if (_selectedConstructionPrefab.GetComponent<Durable>() != null)
        {
            var buildable = CheckBuildingBuildable(cellPos);

            var cursorSprite = _selectedConstructionPrefab.DefaultSprite;
            _previewSprites[0].sprite = cursorSprite;
            _previewSprites[0].color = !buildable ? new Color(1.0f, 0.0f, 0.0f, 0.8f) : new Color(0.0f, 1.0f, 0.5f, 0.8f);
            _previewSprites[0].transform.position = _constructionGridMap.CellToWorld(cellPos);

            if (Input.GetMouseButtonDown(0) && !UIUtil.IsUIObjectOverPointer() && buildable)
            {
                _constructionGridMap.BuildConstruction(_selectedConstructionPrefab, cellPos);

                GameManager.Instance.GetSystem<MoneySystem>().Money -= _selectedConstructionPrefab.Cost;
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
            _previewSprites[0].sprite = hoveredConstruction.DefaultSprite;
            _previewSprites[0].color = new Color(1.0f, 0.0f, 0.0f, 0.8f);

            if (Input.GetMouseButtonDown(0) && !UIUtil.IsUIObjectOverPointer())
            {
                GameManager.Instance.GetSystem<MoneySystem>().Money += hoveredConstruction.Cost / 2;
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

    private bool CheckRoadBuildable(Vector2Int cellPos)
    {
        if (_constructionGridMap.IsConstructionExistAt(cellPos))
        {
            var construction = _constructionGridMap.GetConstructionAt(cellPos).GetComponent<Construction>();
            if (construction && !construction.GetComponent<Road>())
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckBuildingBuildable(Vector2Int cellPos)
    {
        for (var y = 0; y < _selectedConstructionPrefab.Size; y++)
        {
            for (var x = 0; x < _selectedConstructionPrefab.Size; x++)
            {
                if (_constructionGridMap.IsConstructionExistAt(cellPos + new Vector2Int(x, y)))
                {
                    return false;
                }
            }
        }

        for (var y = 0; y < _selectedConstructionPrefab.Size; y++)
        {
            for (var x = 0; x < _selectedConstructionPrefab.Size; x++)
            {
                var dx = new[] { 1, -1, 0, 0 };
                var dy = new[] { 0, 0, 1, -1 };
                for (var i = 0; i < 4; i++)
                {
                    var pos = new Vector2Int(cellPos.x + x + dx[i], cellPos.y + y + dy[i]);
                    if (_constructionGridMap.GetConstructionAt(pos)?.GetComponent<Road>())
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private Sprite GetRoadSprite(Road road, Vector2Int direction, int index, int step)
    {
        if (direction == Vector2Int.up)
        {
            if (index == 0)
            {
                return road.SpriteRule_.N;
            }
            else if (index == step - 1)
            {
                return road.SpriteRule_.S;
            }
            else
            {
                return road.SpriteRule_.SN;
            }
        }
        else if (direction == Vector2Int.right)
        {
            if (index == 0)
            {
                return road.SpriteRule_.E;
            }
            else if (index == step - 1)
            {
                return road.SpriteRule_.W;
            }
            else
            {
                return road.SpriteRule_.WE;
            }
        }
        else if (direction == Vector2Int.down)
        {
            if (index == 0)
            {
                return road.SpriteRule_.S;
            }
            else if (index == step - 1)
            {
                return road.SpriteRule_.N;
            }
            else
            {
                return road.SpriteRule_.SN;
            }
        }
        else if (direction == Vector2Int.left)
        {
            if (index == 0)
            {
                return road.SpriteRule_.W;
            }
            else if (index == step - 1)
            {
                return road.SpriteRule_.E;
            }
            else
            {
                return road.SpriteRule_.WE;
            }
        }
        else
        {
            return road.SpriteRule_.None;
        }
    }
}
