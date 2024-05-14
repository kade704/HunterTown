using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;

public class HunterSpawner : MonoBehaviour, ISerializable, IDeserializable
{
    [SerializeField] private Hunter _hunterPrefab;
    [SerializeField] private string[] _hunterNames;
    private List<Hunter> _hunters = new();
    private List<string> _hunterNamesLeft = new();
    private Sprite[] _hairSprites;
    private Sprite[] _clothSprites;
    private Sprite[] _pantSprites;
    private UnityEvent _onHuntersChanged = new();

    public Hunter[] Hunters => _hunters.ToArray();
    public UnityEvent OnHuntersChanged => _onHuntersChanged;


    private void Awake()
    {
        _hairSprites = Resources.LoadAll<Sprite>("Hunters/Hairs");
        _clothSprites = Resources.LoadAll<Sprite>("Hunters/Clothes");
        _pantSprites = Resources.LoadAll<Sprite>("Hunters/Pants");

        _hunterNamesLeft.AddRange(_hunterNames);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            SpawnRandomHunter();
        }
    }

    public void SpawnRandomHunter()
    {
        var newHunter = Instantiate(_hunterPrefab, transform);
        var day = GameManager.Instance.GetSystem<TimeSystem>().Day.Total;
        var stat = 13 + (day * day / 1500);
        var hp = Random.Range(0, stat + 1 - 6) + 3;
        var damage = stat - hp;

        var nameIdx = Random.Range(0, _hunterNamesLeft.Count);
        var name = _hunterNamesLeft[nameIdx];
        _hunterNamesLeft.RemoveAt(nameIdx);

        newHunter.DisplayName = name;
        newHunter.DefaultHp = hp;
        newHunter.DefaultDamage = damage;

        newHunter.AvatarCustomize.HairSprite = _hairSprites[Random.Range(0, _hairSprites.Length)];
        newHunter.AvatarCustomize.HairColor = Random.ColorHSV(0, 1, 0.4f, 0.6f, 0.5f, 1f);

        var clothIdx = Random.Range(1, 12);
        newHunter.AvatarCustomize.BodyClothSprite = _clothSprites.Where(s => s.name == $"Cloth{clothIdx}_Body").FirstOrDefault();
        newHunter.AvatarCustomize.LeftSleeveSprite = _clothSprites.Where(s => s.name == $"Cloth{clothIdx}_Left").FirstOrDefault();
        newHunter.AvatarCustomize.RightSleeveSprite = _clothSprites.Where(s => s.name == $"Cloth{clothIdx}_Right").FirstOrDefault();

        var pantIdx = Random.Range(1, 4);
        newHunter.AvatarCustomize.LeftPantSprite = _pantSprites.Where(s => s.name == $"Pant{pantIdx}_Left").FirstOrDefault();
        newHunter.AvatarCustomize.RightPantSprite = _pantSprites.Where(s => s.name == $"Pant{pantIdx}_Right").FirstOrDefault();

        _hunters.Add(newHunter);
        _onHuntersChanged.Invoke();

        GameManager.Instance.GetSystem<LoggerSystem>().LogInfo($"<b>{newHunter.DisplayName}</b> 이(가) 마을에 이주했습니다.");
    }

    public void RemoveHunter(Hunter hunter)
    {
        _hunters.Remove(hunter);
        _hunterNamesLeft.Add(hunter.DisplayName);
        _onHuntersChanged.Invoke();
        Destroy(hunter.gameObject);
    }

    public JToken Serialize()
    {
        var token = new JArray();
        foreach (var hunter in _hunters)
        {
            Vector2Int cellPos = GameManager.Instance.GetSystem<ConstructionGridmap>().WorldToCell(hunter.transform.position);

            var obj = new JObject
            {
                ["name"] = hunter.DisplayName,
                ["hp"] = hunter.DefaultHp,
                ["damage"] = hunter.DefaultDamage,
                ["posX"] = cellPos.x,
                ["posY"] = cellPos.y,
                ["hair"] = hunter.AvatarCustomize.HairSprite.name,
                ["cloth"] = hunter.AvatarCustomize.BodyClothSprite.name,
                ["leftSleeve"] = hunter.AvatarCustomize.LeftSleeveSprite.name,
                ["rightSleeve"] = hunter.AvatarCustomize.RightSleeveSprite.name,
                ["leftPant"] = hunter.AvatarCustomize.LeftPantSprite.name,
                ["rightPant"] = hunter.AvatarCustomize.RightPantSprite.name,
                ["hairColor"] = '#' + ColorUtility.ToHtmlStringRGB(hunter.AvatarCustomize.HairColor),
            };
            token.Add(obj);
        }
        return token;
    }

    public void Deserialize(JToken token)
    {
        foreach (var hunter in _hunters)
        {
            Destroy(hunter.gameObject);
        }
        _hunters.Clear();
        _hunterNamesLeft.Clear();
        _hunterNamesLeft.AddRange(_hunterNames);

        foreach (var obj in token)
        {
            var newHunter = Instantiate(_hunterPrefab, transform);

            var cellPos = new Vector2Int(obj["posX"].Value<int>(), obj["posY"].Value<int>());
            var worldPos = GameManager.Instance.GetSystem<ConstructionGridmap>().CellToWorld(cellPos);

            newHunter.DisplayName = obj["name"].Value<string>();
            newHunter.DefaultHp = obj["hp"].Value<float>();
            newHunter.DefaultDamage = obj["damage"].Value<float>();
            newHunter.transform.position = worldPos;
            newHunter.AvatarCustomize.BodyClothSprite = _clothSprites.Where(s => s.name == obj["cloth"].Value<string>()).FirstOrDefault();
            newHunter.AvatarCustomize.LeftSleeveSprite = _clothSprites.Where(s => s.name == obj["leftSleeve"].Value<string>()).FirstOrDefault();
            newHunter.AvatarCustomize.RightSleeveSprite = _clothSprites.Where(s => s.name == obj["rightSleeve"].Value<string>()).FirstOrDefault();
            newHunter.AvatarCustomize.LeftPantSprite = _pantSprites.Where(s => s.name == obj["leftPant"].Value<string>()).FirstOrDefault();
            newHunter.AvatarCustomize.RightPantSprite = _pantSprites.Where(s => s.name == obj["rightPant"].Value<string>()).FirstOrDefault();
            newHunter.AvatarCustomize.HairSprite = _hairSprites.Where(s => s.name == obj["hair"].Value<string>()).FirstOrDefault();
            newHunter.AvatarCustomize.HairColor = ColorUtility.TryParseHtmlString(obj["hairColor"].Value<string>(), out var color) ? color : Color.white;

            _hunters.Add(newHunter);

            _onHuntersChanged.Invoke();
        }
    }
}
