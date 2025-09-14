using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TimeSystem : MonoBehaviour, ISerializable, IDeserializable
{
    public struct Element
    {
        public Element(int start)
        {
            _current = _total = start;
            _onChanged = new UnityEvent();
        }

        private int _current;
        private int _total;
        private UnityEvent _onChanged;

        public int Current
        {
            readonly get => _current;
            set
            {
                _current = value;
                OnChanged.Invoke();
            }
        }

        public int Total
        {
            readonly get => _total;
            set
            {
                _total = value;
                OnChanged.Invoke();
            }
        }

        public readonly UnityEvent OnChanged => _onChanged;

        public bool Increase(int maximum)
        {
            _current++;
            _total++;
            OnChanged.Invoke();
            if (_current >= maximum)
            {
                _current = 0;
                return true;
            }
            return false;
        }
    }

    [SerializeField] private float _timeScale = 1f;

    private Element _hour = new(0);
    private Element _day = new(1);
    private Element _month = new(1);
    private Element _year = new(2020);
    private UnityEvent _onTimeScaleChanged = new UnityEvent();

    public Element Hour => _hour;
    public Element Day => _day;
    public Element Month => _month;
    public Element Year => _year;
    public float TimeScale => _timeScale;
    public UnityEvent OnTimeScaleChanged => _onTimeScaleChanged;


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

        _onTimeScaleChanged.Invoke();
    }

    public void Resume()
    {
        _timeScale = 1;

        StopAllCoroutines();
        StartCoroutine(TimerRoutine());

        _onTimeScaleChanged.Invoke();
    }

    public void Fast()
    {
        _timeScale = 5;

        StopAllCoroutines();
        StartCoroutine(TimerRoutine());

        _onTimeScaleChanged.Invoke();
    }

    public JToken Serialize()
    {
        var obj = new JObject
        {
            ["hourCurrent"] = _hour.Current,
            ["hourTotal"] = _hour.Total,
            ["dayCurrent"] = _day.Current,
            ["dayTotal"] = _day.Total,
            ["monthCurrent"] = _month.Current,
            ["monthTotal"] = _month.Total,
            ["yearCurrent"] = _year.Current,
            ["yearTotal"] = _year.Total,
        };
        return obj;
    }

    public void Deserialize(JToken token)
    {
        _hour.Current = token["hourCurrent"].Value<int>();
        _hour.Total = token["hourTotal"].Value<int>();
        _hour.OnChanged.Invoke();

        _day.Current = token["dayCurrent"].Value<int>();
        _day.Total = token["dayTotal"].Value<int>();
        _day.OnChanged.Invoke();

        _month.Current = token["monthCurrent"].Value<int>();
        _month.Total = token["monthTotal"].Value<int>();
        _month.OnChanged.Invoke();

        _year.Current = token["yearCurrent"].Value<int>();
        _year.Total = token["yearTotal"].Value<int>();
        _year.OnChanged.Invoke();
    }
}
