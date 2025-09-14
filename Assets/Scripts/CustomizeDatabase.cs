using UnityEngine;

[System.Serializable]
public struct BaseBody
{
    public string id;
    public Sprite head;
    public Sprite body;
    public Sprite armR;
    public Sprite armL;
    public Sprite legR;
    public Sprite legL;
}

[System.Serializable]
public struct TopCloth
{
    public string id;
    public Sprite body;
    public Sprite armR;
    public Sprite armL;
}

[System.Serializable]
public struct Armor
{
    public string id;
    public Sprite body;
    public Sprite armR;
    public Sprite armL;
}

[System.Serializable]
public struct BottomCloth
{
    public string id;
    public Sprite legR;
    public Sprite legL;
}

[System.Serializable]
public struct Hair
{
    public string id;
    public Sprite hair;
}

[System.Serializable]
public struct Helmet
{
    public string id;
    public Sprite hair;
}

[System.Serializable]
public struct Weapon
{
    public string id;
    public Sprite weapon;
}

[System.Serializable]
public struct Eye
{
    public string id;
    public Sprite eye;
}

public class CustomizeDatabase : MonoBehaviour
{
    [SerializeField] private BaseBody[] _baseBody;
    [SerializeField] private TopCloth[] _topCloths;
    [SerializeField] private BottomCloth[] _bottomCloths;
    [SerializeField] private Armor[] _armors;
    [SerializeField] private Helmet[] _helmets;
    [SerializeField] private Hair[] _hairs;
    [SerializeField] private Weapon[] _weapons;
    [SerializeField] private Eye[] _eyes;

    public BaseBody[] BaseBodies => _baseBody;
    public TopCloth[] TopCloths => _topCloths;
    public BottomCloth[] BottomCloths => _bottomCloths;
    public Hair[] Hairs => _hairs;
    public Armor[] Armors => _armors;
    public Helmet[] Helmets => _helmets;
    public Weapon[] Weapons => _weapons;
    public Eye[] Eyes => _eyes;
}
