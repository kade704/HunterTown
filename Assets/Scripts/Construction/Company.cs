using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Company : MonoBehaviour, ISerializable, IDeserializable
{
    [ReadOnly][SerializeField] private int _remainEmployeeCount;

    private Construction _construction;
    private Interactable _interactable;

    public int RemainEmployeeCount
    {
        get => _remainEmployeeCount;
        set => _remainEmployeeCount = value;
    }
    public Construction Construction => _construction;

    private void Awake()
    {
        _construction = GetComponent<Construction>();
        _interactable = GetComponent<Interactable>();
    }

    private void Start()
    {
        _interactable.SubDescription = $"고용 가능 인원: {_remainEmployeeCount}명";

        var timeSystem = GameManager.Instance.GetSystem<TimeSystem>();
        timeSystem.Day.OnChanged.AddListener(() =>
        {
            var day = timeSystem.Day.Current;
            if (day == 1 || day == 15)
            {
                _remainEmployeeCount += 1;
                GameManager.Instance.GetSystem<NotificationSystem>().NotifyInfo($"본부에서 고용 가능 인원이 추가되었습니다.");
            }
        });
    }

    private void Update()
    {
        _interactable.SubDescription = $"고용 가능 인원: {_remainEmployeeCount}명";
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
