using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    [SerializeField] private Text _time;
    [SerializeField] private Toggle _pauseButton;
    [SerializeField] private Toggle _resumeButton;
    [SerializeField] private Toggle _fastButton;

    private TimeSystem _timeSystem;


    private void Start()
    {
        _timeSystem = GameManager.Instance.GetSystem<TimeSystem>();

        _pauseButton.onValueChanged.AddListener((active) =>
        {
            if (active)
            {
                _timeSystem.Pause();
            }
        });

        _resumeButton.onValueChanged.AddListener((active) =>
        {
            if (active)
            {
                _timeSystem.Resume();
            }
        });

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

            _time.text = $"{year}년 {month}월 {day}일   {clock}";
        });
    }
}
