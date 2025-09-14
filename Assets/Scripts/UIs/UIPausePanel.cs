using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPausePanel : MonoBehaviour
{
    private CanvasGroup _panel;
    private Button _resumeButton;
    private Button _saveButton;
    private Button _menuButton;
    private Button _quitButton;

    private void Awake()
    {
        _panel = GetComponent<CanvasGroup>();
        _resumeButton = transform.Find("Buttons/ResumeButton").GetComponent<Button>();
        _saveButton = transform.Find("Buttons/SaveButton").GetComponent<Button>();
        _menuButton = transform.Find("Buttons/MenuButton").GetComponent<Button>();
        _quitButton = transform.Find("Buttons/QuitButton").GetComponent<Button>();
    }

    private void Start()
    {
        _resumeButton.onClick.AddListener(() =>
        {
            Hide();
        });
        _saveButton.onClick.AddListener(() =>
        {
            GameManager.Instance.GetSystem<SaveFileSystem>().SaveGame();
            Hide();
        });
        _menuButton.onClick.AddListener(() =>
        {
            GameManager.Instance.GetSystem<AudioController>().MasterCutoff = 1f;
            GameManager.Instance.GetSystem<AudioController>().StopAmbience();
            GameManager.Instance.GetSystem<AudioController>().PlayNextMusic();
            SceneManager.LoadScene("Menu");
        });
        _quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    public void Hide()
    {
        GameManager.Instance.GetSystem<TimeSystem>().Resume();
        GameManager.Instance.GetSystem<AudioController>().MasterCutoff = 1f;
        UIUtil.HideCanvasGroup(_panel);
    }

    public void Show()
    {
        GameManager.Instance.GetSystem<TimeSystem>().Pause();
        GameManager.Instance.GetSystem<AudioController>().MasterCutoff = 0.03f;
        UIUtil.ShowCanvasGroup(_panel);
    }
}
