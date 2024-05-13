using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIFade))]
public class UIInteractablePanel : MonoBehaviour
{
    [SerializeField] private Text _name;
    [SerializeField] private Text _description;
    [SerializeField] private Transform _interactionButtonsParent;
    [SerializeField] private UIInteractionButton _interactionButtonPrefab;

    private UIFade _panel;
    private Interactable _selectedInteractable;
    private InteractableSelector _interactableSelector;


    private void Awake()
    {
        _panel = GetComponent<UIFade>();
        _interactableSelector = FindObjectOfType<InteractableSelector>();

        var InteractableSelector = FindObjectOfType<InteractableSelector>();
        InteractableSelector.OnInteractableSelected.AddListener(OnInteractableClicked);
    }

    private void OnInteractableClicked(Interactable interactable)
    {
        if (_selectedInteractable)
        {
            _selectedInteractable.SetOutline(false);
        }

        if (interactable)
        {
            _panel.FadeIn(10);

            _name.text = interactable.DisplayName;
            _description.text = interactable.Description;

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
                    interactable.SetOutline(false);
                    _panel.FadeOut(10);
                });
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());

            _selectedInteractable = interactable;
            _selectedInteractable.SetOutline(true);
        }
        else
        {
            _panel.FadeOut(10);

            _selectedInteractable = null;
        }
    }
}
