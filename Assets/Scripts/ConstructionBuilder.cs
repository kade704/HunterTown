using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class ConstructionBuilder : MonoBehaviour
{
    private Construction _constructionPrefab;
    private ConstructionGridmap _constructionGridMap;
    private SpriteRenderer[] _previewSprites;
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

    public bool IsBuildMode => _constructionPrefab != null;


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
        _previewSprites = transform.GetComponentsInChildren<SpriteRenderer>();
        _constructionGridMap = GameManager.Instance.GetSystem<ConstructionGridmap>();
    }

    private void Update()
    {
        if (_constructionPrefab)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var cellPos = _constructionGridMap.WorldToCell(mousePos);

            var buildable = CheckBuildable(cellPos);

            var cursorSprite = _constructionPrefab.DefaultSprite;
            _previewSprites[0].sprite = cursorSprite;
            _previewSprites[0].color = !buildable ? new Color(1.0f, 0.0f, 0.0f, 0.8f) : new Color(0.0f, 1.0f, 0.5f, 0.8f);
            _previewSprites[0].transform.position = _constructionGridMap.CellToWorld(cellPos);

            if (Input.GetMouseButtonDown(0) && !UIManager.IsUIObjectOverPointer() && buildable)
            {
                _constructionGridMap.BuildConstruction(_constructionPrefab, cellPos);

                GameManager.Instance.GetSystem<Player>().Money -= _constructionPrefab.Cost;
                GameManager.Instance.GetSystem<AudioController>().PlaySFX("Build");
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                _previewSprites[0].sprite = null;
                _previewSprites[0].color = Color.white;
                ConstructionPrefab = null;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                // Rotate construction
            }
        }
        else if (_isDestructionMode)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var colliders = Physics2D.OverlapPointAll(mousePos);
            Construction construction = null;
            foreach (var collider in colliders)
            {
                construction = collider.GetComponent<Construction>();
                if (construction && !construction.Destroyable) construction = null;
                if (construction) break;
            }

            if (construction)
            {
                _previewSprites[0].transform.position = construction.transform.position;
                _previewSprites[0].sprite = construction.Sprite;
                _previewSprites[0].color = new Color(1.0f, 0.0f, 0.0f, 0.8f);

                if (Input.GetMouseButtonDown(0) && !UIManager.IsUIObjectOverPointer())
                {
                    GameManager.Instance.GetSystem<Player>().Money += construction.Cost / 2;
                    GameManager.Instance.GetSystem<AudioController>().PlaySFX("Destruction");
                    _constructionGridMap.DestroyConstruction(construction.CellPos);
                }
            }
            else
            {
                _previewSprites[0].sprite = null;
                _previewSprites[0].color = Color.white;
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                _previewSprites[0].sprite = null;
                _previewSprites[0].color = Color.white;
                _isDestructionMode = false;
            }
        }
    }


    private bool CheckBuildable(Vector2Int cellPos)
    {
        if (_constructionGridMap.IsConstructionExistAt(cellPos))
        {
            return false;
        }

        if (!_constructionPrefab.GetComponent<Road>())
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
