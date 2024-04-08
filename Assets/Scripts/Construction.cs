using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class Construction : MonoBehaviour
{
    [SerializeField] protected string _id;
    [SerializeField] protected int _cost;
    [SerializeField] protected string _displayName;
    [SerializeField] protected Sprite _icon;
    [SerializeField] protected Vector2Int _size;
    [SerializeField] protected bool _buildable;
    [SerializeField] protected bool _destroyable;
    [SerializeField] protected ConstructionInteraction[] _interactions;
    protected UnityEvent<string> _onInteracted = new();

    protected UnityEvent _onClicked = new();
    protected SpriteRenderer _spriteRenderer;
    protected Vector2Int _cellPos;
    protected int _defaultOrder;

    public string Id => _id;
    public Vector2Int CellPos { get { return _cellPos; } set { _cellPos = value; } }
    public UnityEvent<string> OnInteracted => _onInteracted;
    public UnityEvent OnClicked => _onClicked;
    public int Cost => _cost;
    public string DisplayName => _displayName;
    public Sprite Icon => _icon;
    public Vector2Int Size => _size;

    public bool Buildable => _buildable;
    public bool Destroyable => _destroyable;
    public ConstructionInteraction[] Interactions => _interactions;

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultOrder = _spriteRenderer.sortingOrder;
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        _spriteRenderer.sortingOrder = _defaultOrder - Mathf.FloorToInt(transform.position.y * 10);
    }

    private void OnDestroy()
    {
        if (ConstructionManager.Instance.GetConstructionAt(CellPos) == this)
            ConstructionManager.Instance.DestroyConstruction(CellPos);
    }

    public void SetOutline(bool outline)
    {
        _spriteRenderer.material.SetFloat("_Outline", outline ? 1 : 0);
    }
}
