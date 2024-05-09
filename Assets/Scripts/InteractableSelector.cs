using UnityEngine;
using UnityEngine.Events;

public class InteractableSelector : MonoBehaviour
{
    private UnityEvent<Interactable> _onInteractableSelected = new();
    private UnityEvent<Interactable, Interaction> _onInteractableInteracted = new();

    public UnityEvent<Interactable> OnInteractableSelected => _onInteractableSelected;
    public UnityEvent<Interactable, Interaction> OnInteractableInteracted => _onInteractableInteracted;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !UIManager.IsUIObjectOverPointer() && GameManager.Instance.GetSystem<ConstructionBuilder>().BulidMode == ConstructionBuilder.BuildMode.Select)
        {
            var interactable = GetInteractableOverPointer();
            _onInteractableSelected.Invoke(interactable);
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
            if (frontInteractable == null || interactable.SpriteRenderer.sortingOrder > frontInteractable.SpriteRenderer.sortingOrder)
            {
                frontInteractable = interactable;
            }
        }
        return frontInteractable;
    }
}
