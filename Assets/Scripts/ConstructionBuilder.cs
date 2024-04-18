using UnityEngine;
using UnityEngine.Events;

public class ConstructionBuilder : MonoBehaviour
{
    private Construction _constructionToBuild;
    private ConstructionGridMap _constructionGridMap;
    private Building.Direction _buildDirection;
    private GridCursor _gridCursor;
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

    private void Awake()
    {
        _gridCursor = FindObjectOfType<GridCursor>();
        _constructionGridMap = FindObjectOfType<ConstructionGridMap>();
    }

    private void Update()
    {
        if (_constructionToBuild)
        {
            var buildable = CheckBuildable(_gridCursor.CellPos, _buildDirection);

            var cursorSprite = _constructionToBuild.Icon;
            if (_constructionToBuild is Building)
            {
                cursorSprite = (_constructionToBuild as Building).GetSpriteFromDirection(_buildDirection);
            }
            _gridCursor.Sprite = cursorSprite;
            _gridCursor.Color = !buildable ? Color.red : Color.white;

            if (Input.GetMouseButtonDown(0) && !UIManager.IsUIObjectOverPointer() && buildable)
            {
                var newConstruction = _constructionGridMap.BuildConstruction(_constructionToBuild, _gridCursor.CellPos);
                if (newConstruction is Building)
                {
                    (newConstruction as Building).Direction_ = _buildDirection;
                }

                GameManager.Instance.Money -= _constructionToBuild.Cost;
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                _gridCursor.Sprite = null;
                ConstructionToBuild = null;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _buildDirection += 1;
                if (_buildDirection >= (Building.Direction)4) _buildDirection = 0;
            }
        }
        else
        {
            // if (Input.GetMouseButtonDown(0))
            // {
            //     if (UIManager.IsUIObjectOverPointer()) return;

            //     _constructionSelected = GetConstructionOverPointer();
            //     OnConstructionClicked.Invoke(_constructionSelected);

            //     if (_constructionSelected)
            //     {
            //         _constructionSelected.OnClicked.Invoke();
            //     }
            // }
        }
    }

    private bool CheckBuildable(Vector2Int cellpos, Building.Direction direction)
    {
        var result = true;
        if (_constructionGridMap.IsConstructionExistAt(cellpos))
        {
            result = false;
        }
        if (_constructionToBuild is Building)
        {
            if (direction == Building.Direction.SOUTH)
            {
                if (!_constructionGridMap.IsRoadExistAt(new Vector2Int(cellpos.x - 1, cellpos.y)))
                {
                    result = false;
                }
            }
            else if (direction == Building.Direction.NORTH)
            {
                if (!_constructionGridMap.IsRoadExistAt(new Vector2Int(cellpos.x + 1, cellpos.y)))
                {
                    result = false;
                }
            }
            else if (direction == Building.Direction.EAST)
            {
                if (!_constructionGridMap.IsRoadExistAt(new Vector2Int(cellpos.x, cellpos.y - 1)))
                {
                    result = false;
                }
            }
            else if (direction == Building.Direction.WEST)
            {
                if (!_constructionGridMap.IsRoadExistAt(new Vector2Int(cellpos.x, cellpos.y + 1)))
                {
                    result = false;
                }
            }
        }
        return result;
    }
}
