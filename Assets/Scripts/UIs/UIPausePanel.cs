using UnityEngine;
using UnityEngine.UI;

public class UIPausePanel : MonoBehaviour
{
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _quitButton;

    private UIFade _fade;
    private bool _isPaused;

    private void Awake()
    {
        _fade = GetComponent<UIFade>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                GameManager.Instance.GetSystem<TimeSystem>().Resume();
                GameManager.Instance.GetSystem<AudioController>().MasterCutoff = 1f;
                _fade.FadeOut();
            }
            else
            {
                GameManager.Instance.GetSystem<TimeSystem>().Pause();
                GameManager.Instance.GetSystem<AudioController>().MasterCutoff = 0.03f;
                _fade.FadeIn();
            }
            _isPaused = !_isPaused;
        }
    }

    private void Start()
    {
        _resumeButton.onClick.AddListener(() =>
        {
            _fade.FadeOut();
            GameManager.Instance.GetSystem<TimeSystem>().Resume();
            _isPaused = false;
        });
        _saveButton.onClick.AddListener(() =>
        {
            GameManager.Instance.SaveGame();
            GameManager.Instance.GetSystem<TimeSystem>().Resume();
            GameManager.Instance.GetSystem<AudioController>().MasterCutoff = 1f;
            _fade.FadeOut();
            _isPaused = false;
        });
        _quitButton.onClick.AddListener(() =>
        {
            GameManager.Instance.GetSystem<AudioController>().MasterCutoff = 1f;
            GameManager.Instance.GetSystem<AudioController>().StopAmbience();
            GameManager.Instance.GetSystem<AudioController>().PlayNextMusic();
            GameManager.Instance.GoMenu();
        });
    }
}
