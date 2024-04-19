using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Construction))]
[RequireComponent(typeof(SpriteRenderer))]
public class Portal : MonoBehaviour
{
    [SerializeField] private Sprite[] _spriteFrames;

    private float _defaultPower;
    private float _defaultDanger;
    private float _defaultDifficulty;
    private Ability[] _abilities = new Ability[3];
    private bool _powerVisibility = false;
    private bool _dangerVisibility = false;
    private bool _difficultyVisibility = false;
    private bool[] _abilityVisibilities = new bool[3];
    private bool _isDispatching = false;
    private bool _isExamining = false;
    private bool _isWave = false;
    private Construction _construction;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _progressSprite;
    private ParticleSystem _fireParticle;

    public float Power
    {
        get
        {
            var power = _defaultPower;
            if (ContainAbility("false_evolution"))
            {
                power *= 0.9f;
            }
            else if (ContainAbility("battle_creatures"))
            {
                power *= 1.2f;
            }
            return power;
        }
        set { _defaultPower = value; }
    }

    public float Danger
    {
        get
        {
            var danger = _defaultDanger;
            if (ContainAbility("peaceful_gate"))
            {
                danger *= 0.9f;
            }
            else if (ContainAbility("toxic_atmosphere"))
            {
                danger *= 1.2f;
            }
            return danger;
        }
        set { _defaultDanger = value; }
    }
    public float Difficulty
    {
        get
        {
            var difficulty = _defaultDifficulty;
            if (ContainAbility("landmark"))
            {
                difficulty *= 0.9f;
            }
            else if (ContainAbility("maze"))
            {
                difficulty *= 1.2f;
            }
            return difficulty;
        }
        set { _defaultDifficulty = value; }
    }
    public Ability[] Abilities
    {
        get { return _abilities; }
        set { _abilities = value; }
    }
    public bool PowerVisibility => _powerVisibility;
    public bool DangerVisibility => _dangerVisibility;
    public bool DifficultyVisibility => _difficultyVisibility;

    public bool[] AbilityVisibilities => _abilityVisibilities;
    public Construction Construction => _construction;

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

    private void Awake()
    {
        _construction = GetComponent<Construction>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _progressSprite = transform.Find("Progress").GetComponent<SpriteRenderer>();
        _fireParticle = transform.Find("Fire").GetComponent<ParticleSystem>();

        // _onInteracted.AddListener((id) =>
        //  {
        //      if (id == "examine")
        //      {
        //          Examine();
        //      }
        //  });
    }

    private void Start()
    {
        StartCoroutine(AnimateRoutine());

        var waveDay = Timer.Instance.Day.total + WaveTime;
        Timer.Instance.Day.OnChanged.AddListener(() =>
        {
            if (Timer.Instance.Day.total >= waveDay && !_isWave)
            {
                Wave();
            }
        });
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

        var HunterSpawner = FindObjectOfType<HunterSpawner>();
        foreach (var hunter in hunters)
        {
            var deathProbability = CalcHunterDeathProbability(hunter);
            if (Random.value < deathProbability)
            {
                UILogger.Instance.Log(UILogger.LogType.Error, $"{hunter.DisplayName} 이(가) 파견중 사망했습니다.");
                HunterSpawner.RemoveHunter(hunter);
            }
            hunter.IsDispatched = false;
        }

        var success = CalcDispatchSuccessProbability(hunters);
        if (Random.value <= success)
        {
            UILogger.Instance.Log(UILogger.LogType.Info, $"파견이 성공적으로 완료되었습니다.");
            _construction.ConstructionGridMap.DestroyConstruction(_construction);
            var earnedMoney = Power * 10f;
            if (ContainAbility("crystal_portal"))
            {
                earnedMoney *= 1.2f;
            }
            else if (ContainAbility("badlands"))
            {
                earnedMoney *= 0.5f;
            }
            GameManager.Instance.Money += (int)earnedMoney;

            foreach (var hunter in hunters)
            {
                var reward = HunterDispatchReward;
                var damage = Random.Range(0, reward);
                var hp = reward - damage;

                hunter.DefaultDamage += damage;
                hunter.DefaultHp += hp;
            }
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

        if (ContainAbility("understood_world"))
        {
            _powerVisibility = true;
            _dangerVisibility = true;
            _difficultyVisibility = true;
            for (int i = 0; i < 3; i++)
            {
                _abilityVisibilities[i] = true;
            }
        }
        else
        {
            if (!_powerVisibility)
            {
                _powerVisibility = Random.value < 0.5f;
            }
            if (!_dangerVisibility)
            {
                _dangerVisibility = Random.value < 0.5f;
            }
            if (!_difficultyVisibility)
            {
                _difficultyVisibility = Random.value < 0.5f;
            }
            for (int i = 0; i < 3; i++)
            {
                if (!_abilityVisibilities[i])
                {
                    _abilityVisibilities[i] = Random.value < 0.5f;
                }
            }
        }

        _isExamining = false;
    }

    public void Wave()
    {
        _isWave = true;
        _fireParticle.Play();
        UILogger.Instance.Log(UILogger.LogType.Error, "포탈 웨이브가 시작되었습니다!");
    }

    public float CalcHunterDeathProbability(Hunter hunter)
    {
        var probability = 1 - Mathf.Clamp01(hunter.Viability / _defaultDanger);
        if (ContainAbility("hunters"))
        {
            probability *= 1.1f;
        }
        return probability;
    }

    public float CalcDispatchSuccessProbability(Hunter[] hunters)
    {
        var combatPowerTotal = 0f;
        for (int i = 0; i < hunters.Length; i++)
        {
            combatPowerTotal += hunters[i].CombatPower;
        }
        var probability = (combatPowerTotal / _defaultPower) - (combatPowerTotal / _defaultPower / 6);
        if (ContainAbility("strong_boss"))
        {
            probability *= 0.95f;
        }
        return probability;
    }

    public bool ContainAbility(string id)
    {
        for (int i = 0; i < 3; i++)
        {
            if (_abilities[i] && _abilities[i].ID == id)
            {
                return true;
            }
        }
        return false;
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

    private int HunterDispatchReward
    {
        get
        {
            return Rank switch
            {
                'F' => Random.Range(1, 3),
                'E' => Random.Range(2, 4),
                'D' => Random.Range(3, 5),
                'C' => Random.Range(4, 6),
                'B' => Random.Range(5, 8),
                'A' => Random.Range(7, 9),
                'S' => Random.Range(8, 10),
                _ => 0,
            };
        }
    }

    private int WaveTime
    {
        get
        {
            return Rank switch
            {
                'F' => Random.Range(21, 35),
                'E' => Random.Range(28, 42),
                'D' => Random.Range(35, 49),
                'C' => Random.Range(42, 56),
                'B' => Random.Range(49, 63),
                'A' => Random.Range(56, 70),
                'S' => Random.Range(63, 77),
                _ => 0,
            };
        }
    }
}
