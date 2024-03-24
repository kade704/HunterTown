using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIConstructionInfo : MonoBehaviour {
    private TMP_Text _nameText;
    private Button _destroyButton;
    private CanvasGroup _canvasGroup;
    private Construction _selectedConstruction;

    private void Awake() {
        _canvasGroup = GetComponent<CanvasGroup>();
        _nameText = transform.Find("NameText").GetComponent<TMP_Text>();
        _destroyButton = transform.Find("DestroyButton").GetComponent<Button>();

        ConstructionManager.Instance.OnConstructionClicked.AddListener(OnConstructionClicked);
    }

    private void OnConstructionClicked(Construction construction) {
        if (_selectedConstruction) {
            _selectedConstruction.SetOutline(false);
        }

        if (construction) {
            UIManager.ShowCanvasGroup(_canvasGroup);

            _nameText.text = construction.DisplayName;
            _destroyButton.onClick.RemoveAllListeners();
            _destroyButton.onClick.AddListener(() => {
                construction.DestroyThis();
                UIManager.HideCanvasGroup(_canvasGroup);
            });

            _selectedConstruction = construction;
            _selectedConstruction.SetOutline(true);
        } else {
            UIManager.HideCanvasGroup(_canvasGroup);

            _selectedConstruction = null;
        }
    }
}
