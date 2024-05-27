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
    private int _maintenanceCost;

    [SerializeField]
    private int _populationCondition;


    [PreviewField]
    [SerializeField]
    private Sprite _defaultSprite;

    [SerializeField]
    private int _size;

    [SerializeField]
    private bool _buildable;

    [SerializeField]
    private bool _destroyable;


    private ConstructionGridmap _constructionGridMap;
    private Interactable _interactable;
    private Vector2Int _cellPos;


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

    public string ID => _id;
    public int Cost => _cost;
    public int MaintenanceCost => _maintenanceCost;
    public int PopulationCondition => _populationCondition;
    public Sprite DefaultSprite => _defaultSprite;
    public int Size => _size;

    public bool Buildable => _buildable;
    public bool Destroyable => _destroyable;

    public UnityEvent OnBuilded => _onBuilded;
    public UnityEvent OnDestroyed => _onDestroyed;


    private void Awake()
    {
        _interactable = GetComponent<Interactable>();
    }
}
