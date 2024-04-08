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
        var day = Timer.Instance.Day.current;
        var hour = Timer.Instance.Hour.current;
        var minute = Timer.Instance.Minute.current;

        _timeText.text = $"2020년 1월 {day}일 {hour}시 {minute}분";
    }
}
