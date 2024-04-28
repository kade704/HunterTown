using UnityEngine;
using UnityEngine.Events;

public class ConstructionBuilder : MonoBehaviour
{
    private Construction _constructionToBuild;
    private ConstructionGridMap _constructionGridMap;
    private Rotatable.Direction _buildDirection;
    private GridCursor _gridCursor;
    private bool _isDestructionMode = false;
    private UnityEvent _onEnterBuildMode = new UnityEvent();
    private UnityEvent _onExitBuildMode = new UnityEvent();

    public UnityEvent OnEnterBuildMode => _onEnterBuildMode;
    public UnityEvent OnExitBuildMode => _onExitBuildMode;

    public Construction ConstructionToBuild
    {
        get { return _constructionToBuild; }
        set
        {
            _constructionToBuild = value;
            if (_constructionToBuild)
            {
                _onEnterBuildMode.Invoke();
            }
            else
            {
                _onExitBuildMode.Invoke();
            }
        }
    }

    public void SetDestructionMode(bool value)
    {
        _isDestructionMode = value;
        if (_isDestructionMode)
        {
            _onEnterBuildMode.Invoke();
        }
        else
        {
            _onExitBuildMode.Invoke();
        }
    }

    private void Awake()
    {
        _gridCursor = FindObjectOfType<GridCursor>();
        _constructionGridMap = FindObjectOfType<ConstructionGridMap>();
    }

    private void Update()
    {
        if (_constructionToBuild)
        {
            var buildable = CheckBuildable();

            var cursorSprite = _constructionToBuild.Icon;
            if (_constructionToBuild.GetComponent<Rotatable>() != null)
            {
                cursorSprite = _constructionToBuild.GetComponent<Rotatable>().GetSpriteFromDirection(_buildDirection);
            }
            _gridCursor.Sprite = cursorSprite;
            _gridCursor.Color = !buildable ? new Color(1.0f, 0.0f, 0.0f, 0.8f) : new Color(0.0f, 1.0f, 0.5f, 0.8f);

            if (Input.GetMouseButtonDown(0) && !UIManager.IsUIObjectOverPointer() && buildable)
            {
                var newConstruction = _constructionGridMap.BuildConstruction(_constructionToBuild, _gridCursor.CellPos);
                if (newConstruction.GetComponent<Rotatable>() != null)
                {
                    newConstruction.GetComponent<Rotatable>().Direction_ = _buildDirection;
                }

                GameManager.Instance.GetSystem<Player>().Money -= _constructionToBuild.Cost;
                GameManager.Instance.GetSystem<AudioController>().PlaySFX("Build");
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                _gridCursor.Sprite = null;
                _gridCursor.Color = Color.white;
                ConstructionToBuild = null;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _buildDirection += 1;
                if (_buildDirection >= (Rotatable.Direction)4) _buildDirection = 0;
            }
        }

        if (_isDestructionMode)
        {
            if (Input.GetMouseButtonDown(0) && !UIManager.IsUIObjectOverPointer())
            {
                var construction = _constructionGridMap.GetConstructionAt(_gridCursor.CellPos);
                if (construction)
                {
                    GameManager.Instance.GetSystem<Player>().Money += construction.Cost / 2;
                    Destroy(construction.gameObject);
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                _gridCursor.Sprite = null;
                _gridCursor.Color = Color.white;
                ConstructionToBuild = null;
            }
        }
    }

    private bool CheckBuildable()
    {
        if (_constructionGridMap.IsConstructionExistAt(_gridCursor.CellPos))
        {
            return false;
        }
        if (_constructionToBuild.GetComponent<Rotatable>() != null)
        {
            if (_buildDirection == Rotatable.Direction.SOUTH)
            {
                if (!_constructionGridMap.GetConstructionAt(new Vector2Int(_gridCursor.CellPos.x - 1, _gridCursor.CellPos.y))?.GetComponent<Road>())
                {
                    return false;
                }
            }
            else if (_buildDirection == Rotatable.Direction.NORTH)
            {
                if (!_constructionGridMap.GetConstructionAt(new Vector2Int(_gridCursor.CellPos.x + 1, _gridCursor.CellPos.y))?.GetComponent<Road>())
                {
                    return false;
                }
            }
            else if (_buildDirection == Rotatable.Direction.EAST)
            {
                if (!_constructionGridMap.GetConstructionAt(new Vector2Int(_gridCursor.CellPos.x, _gridCursor.CellPos.y - 1))?.GetComponent<Road>())
                {
                    return false;
                }
            }
            else if (_buildDirection == Rotatable.Direction.WEST)
            {
                if (!_constructionGridMap.GetConstructionAt(new Vector2Int(_gridCursor.CellPos.x, _gridCursor.CellPos.y + 1))?.GetComponent<Road>())
                {
                    return false;
                }
            }
        }
        return true;
    }
}
