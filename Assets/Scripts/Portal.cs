using System.Collections;
using System.Linq;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Construction))]
public class Portal : MonoBehaviour, ISerializable, IDeserializable
{
    [SerializeField] private SpriteRenderer _portalRenderer;
    [SerializeField] private Sprite[] _spriteFrames;
    [SerializeField] private Transform[] _visitorPositions;

    [ReadOnly][SerializeField] private float _defaultPower;
    [ReadOnly][SerializeField] private float _defaultDanger;
    [ReadOnly][SerializeField] private float _defaultDifficulty;
    [ReadOnly][SerializeField] private Ability[] _abilities = new Ability[3];
    [ReadOnly][SerializeField] private bool _powerVisibility = true;
    [ReadOnly][SerializeField] private bool _dangerVisibility = true;
    [ReadOnly][SerializeField] private bool _difficultyVisibility = true;
    [ReadOnly][SerializeField] private bool[] _abilityVisibilities = new bool[3] { true, true, true };
    private bool _isWave = false;
    private Construction _construction;


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

        var waveDay = GameManager.Instance.GetSystem<TimeSystem>().Day.Total + WaveTime;
        GameManager.Instance.GetSystem<TimeSystem>().Day.OnChanged.AddListener(() =>
        {
            if (GameManager.Instance.GetSystem<TimeSystem>().Day.Total >= waveDay && !_isWave)
            {
                Wave();
            }
        });

        _construction.OnVisitorChanged.AddListener(() =>
        {
            for (int i = 0; i < _construction.VisitedHunters.Length; i++)
            {
                _construction.VisitedHunters[i].transform.position = _visitorPositions[i].position;
            }
        });
    }


    private void DispatchRoutine()
    {
        GameManager.Instance.GetSystem<LoggerSystem>().LogInfo($"파견이 시작되었습니다.");


        var HunterSpawner = FindObjectOfType<HunterSpawner>();
        foreach (var hunter in _construction.VisitedHunters)
        {
            var deathProbability = CalcHunterDeathProbability(hunter);
            if (Random.value < deathProbability)
            {
                GameManager.Instance.GetSystem<LoggerSystem>().LogError($"{hunter.DisplayName} 이(가) 파견중 사망했습니다.");
                HunterSpawner.RemoveHunter(hunter);
            }
        }

        var success = CalcDispatchSuccessProbability(_construction.VisitedHunters);
        if (Random.value <= success)
        {
            GameManager.Instance.GetSystem<LoggerSystem>().LogInfo($"파견이 성공적으로 완료되었습니다.");
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
            GameManager.Instance.GetSystem<Player>().Money += (int)earnedMoney;

            foreach (var hunter in _construction.VisitedHunters)
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
            GameManager.Instance.GetSystem<LoggerSystem>().LogError($"파견이 실패했습니다.");
        }
    }


    public void ExamineRoutine()
    {
        GameManager.Instance.GetSystem<LoggerSystem>().LogInfo($"탐색이 시작되었습니다.");

        GameManager.Instance.GetSystem<Player>().Money -= Random.Range(30, 70);


        GameManager.Instance.GetSystem<LoggerSystem>().LogInfo($"탐색이 완료되었습니다.");

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
    }

    public void Wave()
    {
        _isWave = true;
        GameManager.Instance.GetSystem<LoggerSystem>().LogError("포탈 웨이브가 시작되었습니다!");
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
            _portalRenderer.sprite = _spriteFrames[frameIndex];
            frameIndex = (frameIndex + 1) % _spriteFrames.Length;
            yield return new WaitForSeconds(0.3f);
        }
    }

    public JToken Serialize()
    {
        var data = new JObject
        {
            ["power"] = _defaultPower,
            ["danger"] = _defaultDanger,
            ["difficulty"] = _defaultDifficulty,
            ["powerVisibility"] = _powerVisibility,
            ["dangerVisibility"] = _dangerVisibility,
            ["difficultyVisibility"] = _difficultyVisibility,
            ["abilityVisibilities"] = new JArray(_abilityVisibilities)
        };
        var abilities = new JArray();
        foreach (var ability in _abilities)
        {
            if (ability)
            {
                abilities.Add(ability.ID);
            }
        }
        data["abilities"] = abilities;
        return data;
    }

    public void Deserialize(JToken token)
    {
        _defaultPower = token["power"].Value<float>();
        _defaultDanger = token["danger"].Value<float>();
        _defaultDifficulty = token["difficulty"].Value<float>();
        _powerVisibility = token["powerVisibility"].Value<bool>();
        _dangerVisibility = token["dangerVisibility"].Value<bool>();
        _difficultyVisibility = token["difficultyVisibility"].Value<bool>();
        _abilityVisibilities = token["abilityVisibilities"].ToObject<bool[]>();
        var abilities = token["abilities"].ToObject<string[]>();
        for (int i = 0; i < abilities.Length; i++)
        {
            var portalGenerator = GameManager.Instance.GetSystem<PortalGenerator>();
            _abilities[i] = portalGenerator.PortalAbilities.First(x => x.ID == abilities[i]);
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
