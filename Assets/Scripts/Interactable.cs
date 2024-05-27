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


    private SpriteRenderer[] _renderers;
    private Dictionary<SpriteRenderer, Color> _defaultColors = new();
    private SortingGroup _sortingGroup;
    private UnityEvent<Interaction> _onInteracted = new();

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

    private void Awake()
    {
        _renderers = GetComponentsInChildren<SpriteRenderer>();
        _sortingGroup = GetComponent<SortingGroup>();
    }

    private void Start()
    {
        foreach (var _renderer in _renderers)
            _defaultColors.Add(_renderer, _renderer.color);
    }

    public void SetFocus(bool value)
    {
        foreach (var renderer in _renderers)
        {
            renderer.color = value ? Color.green : _defaultColors[renderer];
        }
    }
}
