using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Interactable))]
public class Construction : MonoBehaviour
{
    [SerializeField]
    private string _id;

    [SerializeField]
    private int _cost;

    [SerializeField]
    private string _displayName;

    [Title("Description", bold: false)]
    [HideLabel]
    [MultiLineProperty(5)]
    [SerializeField]
    private string _description;

    [PreviewField]
    [SerializeField]
    private Sprite _defaultSprite;

    [SerializeField]
    private Vector2Int _size;

    [SerializeField]
    private bool _buildable;

    [SerializeField]
    private bool _destroyable;

    [SerializeField]
    private bool _visitable;

    [SerializeField]
    private int _durability;

    private List<Hunter> _visitedHunters = new();
    private ConstructionGridmap _constructionGridMap;
    private Interactable _interactable;
    private Vector2Int _cellPos;
    private UnityEvent _onVisitorChanged = new UnityEvent();
    private UnityEvent _onBuilded = new UnityEvent();
    private UnityEvent _onDestroyed = new UnityEvent();

    public Vector2Int CellPos
    {
        get => _cellPos;
        set => _cellPos = value;
    }
    public ConstructionGridmap ConstructionGridMap
    {
        get => _constructionGridMap;
        set => _constructionGridMap = value;
    }
    public int Durability
    {
        get => _durability;
        set => _durability = value;
    }
    public string ID => _id;
    public int Cost => _cost;
    public string DisplayName => _displayName;
    public string Description => _description;
    public Sprite DefaultSprite => _defaultSprite;
    public Vector2Int Size => _size;

    public bool Buildable => _buildable;
    public bool Destroyable => _destroyable;
    public bool Visitable => _visitable;
    public Hunter[] VisitedHunters => _visitedHunters.ToArray();
    public UnityEvent OnVisitorChanged => _onVisitorChanged;
    public UnityEvent OnBuilded => _onBuilded;
    public UnityEvent OnDestroyed => _onDestroyed;

    private void Awake()
    {
        _interactable = GetComponent<Interactable>();
    }

    private void Start()
    {
        _interactable.DisplayName = _displayName;
        _interactable.Description = _description;
    }

    public void EnterVisitor(Hunter hunter)
    {
        if (!_visitedHunters.Contains(hunter))
        {
            _visitedHunters.Add(hunter);
            _onVisitorChanged.Invoke();
        }
    }

    public void ExitVisitor(Hunter hunter)
    {
        if (_visitedHunters.Contains(hunter))
        {
            _visitedHunters.Remove(hunter);
            _onVisitorChanged.Invoke();
        }
    }
}
