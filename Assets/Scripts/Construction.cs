using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

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

    private List<Hunter> _visitedHunters = new();
    private ConstructionGridmap _constructionGridMap;
    private Interactable _interactable;
    private Vector2Int _cellPos;

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
    public string ID => _id;
    public int Cost => _cost;
    public string DisplayName => _displayName;
    public string Description => _description;
    public Sprite DefaultSprite => _defaultSprite;
    public Vector2Int Size => _size;

    public bool Buildable => _buildable;
    public bool Destroyable => _destroyable;
    public bool Visitable => _visitable;
    public List<Hunter> VisitedHunters => _visitedHunters;

    private void Awake()
    {
        _interactable = GetComponent<Interactable>();
    }

    private void Start()
    {
        _interactable.DisplayName = _displayName;
        _interactable.Description = _description;
    }

    private void OnDestroy()
    {
        if (_constructionGridMap.GetConstructionAt(CellPos) == this)
            _constructionGridMap.DestroyConstruction(CellPos);
    }
}
