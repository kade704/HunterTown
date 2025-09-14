using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameOverPanel : MonoBehaviour
{
    private CanvasGroup _panel;
    private Text _messageText;
    private Button _confirmButton;

    private void Awake()
    {
        _panel = GetComponent<CanvasGroup>();
        _messageText = transform.Find("MessageText").GetComponent<Text>();
        _confirmButton = transform.Find("ConfirmButton").GetComponent<Button>();
    }

    private void Start()
    {
        var gameOverSystem = GameManager.Instance.GetSystem<GameOverSystem>();
        gameOverSystem.OnGameOver.AddListener((message) =>
        {
            _messageText.text = message;
            UIUtil.ShowCanvasGroup(_panel);
        });

        _confirmButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Menu");
        });
    }
}
