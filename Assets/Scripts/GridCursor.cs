using UnityEngine;

public class GridCursor : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private ConstructionGridMap _constructionGridMap;
    private Vector2Int _cellPos;
    private Sprite _defaultCell;

    public Sprite Sprite
    {
        get { return _spriteRenderer.sprite; }
        set
        {
            if (value)
            {
                _spriteRenderer.sprite = value;
            }
            else
            {
                _spriteRenderer.sprite = _defaultCell;
            }
        }
    }

    public Color Color
    {
        get { return _spriteRenderer.color; }
        set { _spriteRenderer.color = value; }
    }

    public Vector2Int CellPos => _cellPos;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _constructionGridMap = FindObjectOfType<ConstructionGridMap>();
        _defaultCell = _spriteRenderer.sprite;
    }

    private void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _cellPos = _constructionGridMap.WorldToCell(mousePos);
        transform.position = _constructionGridMap.CellToWorld(_cellPos);
    }
}
