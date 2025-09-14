using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

[System.Serializable]
public struct Interaction
{
    [SerializeField] private string _id;
    [SerializeField] private string _displayName;
    [SerializeField] private Sprite _icon;
    [SerializeField] private KeyCode _key;

    public string ID => _id;
    public string DisplayName => _displayName;
    public Sprite Icon => _icon;
    public KeyCode Key => _key;
}


public class Interactable : MonoBehaviour
{
    [SerializeField]
    private string _displayName;

    [Title("Description", bold: false)]
    [HideLabel]
    [MultiLineProperty(5)]
    [SerializeField]
    private string _description;

    [Title("Sub Description", bold: false)]
    [HideLabel]
    [MultiLineProperty(3)]
    [SerializeField]
    private string _subDescription;

    [SerializeField]
    private Interaction[] _interactions;

    private bool _selected;
    private SpriteRenderer[] _renderers;
    private Dictionary<SpriteRenderer, Color> _defaultColors = new();
    private SortingGroup _sortingGroup;
    private UnityEvent<Interaction> _onInteracted = new();
    private UnityEvent _onMouseEnter = new();
    private UnityEvent _onMouseExit = new();
    private UnityEvent _onSelected = new();
    private UnityEvent _onDeselected = new();


    public string DisplayName
    {
        get => _displayName;
        set => _displayName = value;
    }

    public string Description
    {
        get => _description;
        set => _description = value;
    }

    public string SubDescription
    {
        get => _subDescription;
        set => _subDescription = value;
    }

    public Interaction[] Interactions => _interactions;
    public SpriteRenderer[] SpriteRenderer => _renderers;
    public SortingGroup SortingGroup => _sortingGroup;
    public UnityEvent<Interaction> OnInteracted => _onInteracted;
    public UnityEvent OnMouseEnter => _onMouseEnter;
    public UnityEvent OnMouseExit => _onMouseExit;
    public UnityEvent OnSelected => _onSelected;
    public UnityEvent OnDeselected => _onDeselected;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<SpriteRenderer>();
        _sortingGroup = GetComponent<SortingGroup>();
    }

    private void Start()
    {
        foreach (var _renderer in _renderers)
            _defaultColors.Add(_renderer, _renderer.color);

        _onSelected.AddListener(Selected);
        _onDeselected.AddListener(Deselected);
        _onMouseEnter.AddListener(MouseEnter);
        _onMouseExit.AddListener(MouseExit);
    }

    private void Selected()
    {
        _selected = true;
        foreach (var renderer in _renderers)
        {
            renderer.color = new Color(0.1f, 0.9f, 0.5f, _defaultColors[renderer].a);
        }
    }

    private void Deselected()
    {
        _selected = false;
        foreach (var renderer in _renderers)
        {
            renderer.color = _defaultColors[renderer];
        }
    }

    private void MouseEnter()
    {
        if (!_selected)
        {
            foreach (var renderer in _renderers)
            {
                renderer.color = new Color(0.9f, 0.9f, 0.9f, _defaultColors[renderer].a);
            }
        }
    }

    private void MouseExit()
    {
        if (!_selected)
        {
            foreach (var renderer in _renderers)
            {
                renderer.color = _defaultColors[renderer];
            }
        }
    }
}
