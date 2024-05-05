using UnityEngine;
using UnityEngine.UI;

public class UIEmploymentPanel : MonoBehaviour
{
    private UIFade _fade;
    [SerializeField] private Button _closeButton;

    private void Awake()
    {
        _fade = GetComponent<UIFade>();
    }

    void Start()
    {
        var interactableSelector = FindObjectOfType<InteractableSelector>();
        interactableSelector.OnInteractableInteracted.AddListener((interactable, interaction) =>
        {
            if (interaction.ID == "#employment")
            {
                _fade.FadeIn();
            }
            else
            {
                _fade.FadeOut();
            }
        });

        _closeButton.onClick.AddListener(() =>
        {
            _fade.FadeOut();
        });
    }
}
