using UnityEngine;

public class EmployDirector : MonoBehaviour
{
    [SerializeField] private EmployHunter[] _employHunter = new EmployHunter[4];
    [SerializeField] private string[] _nameTemplates;

    public EmployHunter[] EmployHunter => _employHunter;

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            SetRandomEmployHunter(i);
        }
    }

    public void SetRandomEmployHunter(int index)
    {
        if (index < 0 || index >= 4)
            Debug.LogError("Invalid index");

        var day = GameManager.Instance.GetSystem<TimeSystem>().Day.Total;
        var stat = 13 + (day * day / 1500);
        var hp = Random.Range(0, stat + 1 - 6) + 3;
        var damage = stat - hp;

        _employHunter[index].Name = _nameTemplates[Random.Range(0, _nameTemplates.Length)];
        _employHunter[index].HP = hp;
        _employHunter[index].Damage = damage;

        var database = GameManager.Instance.GetSystem<CustomizeDatabase>();
        _employHunter[index].AvatarCustomize.Hair = database.Hairs[Random.Range(0, database.Hairs.Length)];
        _employHunter[index].AvatarCustomize.HairColor = Random.ColorHSV(0, 1, 0.4f, 0.6f, 0.5f, 1f);
        _employHunter[index].AvatarCustomize.TopCloth = database.TopCloths[Random.Range(0, database.TopCloths.Length)];
        _employHunter[index].AvatarCustomize.BottomCloth = database.BottomCloths[Random.Range(0, database.BottomCloths.Length)];
    }
}
