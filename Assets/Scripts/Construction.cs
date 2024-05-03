using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Interactable))]
public class Construction : MonoBehaviour
{
    [SerializeField] private string _id;
    [SerializeField] private int _cost;
    [SerializeField] private string _displayName;
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Vector2Int _size;
    [SerializeField] private bool _buildable;
    [SerializeField] private bool _destroyable;

    private UnityEvent<string> _onInteracted = new();
    private UnityEvent _onClicked = new();
    private ConstructionGridmap _constructionGridMap;
    private SpriteRenderer _spriteRenderer;
    private Interactable _interactable;
    private Vector2Int _cellPos;
    private int _defaultOrder;

    public string Id => _id;
    public Vector2Int CellPos { get { return _cellPos; } set { _cellPos = value; } }
    public ConstructionGridmap ConstructionGridMap { get { return _constructionGridMap; } set { _constructionGridMap = value; } }
    public UnityEvent<string> OnInteracted => _onInteracted;
    public UnityEvent OnClicked => _onClicked;
    public int Cost => _cost;
    public string DisplayName => _displayName;
    public Sprite DefaultSprite => _defaultSprite;
    public Vector2Int Size => _size;

    public bool Buildable => _buildable;
    public bool Destroyable => _destroyable;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _interactable = GetComponent<Interactable>();
        _defaultOrder = _spriteRenderer.sortingOrder;
    }

    private void Start()
    {
        _interactable.DisplayName = _displayName;
    }

    private void Update()
    {
        _spriteRenderer.sortingOrder = _defaultOrder - Mathf.FloorToInt(transform.position.y * 10);
    }

    private void OnDestroy()
    {
        if (_constructionGridMap.GetConstructionAt(CellPos) == this)
            _constructionGridMap.DestroyConstruction(CellPos);
    }

    public void SetOutline(bool outline)
    {
        _spriteRenderer.material.SetFloat("_Outline", outline ? 1 : 0);
    }
}
