using System.Collections;
using UnityEngine;

public class Portal : Construction
{
    [SerializeField] private float _defaultPower;
    [SerializeField] private bool _powerVisibility;
    [SerializeField] private float _defaultDanger;
    [SerializeField] private bool _dangerVisibility;
    [SerializeField] private float _defaultDifficulty;
    [SerializeField] private bool _difficultyVisibility;
    [SerializeField] private bool[] _abilityVisibilities = new bool[3];
    [SerializeField] private Sprite[] _spriteFrames;

    private bool _isDispatching = false;
    private bool _isExamining = false;
    private SpriteRenderer _progressSprite;

    public float DefaultPower { get { return _defaultPower; } set { _defaultPower = value; } }
    public float DefaultDanger { get { return _defaultDanger; } set { _defaultDanger = value; } }
    public float DefaultDifficulty { get { return _defaultDifficulty; } set { _defaultDifficulty = value; } }
    public bool PowerVisibility { get { return _powerVisibility; } set { _powerVisibility = value; } }
    public bool DangerVisibility { get { return _dangerVisibility; } set { _dangerVisibility = value; } }
    public bool DifficultyVisibility { get { return _difficultyVisibility; } set { _difficultyVisibility = value; } }

    public bool[] AbilityVisibilities => _abilityVisibilities;


    public char Rank
    {
        get
        {
            if (_defaultPower <= 75)
                return 'F';
            else if (_defaultPower <= 150)
                return 'D';
            else if (_defaultPower <= 300)
                return 'D';
            else if (_defaultPower <= 500)
                return 'C';
            else if (_defaultPower <= 700)
                return 'B';
            else if (_defaultPower <= 999)
                return 'A';
            else
                return 'S';
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _progressSprite = transform.Find("Progress").GetComponent<SpriteRenderer>();

        _onInteracted.AddListener((id) =>
         {
             if (id == "examine")
             {
                 Examine();
             }
         });
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(AnimateRoutine());
    }

    public void Dispatch(Hunter[] hunters)
    {

        StartCoroutine(DispatchRoutine(hunters));
    }

    private IEnumerator DispatchRoutine(Hunter[] hunters)
    {
        UILogger.Instance.Log(UILogger.LogType.Info, $"파견이 시작되었습니다.");

        _isDispatching = true;

        foreach (var hunter in hunters)
        {
            hunter.IsDispatched = true;
        }

        _progressSprite.enabled = true;
        var startTime = Time.time;
        var nextTime = startTime + Random.Range(3, 8);
        while (Time.time < nextTime)
        {
            var progressValue = (Time.time - startTime) / (nextTime - startTime);
            _progressSprite.material.SetFloat("_Value", progressValue);
            yield return null;
        }
        _progressSprite.enabled = false;

        for (int i = 0; i < hunters.Length; i++)
        {
            var hunter = hunters[i];
            var deathProbability = CalcHunterDeathProbability(hunter);
            if (Random.value < deathProbability)
            {
                UILogger.Instance.Log(UILogger.LogType.Error, $"{hunter.DisplayName}가 파견중 사망했습니다.");
                HunterManager.Instance.RemoveHunter(hunter);
            }
            hunter.IsDispatched = false;
        }

        var success = CalcDispatchSuccessProbability(hunters);
        if (Random.value <= success)
        {
            UILogger.Instance.Log(UILogger.LogType.Info, $"파견이 성공적으로 완료되었습니다.");
            ConstructionManager.Instance.DestroyConstruction(this);
        }
        else
        {
            UILogger.Instance.Log(UILogger.LogType.Error, $"파견이 실패했습니다.");
        }

        _isDispatching = false;
    }

    public void Examine()
    {
        StartCoroutine(ExamineRoutine());
    }

    public IEnumerator ExamineRoutine()
    {
        UILogger.Instance.Log(UILogger.LogType.Info, $"탐색이 시작되었습니다.");

        _isExamining = true;

        GameManager.Instance.Money -= Random.Range(30, 70);

        _progressSprite.enabled = true;
        var startTime = Time.time;
        var nextTime = startTime + 2;
        while (Time.time < nextTime)
        {
            var progressValue = (Time.time - startTime) / (nextTime - startTime);
            _progressSprite.material.SetFloat("_Value", progressValue);
            yield return null;
        }
        _progressSprite.enabled = false;

        UILogger.Instance.Log(UILogger.LogType.Info, $"탐색이 완료되었습니다.");

        if (!PowerVisibility)
        {
            PowerVisibility = Random.value < 0.5f;
        }
        if (!DangerVisibility)
        {
            DangerVisibility = Random.value < 0.5f;
        }
        if (!DifficultyVisibility)
        {
            DifficultyVisibility = Random.value < 0.5f;
        }

        _isExamining = false;
    }

    public float CalcHunterDeathProbability(Hunter hunter)
    {
        return 1 - Mathf.Clamp01(hunter.Viability / _defaultDanger);
    }

    public float CalcDispatchSuccessProbability(Hunter[] hunters)
    {
        var combatPowerTotal = 0f;
        for (int i = 0; i < hunters.Length; i++)
        {
            combatPowerTotal += hunters[i].CombatPower;
        }
        return (combatPowerTotal / _defaultPower) - (combatPowerTotal / _defaultPower / 6);
    }

    private IEnumerator AnimateRoutine()
    {
        var frameIndex = 0;
        while (true)
        {
            _spriteRenderer.sprite = _spriteFrames[frameIndex];
            frameIndex = (frameIndex + 1) % _spriteFrames.Length;
            yield return new WaitForSeconds(0.3f);
        }
    }
}
