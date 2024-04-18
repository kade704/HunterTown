using UnityEngine;
using UnityEngine.UI;

public class UIConstructionInfo : UIPanel
{
    private Text _nameText;
    private Button _destroyButton;
    private Construction _selectedConstruction;
    private Transform _interactionButtonsParent;
    private UIInteractionButton _interactionButtonRef;


    protected override void Awake()
    {
        base.Awake();

        _nameText = transform.Find("NameBG/NameText").GetComponent<Text>();
        _destroyButton = transform.Find("InteractionButtons/DestroyButton").GetComponent<Button>();
        _interactionButtonsParent = transform.Find("InteractionButtons");
        _interactionButtonRef = transform.Find("InteractionButtonRef").GetComponent<UIInteractionButton>();

        //ConstructionGridMap.Instance.OnConstructionClicked.AddListener(OnConstructionClicked);
    }

    private void OnConstructionClicked(Construction construction)
    {
        if (_selectedConstruction)
        {
            _selectedConstruction.SetOutline(false);
        }

        if (construction)
        {
            ShowPanel(20);

            _nameText.text = construction.DisplayName;

            _destroyButton.gameObject.SetActive(construction.Destroyable);
            _destroyButton.onClick.RemoveAllListeners();
            _destroyButton.onClick.AddListener(() =>
            {
                Destroy(construction.gameObject);
                HidePanel(20);
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
                    construction.OnInteracted.Invoke(interaction.ID);
                    construction.SetOutline(false);
                    HidePanel(20);
                });
            }

            _selectedConstruction = construction;
            _selectedConstruction.SetOutline(true);
        }
        else
        {
            HidePanel(20);

            _selectedConstruction = null;
        }
    }
}
