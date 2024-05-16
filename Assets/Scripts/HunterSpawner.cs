using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;

public class HunterSpawner : MonoBehaviour, ISerializable, IDeserializable
{
    [SerializeField] private Hunter _hunterPrefab;

    private List<Hunter> _hunters = new();
    private UnityEvent _onHuntersChanged = new();

    public Hunter[] Hunters => _hunters.ToArray();
    public UnityEvent OnHuntersChanged => _onHuntersChanged;

    public Hunter SpawnHunter()
    {
        var newHunter = Instantiate(_hunterPrefab, transform);

        _hunters.Add(newHunter);
        _onHuntersChanged.Invoke();

        return newHunter;
    }

    public void RemoveHunter(Hunter hunter)
    {
        _hunters.Remove(hunter);
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
                ["hair"] = hunter.AvatarCustomize.Hair.id,
                ["top"] = hunter.AvatarCustomize.TopCloth.id,
                ["bottom"] = hunter.AvatarCustomize.BottomCloth.id,
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

        var database = GameManager.Instance.GetSystem<CustomizeDatabase>();

        foreach (var obj in token)
        {
            var newHunter = Instantiate(_hunterPrefab, transform);

            var cellPos = new Vector2Int(obj["posX"].Value<int>(), obj["posY"].Value<int>());
            var worldPos = GameManager.Instance.GetSystem<ConstructionGridmap>().CellToWorld(cellPos);

            newHunter.DisplayName = obj["name"].Value<string>();
            newHunter.DefaultHp = obj["hp"].Value<float>();
            newHunter.DefaultDamage = obj["damage"].Value<float>();
            newHunter.transform.position = worldPos;
            newHunter.AvatarCustomize.Hair = database.Hairs.Where(h => h.id == obj["hair"].Value<string>()).FirstOrDefault();
            newHunter.AvatarCustomize.TopCloth = database.TopCloths.Where(t => t.id == obj["top"].Value<string>()).FirstOrDefault();
            newHunter.AvatarCustomize.BottomCloth = database.BottomCloths.Where(b => b.id == obj["bottom"].Value<string>()).FirstOrDefault();
            newHunter.AvatarCustomize.HairColor = ColorUtility.TryParseHtmlString(obj["hairColor"].Value<string>(), out var color) ? color : Color.white;

            _hunters.Add(newHunter);

            _onHuntersChanged.Invoke();
        }
    }
}
