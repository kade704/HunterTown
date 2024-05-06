using System.Collections;
using System.Linq;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, ISerializable, IDeserializable
{
    [ReadOnly] private int _money;

    private UnityEvent<int> _onMoneyChanged = new();

    public UnityEvent<int> OnMoneyChanged => _onMoneyChanged;


    public int Money
    {
        get => _money;
        set
        {
            _money = value;
            OnMoneyChanged.Invoke(value);
        }
    }



    public void Deserialize(JToken token)
    {
        Money = token["money"].Value<int>();
    }

    public JToken Serialize()
    {
        return new JObject
        {
            ["money"] = Money,
        };
    }

}
