using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class HunterManager : MonoBehaviour
{
    private static HunterManager _instance;
    private List<Hunter> _hunters = new();

    public static HunterManager Instance => _instance;
    public Hunter[] Hunters => _hunters.ToArray();

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        AddRandomHunter();
        AddRandomHunter();
        AddRandomHunter();
        AddRandomHunter();
        AddRandomHunter();
        AddRandomHunter();
    }

    static int hunterCount = 1;
    public void AddRandomHunter()
    {
        var hunterPrefab = Resources.Load<Hunter>("Hunter");
        var newHunter = Instantiate(hunterPrefab, transform);
        var day = Timer.Instance.Day.total;
        var stat = 13 + (day * day / 1500);
        var hp = Random.Range(0, stat + 1 - 6) + 3;
        var damage = stat - hp;
        newHunter.DisplayName = "헌터" + hunterCount++;
        newHunter.DefaultHp = hp;
        newHunter.DefaultDamage = damage;
        _hunters.Add(newHunter);
        UILogger.Instance.Log(UILogger.LogType.Info, $"{newHunter.DisplayName}가 마을에 이주했습니다.");
    }

    public void RemoveHunter(Hunter hunter)
    {
        _hunters.Remove(hunter);
        Destroy(hunter.gameObject);
    }
}
