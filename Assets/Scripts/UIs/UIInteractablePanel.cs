using UnityEngine;
using UnityEngine.UI;

public class UIInteractablePanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Text _name;
    [SerializeField] private Text _description;
    [SerializeField] private Transform _interactionButtonsParent;
    [SerializeField] private UIInteractionButton _interactionButtonPrefab;

    private InteractableSelector _interactableSelector;


    private void Awake()
    {
        _interactableSelector = FindObjectOfType<InteractableSelector>();

        var InteractableSelector = FindObjectOfType<InteractableSelector>();
        InteractableSelector.OnInteractableSelected.AddListener(OnInteractableClicked);
    }

    private void OnInteractableClicked(Interactable interactable)
    {
        if (interactable)
        {
            _panel.SetActive(true);

            _name.text = interactable.DisplayName;
            _description.text = interactable.Description;

            LayoutRebuilder.ForceRebuildLayoutImmediate(_panel.GetComponent<RectTransform>());

            foreach (Transform interactionButton in _interactionButtonsParent)
            {
                Destroy(interactionButton.gameObject);
            }

            foreach (var interaction in interactable.Interactions)
            {
                var interactionButton = Instantiate(_interactionButtonPrefab, _interactionButtonsParent);
                interactionButton.Interaction = interaction;
                interactionButton.OnClick.AddListener(() =>
                {
                    _interactableSelector.OnInteractableInteracted.Invoke(interactable, interaction);
                    interactable.OnInteracted.Invoke(interaction);
                });
            }
        }
        else
        {
            foreach (Transform interactionButton in _interactionButtonsParent)
            {
                Destroy(interactionButton.gameObject);
            }

            _panel.SetActive(false);
        }
    }
}
