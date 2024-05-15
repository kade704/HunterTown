using UnityEngine;
using UnityEngine.UI;

public class UIEmploymentPanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _closeButton;

    private void Start()
    {
        var interactableSelector = FindObjectOfType<InteractableSelector>();
        interactableSelector.OnInteractableInteracted.AddListener((interactable, interaction) =>
        {
            if (interaction.ID == "#employment")
            {
            }
            else
            {
            }
        });

        _closeButton.onClick.AddListener(() =>
        {
            _panel.SetActive(false);
        });
    }

    private void Initialize(Portal portal)
    {

    }
}
