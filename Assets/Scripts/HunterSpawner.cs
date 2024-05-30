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

    public Hunter SpawnHunter(Vector2Int cellPos)
    {
        var worldPos = GameManager.Instance.GetSystem<ConstructionGridmap>().CellToWorld(cellPos);
        var newHunter = Instantiate(_hunterPrefab, worldPos, Quaternion.identity, transform);

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
                ["name"] = hunter.Interactable.DisplayName,
                ["decription"] = hunter.Interactable.Description,
                ["hp"] = hunter.DefaultHp,
                ["damage"] = hunter.DefaultDamage,
                ["posX"] = cellPos.x,
                ["posY"] = cellPos.y,

                ["baseBody"] = hunter.AvatarCustomize.BaseBody.id,
                ["top"] = hunter.AvatarCustomize.TopCloth.id,
                ["bottom"] = hunter.AvatarCustomize.BottomCloth.id,
                ["armor"] = hunter.AvatarCustomize.Armor.id,
                ["helmet"] = hunter.AvatarCustomize.Helmet.id,
                ["weapon"] = hunter.AvatarCustomize.Weapon.id,
                ["eye"] = hunter.AvatarCustomize.Eye.id,
                ["eyeColor"] = '#' + ColorUtility.ToHtmlStringRGB(hunter.AvatarCustomize.EyeColor),
                ["hair"] = hunter.AvatarCustomize.Hair.id,
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

            newHunter.Interactable.DisplayName = obj["name"].Value<string>();
            newHunter.Interactable.Description = obj["decription"].Value<string>();
            newHunter.DefaultHp = obj["hp"].Value<float>();
            newHunter.DefaultDamage = obj["damage"].Value<float>();
            newHunter.transform.position = worldPos;

            newHunter.AvatarCustomize.BaseBody = database.BaseBodies.Where(b => b.id == obj["baseBody"].Value<string>()).FirstOrDefault();
            newHunter.AvatarCustomize.TopCloth = database.TopCloths.Where(t => t.id == obj["top"].Value<string>()).FirstOrDefault();
            newHunter.AvatarCustomize.BottomCloth = database.BottomCloths.Where(b => b.id == obj["bottom"].Value<string>()).FirstOrDefault();
            newHunter.AvatarCustomize.Armor = database.Armors.Where(a => a.id == obj["armor"].Value<string>()).FirstOrDefault();
            newHunter.AvatarCustomize.Helmet = database.Helmets.Where(h => h.id == obj["helmet"].Value<string>()).FirstOrDefault();
            newHunter.AvatarCustomize.Weapon = database.Weapons.Where(w => w.id == obj["weapon"].Value<string>()).FirstOrDefault();
            newHunter.AvatarCustomize.Eye = database.Eyes.Where(e => e.id == obj["eye"].Value<string>()).FirstOrDefault();
            newHunter.AvatarCustomize.EyeColor = ColorUtility.TryParseHtmlString(obj["eyeColor"].Value<string>(), out Color color) ? color : Color.white;
            newHunter.AvatarCustomize.Hair = database.Hairs.Where(h => h.id == obj["hair"].Value<string>()).FirstOrDefault();
            newHunter.AvatarCustomize.HairColor = ColorUtility.TryParseHtmlString(obj["hairColor"].Value<string>(), out color) ? color : Color.white;

            _hunters.Add(newHunter);

            _onHuntersChanged.Invoke();
        }
    }
}
