using UnityEngine;
using UnityEngine.UI;

public class UIPausePanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _quitButton;


    private bool _isPaused;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                GameManager.Instance.GetSystem<TimeSystem>().Resume();
                GameManager.Instance.GetSystem<AudioController>().MasterCutoff = 1f;
                _panel.SetActive(false);
            }
            else
            {
                GameManager.Instance.GetSystem<TimeSystem>().Pause();
                GameManager.Instance.GetSystem<AudioController>().MasterCutoff = 0.03f;
                _panel.SetActive(true);
            }
            _isPaused = !_isPaused;
        }
    }

    private void Start()
    {
        _resumeButton.onClick.AddListener(() =>
        {
            _panel.SetActive(false);
            GameManager.Instance.GetSystem<TimeSystem>().Resume();
            _isPaused = false;
        });
        _saveButton.onClick.AddListener(() =>
        {
            GameManager.Instance.SaveGame();
            GameManager.Instance.GetSystem<TimeSystem>().Resume();
            GameManager.Instance.GetSystem<AudioController>().MasterCutoff = 1f;
            _panel.SetActive(false);
            _isPaused = false;
        });
        _menuButton.onClick.AddListener(() =>
        {
            GameManager.Instance.GetSystem<AudioController>().MasterCutoff = 1f;
            GameManager.Instance.GetSystem<AudioController>().StopAmbience();
            GameManager.Instance.GetSystem<AudioController>().PlayNextMusic();
            GameManager.Instance.GoMenu();
        });
        _quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
