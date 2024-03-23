using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIConstructionInfo : MonoBehaviour {
    private TMP_Text _nameText;
    private Button _destroyButton;
    private CanvasGroup _canvasGroup;

    public Construction Construction {
        set {
            if (value) {
                UIManager.ShowCanvasGroup(_canvasGroup);

                _nameText.text = value.DisplayName;
                _destroyButton.onClick.AddListener(() => {
                    ConstructionManager.Instance.RemoveConstruction(value.CellPos);
                });
            } else {
                UIManager.HideCanvasGroup(_canvasGroup);
                _destroyButton.onClick.RemoveAllListeners();
            }
        }
    }

    private void Awake() {
        _canvasGroup = GetComponent<CanvasGroup>();
        _nameText = transform.Find("NameText").GetComponent<TMP_Text>();
        _destroyButton = transform.Find("DestroyButton").GetComponent<Button>();
    }
}
