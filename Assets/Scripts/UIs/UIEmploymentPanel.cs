using UnityEngine;
using UnityEngine.UI;

public class UIEmploymentPanel : MonoBehaviour
{
    [SerializeField] private Button _closeButton;

    private void Awake()
    {
    }

    void Start()
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
        });
    }
}
