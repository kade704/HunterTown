using UnityEngine;

[System.Serializable]
public struct TopCloth
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

public class CustomizeDatabase : MonoBehaviour
{
    [SerializeField] private TopCloth[] _topCloths;
    [SerializeField] private BottomCloth[] _bottomCloths;

    [SerializeField] private Hair[] _hairs;

    public TopCloth[] TopCloths => _topCloths;
    public BottomCloth[] BottomCloths => _bottomCloths;
    public Hair[] Hairs => _hairs;
}
