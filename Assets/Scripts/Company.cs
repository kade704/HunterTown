using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Company : MonoBehaviour, ISerializable, IDeserializable
{
    [ReadOnly][SerializeField] private int _remainEmployeeCount;

    private Interactable _interactable;
    private string _defaultDescription;

    public int RemainEmployeeCount
    {
        get => _remainEmployeeCount;
        set => _remainEmployeeCount = value;
    }

    private void Awake()
    {
        _interactable = GetComponent<Interactable>();
    }

    private void Start()
    {
        _defaultDescription = _interactable.Description;
        _interactable.Description = $"{_defaultDescription}\n\n고용 가능 인원: {_remainEmployeeCount}명";

        var timeSystem = GameManager.Instance.GetSystem<TimeSystem>();
        timeSystem.Month.OnChanged.AddListener(() =>
        {
            _remainEmployeeCount += 1;
            _interactable.Description = $"{_defaultDescription}\n고용 가능 인원: {_remainEmployeeCount}명";
            GameManager.Instance.GetSystem<LoggerSystem>().LogInfo($"본부에서 고용 가능 인원이 추가되었습니다.");
        });
    }

    public JToken Serialize()
    {
        var token = new JObject
        {
            ["remain"] = _remainEmployeeCount
        };
        return token;
    }

    public void Deserialize(JToken token)
    {
        _remainEmployeeCount = token["remain"].Value<int>();
    }
}
