using UnityEngine;
using UnityEngine.UI;

public class UITime : MonoBehaviour
{
    private Text _timeText;
    private Image _progressImage;
    private Toggle _pauseButton;
    private Toggle _resumeButton;
    private Toggle _fastButton;

    private void Awake()
    {
        _timeText = transform.Find("TimeText").GetComponent<Text>();
        _progressImage = transform.Find("ProgressImage").GetComponent<Image>();
        _pauseButton = transform.Find("SpeedButtons/PauseButton").GetComponent<Toggle>();
        _resumeButton = transform.Find("SpeedButtons/ResumeButton").GetComponent<Toggle>();
        _fastButton = transform.Find("SpeedButtons/FastButton").GetComponent<Toggle>();
    }

    private void Start()
    {
        var timeSystem = GameManager.Instance.GetSystem<TimeSystem>();
        _timeText.text = $"{timeSystem.Year.Current}년 {timeSystem.Month.Current}월 {timeSystem.Day.Current}일";

        _pauseButton.onValueChanged.AddListener((active) =>
        {
            if (active)
            {
                timeSystem.Pause();
            }
        });

        _resumeButton.onValueChanged.AddListener((active) =>
        {
            if (active)
            {
                timeSystem.Resume();
            }
        });

        _fastButton.onValueChanged.AddListener((active) =>
        {
            if (active)
            {
                timeSystem.Fast();
            }
        });

        timeSystem.Day.OnChanged.AddListener(() =>
        {
            _timeText.text = $"{timeSystem.Year.Current}년 {timeSystem.Month.Current}월 {timeSystem.Day.Current}일";
        });

        timeSystem.Hour.OnChanged.AddListener(() =>
        {
            _progressImage.fillAmount = timeSystem.Hour.Current / 24f;
        });


        timeSystem.OnTimeScaleChanged.AddListener(() =>
        {
            _pauseButton.SetIsOnWithoutNotify(timeSystem.TimeScale == 0);
            _resumeButton.SetIsOnWithoutNotify(timeSystem.TimeScale == 1);
            _fastButton.SetIsOnWithoutNotify(timeSystem.TimeScale == 5);
        });
    }
}
