using UnityEngine;
using UnityEngine.UI;

public class UITutorialPanel : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private Button _confirmButton;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _confirmButton = transform.Find("ConfirmButton").GetComponent<Button>();
    }

    private void Start()
    {
        _confirmButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    public void Show()
    {
        UIUtil.ShowCanvasGroup(_canvasGroup);
    }

    public void Hide()
    {
        UIUtil.HideCanvasGroup(_canvasGroup);
    }
}
