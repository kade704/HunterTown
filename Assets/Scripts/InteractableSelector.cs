using UnityEngine;
using UnityEngine.Events;

public class InteractableSelector : MonoBehaviour
{
    private bool _enableSelect = true;
    private Interactable _selectedInteractable;
    private Interactable _mouseOverInteractable;
    private UnityEvent<Interactable> _onInteractableMouseEnter = new();
    private UnityEvent<Interactable> _onInteractableMouseExit = new();
    private UnityEvent<Interactable> _onInteractableSelected = new();
    private UnityEvent<Interactable> _onInteractableDeselected = new();
    private UnityEvent<Interactable, Interaction> _onInteractableInteracted = new();

    public Interactable SelectedInteractable => _selectedInteractable;
    public UnityEvent<Interactable> OnInteractableMouseEnter => _onInteractableMouseEnter;
    public UnityEvent<Interactable> OnInteractableMouseExit => _onInteractableMouseExit;
    public UnityEvent<Interactable> OnInteractableSelected => _onInteractableSelected;
    public UnityEvent<Interactable> OnInteractableDeselected => _onInteractableDeselected;
    public UnityEvent<Interactable, Interaction> OnInteractableInteracted => _onInteractableInteracted;
    public bool EnableSelect
    {
        get => _enableSelect;
        set => _enableSelect = value;
    }

    void Update()
    {
        var interactable = GetInteractableOverPointer();
        if (interactable != _mouseOverInteractable)
        {
            if (_mouseOverInteractable)
            {
                _onInteractableMouseExit.Invoke(_mouseOverInteractable);
                _mouseOverInteractable.OnMouseExit.Invoke();
            }

            if (interactable)
            {
                _onInteractableMouseEnter.Invoke(interactable);
                interactable.OnMouseEnter.Invoke();
            }

            _mouseOverInteractable = interactable;
        }

        if (Input.GetMouseButtonDown(0) && !UIUtil.IsUIObjectOverPointer() && EnableSelect)
        {
            SelectInteractable(interactable);
        }

        if (_selectedInteractable)
        {
            foreach (var interaction in _selectedInteractable.Interactions)
            {
                if (Input.GetKeyDown(interaction.Key))
                {
                    _onInteractableInteracted.Invoke(_selectedInteractable, interaction);
                    _selectedInteractable.OnInteracted.Invoke(interaction);
                }
            }
        }
    }

    private Interactable GetInteractableOverPointer()
    {
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var colliders = Physics2D.OverlapPointAll(position);

        Interactable frontInteractable = null;
        foreach (var collider in colliders)
        {
            var interactable = collider.GetComponent<Interactable>();
            if (frontInteractable == null || frontInteractable.SortingGroup.sortingOrder < interactable.SortingGroup.sortingOrder)
            {
                frontInteractable = interactable;
            }
        }
        return frontInteractable;
    }

    public void SelectInteractable(Interactable interactable)
    {
        if (_selectedInteractable)
        {
            _selectedInteractable.OnDeselected.Invoke();
            _onInteractableDeselected.Invoke(_selectedInteractable);
        }

        if (interactable != null)
        {
            _selectedInteractable = interactable;
            _selectedInteractable.OnSelected.Invoke();
            _onInteractableSelected.Invoke(_selectedInteractable);
        }
        else
        {
            _selectedInteractable = null;
        }
    }
}
