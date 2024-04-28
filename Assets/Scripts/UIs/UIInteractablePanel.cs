using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIFade))]
public class UIInteractablePanel : MonoBehaviour
{
    private UIFade _panel;
    private Text _nameText;
    private Interactable _selectedInteractable;
    private Transform _interactionButtonsParent;
    private UIInteractionButton _interactionButtonRef;
    private InteractableSelector _interactableSelector;


    private void Awake()
    {
        _panel = GetComponent<UIFade>();
        _interactableSelector = FindObjectOfType<InteractableSelector>();
        _nameText = transform.Find("NameBG/NameText").GetComponent<Text>();
        _interactionButtonsParent = transform.Find("InteractionButtons");
        _interactionButtonRef = transform.Find("InteractionButtonRef").GetComponent<UIInteractionButton>();

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
            _panel.FadeIn(20);

            _nameText.text = interactable.DisplayName;

            foreach (Transform interactionButton in _interactionButtonsParent)
            {
                Destroy(interactionButton.gameObject);
            }

            foreach (var interaction in interactable.Interactions)
            {
                var interactionButton = Instantiate(_interactionButtonRef, _interactionButtonsParent);
                interactionButton.Interaction = interaction;
                interactionButton.OnClick.AddListener(() =>
                {
                    _interactableSelector.OnInteractableInteracted.Invoke(interactable, interaction);
                    interactable.OnInteracted.Invoke(interaction);
                    interactable.SetOutline(false);
                    _panel.FadeOut(20);
                });
            }

            _selectedInteractable = interactable;
            _selectedInteractable.SetOutline(true);
        }
        else
        {
            _panel.FadeOut(20);

            _selectedInteractable = null;
        }
    }
}
