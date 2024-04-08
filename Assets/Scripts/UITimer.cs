using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    private Text _timeText;
    private Button _pauseButton;
    private Button _resumeButton;
    private Button _fastButton;

    private void Awake()
    {
        _timeText = transform.Find("TimeText").GetComponent<Text>();

        _pauseButton = transform.Find("PauseButton").GetComponent<Button>();
        _pauseButton.onClick.AddListener(() =>
        {
            Timer.Instance.Pause();
        });

        _resumeButton = transform.Find("ResumeButton").GetComponent<Button>();
        _resumeButton.onClick.AddListener(() =>
        {
            Timer.Instance.Resume();
        });

        _fastButton = transform.Find("FastButton").GetComponent<Button>();
        _fastButton.onClick.AddListener(() =>
        {
            Timer.Instance.Fast();
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
