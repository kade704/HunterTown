using UnityEngine;
using UnityEngine.Events;

public class InteractableSelector : MonoBehaviour
{
    private Interactable _selectedInteractable;
    private UnityEvent<Interactable> _onInteractableSelected = new();
    private UnityEvent<Interactable, Interaction> _onInteractableInteracted = new();


    public UnityEvent<Interactable> OnInteractableSelected => _onInteractableSelected;
    public UnityEvent<Interactable, Interaction> OnInteractableInteracted => _onInteractableInteracted;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !UIUtil.IsUIObjectOverPointer() && GameManager.Instance.GetSystem<ConstructionBuilder>().BulidMode == ConstructionBuilder.BuildMode.Select)
        {
            var interactable = GetInteractableOverPointer();
            SelectInteractable(interactable);
            _selectedInteractable = interactable;
        }

        if (_selectedInteractable)
        {
            foreach (var interaction in _selectedInteractable.Interactions)
            {
                if (Input.GetKeyDown(interaction.Key))
                {
                    _onInteractableInteracted.Invoke(_selectedInteractable, interaction);
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
        _onInteractableSelected.Invoke(interactable);
    }
}
