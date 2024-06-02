using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameClearPanel : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private Button _confirmButton;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _confirmButton = transform.Find("ConfirmButton").GetComponent<Button>();
    }

    void Start()
    {
        var gameClearSystem = GameManager.Instance.GetSystem<GameClearSystem>();
        gameClearSystem.OnGameClear.AddListener(() =>
        {
            UIUtil.ShowCanvasGroup(_canvasGroup);
        });

        _confirmButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Menu");
        });
    }
}
