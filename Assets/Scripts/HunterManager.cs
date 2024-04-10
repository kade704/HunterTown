using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HunterManager : Singleton<HunterManager>
{
    [SerializeField] private string[] _hunterNames;
    private List<Hunter> _hunters = new();
    private List<string> _hunterNamesLeft = new();
    private Sprite[] _clothSprites;
    private Sprite[] _leftSleeveSprites;
    private Sprite[] _rightSleeveSprites;
    private Sprite[] _hairSprites;

    public Hunter[] Hunters => _hunters.ToArray();

    protected override void Awake()
    {
        base.Awake();

        var clothGroup = Resources.LoadAll<Sprite>("SPUM/SPUM_Sprites/Items/2_Cloth");
        _clothSprites = clothGroup.Where(x => x.name == "Body").ToArray();
        _leftSleeveSprites = clothGroup.Where(x => x.name == "Left").ToArray();
        _rightSleeveSprites = clothGroup.Where(x => x.name == "Right").ToArray();
        _hairSprites = Resources.LoadAll<Sprite>("SPUM/SPUM_Sprites/Items/0_Hair");
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

        newHunter.ClothSprite = _clothSprites[Random.Range(0, _clothSprites.Length)];
        newHunter.HairSprite = _hairSprites[Random.Range(0, _hairSprites.Length)];
        newHunter.LeftSleeveSprite = _leftSleeveSprites[Random.Range(0, _leftSleeveSprites.Length)];
        newHunter.RightSleeveSprite = _rightSleeveSprites[Random.Range(0, _rightSleeveSprites.Length)];
        newHunter.HairColor = Random.ColorHSV(0, 1, 0.4f, 0.6f, 0.5f, 1f);

        _hunters.Add(newHunter);
        UILogger.Instance.Log(UILogger.LogType.Info, $"<b>{newHunter.DisplayName}</b> 이(가) 마을에 이주했습니다.");
    }

    public void RemoveHunter(Hunter hunter)
    {
        _hunters.Remove(hunter);
        Destroy(hunter.gameObject);
    }


}
