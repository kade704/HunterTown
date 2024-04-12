using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Timer : Singleton<Timer>
{
    public struct Element
    {
        public Element(int start)
        {
            this.current = this.total = start;
            this.OnChanged = new UnityEvent();
        }

        public int current;
        public int total;
        public UnityEvent OnChanged;

        public bool Increase(int maximum)
        {
            current++;
            total++;
            OnChanged.Invoke();
            if (current >= maximum)
            {
                current = 0;
                return true;
            }
            return false;
        }
    }

    [SerializeField] private float _timeScale = 1f;

    private Element _hour = new Element(0);
    private Element _day = new Element(1);
    private Element _month = new Element(1);
    private Element _year = new Element(2020);

    public Element Hour => _hour;
    public Element Day => _day;
    public Element Month => _month;
    public Element Year => _year;
    public float TimeScale => _timeScale;


    private void Start()
    {
        StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        while (true)
        {
            var nextDay = _hour.Increase(24);
            if (nextDay)
            {
                var nextMonth = _day.Increase(30);
                if (nextMonth)
                {
                    _month.Increase(12);
                }
            }
            yield return new WaitForSeconds((9f / 24f) * (1 / _timeScale));
        }
    }

    public void Pause()
    {
        _timeScale = 0;

        StopAllCoroutines();
    }

    public void Resume()
    {
        _timeScale = 1;

        StopAllCoroutines();
        StartCoroutine(TimerRoutine());
    }

    public void Fast()
    {
        _timeScale = 10;

        StopAllCoroutines();
        StartCoroutine(TimerRoutine());
    }
}
