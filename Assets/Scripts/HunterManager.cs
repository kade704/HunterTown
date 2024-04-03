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
        newHunter.DisplayName = "헌터" + hunterCount++;
        newHunter.DefaultHp = Random.Range(10, 50);
        newHunter.DefaultDamage = Random.Range(1, 10);
        _hunters.Add(newHunter);
        UILogger.Instance.LogInfo($"{newHunter.DisplayName}가 마을에 이주했습니다.");
    }
}
