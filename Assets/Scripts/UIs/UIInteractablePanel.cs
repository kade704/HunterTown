using UnityEngine;
using UnityEngine.UI;

public class UIInteractablePanel : MonoBehaviour
{
    [SerializeField] private UIInteractionButton _interactionButtonPrefab;

    private CanvasGroup _panel;
    private Text _nameText;
    private Text _descriptionText;
    private Text _subDescriptionText;
    private Transform _interactionButtonContainer;
    private InteractableSelector _interactableSelector;


    private void Awake()
    {
        _panel = transform.Find("Panel").GetComponent<CanvasGroup>();
        _nameText = transform.Find("Panel/NameText").GetComponent<Text>();
        _descriptionText = transform.Find("Panel/DescriptionText").GetComponent<Text>();
        _subDescriptionText = transform.Find("Panel/SubDescriptionText").GetComponent<Text>();
        _interactionButtonContainer = transform.Find("InteractionButtons");

        _interactableSelector = FindObjectOfType<InteractableSelector>();

        var InteractableSelector = FindObjectOfType<InteractableSelector>();
        InteractableSelector.OnInteractableSelected.AddListener(OnInteractableSelected);
        InteractableSelector.OnInteractableDeselected.AddListener(OnInteractableDeselected);
    }

    private void Update()
    {
        if (_interactableSelector.SelectedInteractable)
        {
            _subDescriptionText.text = _interactableSelector.SelectedInteractable.SubDescription;
        }
    }

    private void OnInteractableSelected(Interactable interactable)
    {
        UIUtil.ShowCanvasGroup(_panel);

        _nameText.text = interactable.DisplayName;
        _descriptionText.text = interactable.Description;

        LayoutRebuilder.ForceRebuildLayoutImmediate(_panel.GetComponent<RectTransform>());

        foreach (Transform interactionButton in _interactionButtonContainer)
        {
            Destroy(interactionButton.gameObject);
        }

        foreach (var interaction in interactable.Interactions)
        {
            var interactionButton = Instantiate(_interactionButtonPrefab, _interactionButtonContainer);
            interactionButton.Interaction = interaction;
            interactionButton.OnClick.AddListener(() =>
            {
                _interactableSelector.OnInteractableInteracted.Invoke(interactable, interaction);
                interactable.OnInteracted.Invoke(interaction);
            });
        }
    }

    private void OnInteractableDeselected(Interactable interactable)
    {
        foreach (Transform interactionButton in _interactionButtonContainer)
        {
            Destroy(interactionButton.gameObject);
        }

        UIUtil.HideCanvasGroup(_panel);
    }
}
