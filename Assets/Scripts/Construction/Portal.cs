using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Construction))]
[RequireComponent(typeof(Visitable))]
public class Portal : MonoBehaviour, ISerializable, IDeserializable
{
    [SerializeField] private SpriteRenderer _portalRenderer;
    [SerializeField] private Sprite[] _spriteFrames;
    [SerializeField] private Transform[] _visitorPositions;
    [SerializeField] private SpriteRenderer _progressRenderer;

    private float _defaultPower;
    private float _defaultDanger;
    private float _defaultDifficulty;
    private Ability[] _abilities = new Ability[3];
    private bool _powerVisibility = false;
    private bool _dangerVisibility = false;
    private bool _difficultyVisibility = false;
    private bool[] _abilityVisibilities = new bool[3] { false, false, false };
    private int _startDay;
    private int _waveDay = int.MaxValue;
    private Construction _construction;
    private Visitable _visitable;
    private Interactable _interactable;


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
        set => _defaultPower = value;
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
        set => _defaultDanger = value;
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
        set => _defaultDifficulty = value;
    }
    public Ability[] Abilities
    {
        get => _abilities;
        set => _abilities = value;
    }
    public bool PowerVisibility
    {
        get => _powerVisibility;
        set => _powerVisibility = value;
    }
    public bool DangerVisibility
    {
        get => _dangerVisibility;
        set => _dangerVisibility = value;
    }
    public bool DifficultyVisibility
    {
        get => _difficultyVisibility;
        set => _difficultyVisibility = value;
    }

    public bool[] AbilityVisibilities
    {
        get => _abilityVisibilities;
        set => _abilityVisibilities = value;
    }
    public Construction Construction => _construction;
    public Visitable Visitable => _visitable;

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
        _interactable = GetComponent<Interactable>();
        _visitable = GetComponent<Visitable>();

        _construction.OnBuilded.AddListener(OnBuilded);
        _construction.OnDestroyed.AddListener(OnDestroyed);
    }

    private void Start()
    {
        StartCoroutine(AnimateRoutine());

        var timeSystem = GameManager.Instance.GetSystem<TimeSystem>();
        timeSystem.Day.OnChanged.AddListener(OnDayChanged);

        _visitable.OnVisitorChanged.AddListener(OnVisitorChanged);

        _interactable.SubDescription = $"포탈 웨이브 까지 {_waveDay - timeSystem.Day.Total}일";
    }

    private void OnBuilded()
    {
        var timeSystem = GameManager.Instance.GetSystem<TimeSystem>();
        _startDay = timeSystem.Day.Total;
        _waveDay = timeSystem.Day.Total + WaveTime;
    }

    private void OnDestroyed()
    {
        var timeSystem = GameManager.Instance.GetSystem<TimeSystem>();
        timeSystem.Day.OnChanged.RemoveListener(OnDayChanged);
    }

    private void OnVisitorChanged()
    {
        for (int i = 0; i < _visitable.VisitedHunters.Length; i++)
        {
            _visitable.VisitedHunters[i].transform.position = _visitorPositions[i].position;
        }
    }

    private void OnDayChanged()
    {
        var timeSystem = GameManager.Instance.GetSystem<TimeSystem>();
        var day = timeSystem.Day.Total;

        var progress = (day - _startDay) / (float)(_waveDay - _startDay);
        _progressRenderer.material.SetFloat("_Value", progress);

        _interactable.SubDescription = $"포탈 웨이브 까지 {_waveDay - day}일";

        if (day >= _waveDay)
        {
            _startDay = timeSystem.Day.Total;
            _waveDay = timeSystem.Day.Total + WaveTime;

            StartCoroutine(WaveRoutine());
        }
    }

    public IEnumerator WaveRoutine()
    {
        GameManager.Instance.GetSystem<CameraMovement>().MovePosition(_construction.transform.position);

        var notificationSystem = GameManager.Instance.GetSystem<NotificationSystem>();

        var message = notificationSystem.NotifyError("포탈 웨이브가 시작되었습니다!", false);

        var monsters = new List<Monster>();
        for (int i = 0; i < 3; i++)
        {
            var monster = GameManager.Instance.GetSystem<MonsterSpawner>().SpawnMonster(_construction.CellPos);
            monsters.Add(monster);
            yield return new WaitForSeconds(1);
        }

        while (monsters.Where(monter => monter != null).Count() > 0)
        {
            yield return new WaitForSeconds(1);
        }

        notificationSystem.RemoveMessage(message);
    }

    public float[] CalcHunterDeathProbability(Hunter[] hunters)
    {
        float[] result = new float[hunters.Length];
        for (int i = 0; i < hunters.Length; i++)
        {
            var probability = 1 - Mathf.Clamp01(hunters[i].Viability / _defaultDanger);
            if (ContainAbility("hunters"))
            {
                probability *= 1.03f;
            }
            probability *= 1 - (hunters.Length - 1) * 0.1f;
            result[i] = Mathf.Clamp01(probability);
        }

        return result;
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
            ["waveDay"] = _waveDay,
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
        _waveDay = token["waveDay"].Value<int>();
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

    public int HunterDispatchReward
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

    public int WaveTime
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

    public int Reward
    {
        get
        {
            var earnedMoney = Power * 10f;
            if (ContainAbility("crystal_portal"))
            {
                earnedMoney *= 1.2f;
            }
            else if (ContainAbility("badlands"))
            {
                earnedMoney *= 0.5f;
            }
            return (int)earnedMoney;
        }
    }
}
