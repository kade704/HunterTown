using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    private Text _timeText;
    private Toggle _pauseButton;
    private Toggle _resumeButton;
    private Toggle _fastButton;

    private void Awake()
    {
        _timeText = transform.Find("TimeText").GetComponent<Text>();

        _pauseButton = transform.Find("SpeedButtons/PauseButton").GetComponent<Toggle>();
        _pauseButton.onValueChanged.AddListener((active) =>
        {
            if (active)
            {
                Timer.Instance.Pause();
            }
        });

        _resumeButton = transform.Find("SpeedButtons/ResumeButton").GetComponent<Toggle>();
        _resumeButton.onValueChanged.AddListener((active) =>
        {
            if (active)
            {
                Timer.Instance.Resume();
            }
        });

        _fastButton = transform.Find("SpeedButtons/FastButton").GetComponent<Toggle>();
        _fastButton.onValueChanged.AddListener((active) =>
        {
            if (active)
            {
                Timer.Instance.Fast();
            }
        });
    }
    private void Update()
    {
        var year = Timer.Instance.Year.current;
        var month = Timer.Instance.Month.current;
        var day = Timer.Instance.Day.current;
        var hour = Timer.Instance.Hour.current;

        var clock = (hour < 12 ? "오전" : "오후") + " " + (hour % 12 == 0 ? "12" : hour % 12) + ":00";

        _timeText.text = $"{year}년 {month}월 {day}일   {clock}";
    }
}
