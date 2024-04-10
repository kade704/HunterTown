using System.Collections.Generic;
using UnityEngine;

public class HunterManager : Singleton<HunterManager>
{
    [SerializeField] private string[] _hunterNames;
    private List<Hunter> _hunters = new();
    private List<string> _hunterNamesLeft = new();

    public Hunter[] Hunters => _hunters.ToArray();

    protected override void Awake()
    {
        _hunterNamesLeft.AddRange(_hunterNames);
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

    public void AddRandomHunter()
    {
        var hunterPrefab = Resources.Load<Hunter>("Hunter");
        var newHunter = Instantiate(hunterPrefab, transform);
        var day = Timer.Instance.Day.total;
        var stat = 13 + (day * day / 1500);
        var hp = Random.Range(0, stat + 1 - 6) + 3;
        var damage = stat - hp;

        var nameIdx = Random.Range(0, _hunterNamesLeft.Count);
        var name = _hunterNamesLeft[nameIdx];
        _hunterNamesLeft.RemoveAt(nameIdx);

        newHunter.DisplayName = name;
        newHunter.DefaultHp = hp;
        newHunter.DefaultDamage = damage;
        _hunters.Add(newHunter);
        UILogger.Instance.Log(UILogger.LogType.Info, $"<b>{newHunter.DisplayName}</b> 이(가) 마을에 이주했습니다.");
    }

    public void RemoveHunter(Hunter hunter)
    {
        _hunters.Remove(hunter);
        Destroy(hunter.gameObject);
    }
}
