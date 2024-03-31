using UnityEngine;
using UnityEngine.UI;

public class UIConstructionInfo : MonoBehaviour
{
    private Text _nameText;
    private Button _destroyButton;
    private CanvasGroup _canvasGroup;
    private Construction _selectedConstruction;
    private Transform _interactionButtonsParent;
    private UIInteractionButton _interactionButtonRef;


    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _nameText = transform.Find("NameBG/NameText").GetComponent<Text>();
        _destroyButton = transform.Find("InteractionButtons/DestroyButton").GetComponent<Button>();
        _interactionButtonsParent = transform.Find("InteractionButtons");
        _interactionButtonRef = transform.Find("InteractionButtonRef").GetComponent<UIInteractionButton>();

        ConstructionManager.Instance.OnConstructionClicked.AddListener(OnConstructionClicked);
    }

    private void OnConstructionClicked(Construction construction)
    {
        if (_selectedConstruction)
        {
            _selectedConstruction.SetOutline(false);
        }

        if (construction)
        {
            UIManager.ShowCanvasGroup(_canvasGroup);

            _nameText.text = construction.DisplayName;

            _destroyButton.gameObject.SetActive(construction.Destroyable);
            _destroyButton.onClick.RemoveAllListeners();
            _destroyButton.onClick.AddListener(() =>
            {
                Destroy(construction.gameObject);
                UIManager.HideCanvasGroup(_canvasGroup);
            });

            foreach (Transform interactionButton in _interactionButtonsParent)
            {
                if (interactionButton.name != "DestroyButton")
                    Destroy(interactionButton.gameObject);
            }

            foreach (var interaction in construction.Interactions)
            {
                var interactionButton = Instantiate(_interactionButtonRef, _interactionButtonsParent);
                interactionButton.Interaction = interaction;
                interactionButton.OnClick.AddListener(() =>
                {
                    UIManager.Instance.OnConstructionInteracted.Invoke(interaction.ID, construction);
                    UIManager.HideCanvasGroup(_canvasGroup);
                });
            }

            _selectedConstruction = construction;
            _selectedConstruction.SetOutline(true);
        }
        else
        {
            UIManager.HideCanvasGroup(_canvasGroup);

            _selectedConstruction = null;
        }
    }
}
