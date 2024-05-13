using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

[System.Serializable]
public struct Interaction
{
    [SerializeField] private string _id;
    [SerializeField] private string _displayName;
    [SerializeField] private Sprite _icon;

    public string ID => _id;
    public string DisplayName => _displayName;
    public Sprite Icon => _icon;
}


public class Interactable : MonoBehaviour
{
    [SerializeField] private Interaction[] _interactions;

    private string _displayName;
    private string _description;
    private SpriteRenderer[] _renderers;
    private SortingGroup _sortingGroup;
    private int _defaultOrder;
    private UnityEvent<Interaction> _onInteracted = new();

    public Interaction[] Interactions => _interactions;
    public string DisplayName
    {
        get { return _displayName; }
        set { _displayName = value; }
    }
    public string Description
    {
        get { return _description; }
        set { _description = value; }
    }

    public SpriteRenderer[] SpriteRenderer => _renderers;
    public SortingGroup SortingGroup => _sortingGroup;
    public UnityEvent<Interaction> OnInteracted => _onInteracted;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<SpriteRenderer>();
        _sortingGroup = GetComponent<SortingGroup>();
    }

    private void Start()
    {
        _defaultOrder = _sortingGroup.sortingOrder;
    }

    private void Update()
    {
        _sortingGroup.sortingOrder = 300 - Mathf.FloorToInt(transform.position.y * 10) + _defaultOrder;
    }

    public void SetOutline(bool value)
    {
        foreach (var _renderer in _renderers)
            _renderer.material.SetFloat("_Outline", value ? 1 : 0);
    }
}
