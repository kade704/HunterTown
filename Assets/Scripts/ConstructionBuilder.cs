using UnityEngine;
using UnityEngine.Events;

public class ConstructionBuilder : MonoBehaviour
{
    private Construction _constructionPrefab;
    private ConstructionGridmap _constructionGridMap;
    private GridCursor _gridCursor;
    private bool _isDestructionMode = false;
    private UnityEvent _onEnterBuildMode = new UnityEvent();
    private UnityEvent _onExitBuildMode = new UnityEvent();

    public UnityEvent OnEnterBuildMode => _onEnterBuildMode;
    public UnityEvent OnExitBuildMode => _onExitBuildMode;

    public Construction ConstructionPrefab
    {
        get { return _constructionPrefab; }
        set
        {
            _constructionPrefab = value;
            if (_constructionPrefab)
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
        _constructionGridMap = FindObjectOfType<ConstructionGridmap>();
    }

    private void Update()
    {
        if (_constructionPrefab)
        {
            var buildable = CheckBuildable();

            var cursorSprite = _constructionPrefab.DefaultSprite;
            _gridCursor.Sprite = cursorSprite;
            _gridCursor.Color = !buildable ? new Color(1.0f, 0.0f, 0.0f, 0.8f) : new Color(0.0f, 1.0f, 0.5f, 0.8f);

            if (Input.GetMouseButtonDown(0) && !UIManager.IsUIObjectOverPointer() && buildable)
            {
                _constructionGridMap.BuildConstruction(_constructionPrefab, _gridCursor.CellPos);

                GameManager.Instance.GetSystem<Player>().Money -= _constructionPrefab.Cost;
                GameManager.Instance.GetSystem<AudioController>().PlaySFX("Build");
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                _gridCursor.Sprite = null;
                _gridCursor.Color = Color.white;
                ConstructionPrefab = null;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {

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
                ConstructionPrefab = null;
            }
        }
    }

    private bool CheckBuildable()
    {
        if (_constructionGridMap.IsConstructionExistAt(_gridCursor.CellPos))
        {
            return false;
        }

        if (!_constructionPrefab.GetComponent<Road>())
        {
            var x = new[] { 1, -1, 0, 0 };
            var y = new[] { 0, 0, 1, -1 };
            for (var i = 0; i < 4; i++)
            {
                var pos = new Vector2Int(_gridCursor.CellPos.x + x[i], _gridCursor.CellPos.y + y[i]);
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
