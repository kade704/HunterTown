using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    private TimeSystem _timeSystem;
    private Text _timeText;
    private Toggle _pauseButton;
    private Toggle _resumeButton;
    private Toggle _fastButton;

    private void Awake()
    {
        _timeSystem = GameManager.Instance.GetSystem<TimeSystem>();
        _timeText = transform.Find("TimeText").GetComponent<Text>();

        _pauseButton = transform.Find("SpeedButtons/PauseButton").GetComponent<Toggle>();
        _pauseButton.onValueChanged.AddListener((active) =>
        {
            if (active)
            {
                _timeSystem.Pause();
            }
        });

        _resumeButton = transform.Find("SpeedButtons/ResumeButton").GetComponent<Toggle>();
        _resumeButton.onValueChanged.AddListener((active) =>
        {
            if (active)
            {
                _timeSystem.Resume();
            }
        });

        _fastButton = transform.Find("SpeedButtons/FastButton").GetComponent<Toggle>();
        _fastButton.onValueChanged.AddListener((active) =>
        {
            if (active)
            {
                _timeSystem.Fast();
            }
        });

        _timeSystem.Hour.OnChanged.AddListener(() =>
        {
            var year = _timeSystem.Year.Current;
            var month = _timeSystem.Month.Current;
            var day = _timeSystem.Day.Current;
            var hour = _timeSystem.Hour.Current;

            var clock = (hour < 12 ? "오전" : "오후") + " " + (hour % 12 == 0 ? "12" : hour % 12) + ":00";

            _timeText.text = $"{year}년 {month}월 {day}일   {clock}";
        });
    }
}
