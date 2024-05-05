using UnityEngine;
using UnityEngine.Events;

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
    private SpriteRenderer[] _spriteRenderers;
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
    public UnityEvent<Interaction> OnInteracted => _onInteracted;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void SetOutline(bool value)
    {
        foreach (var spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.material.SetFloat("_Outline", value ? 1 : 0);
        }
    }
}
