using UnityEngine;

public class EmployDirector : MonoBehaviour
{
    [SerializeField] private string[] _nameTemplates;

    private EmployHunter[] _employHunters;

    public EmployHunter[] EmployHunters => _employHunters;

    private void Awake()
    {
        _employHunters = GetComponentsInChildren<EmployHunter>();
    }

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
        var totalStat = 13 + (day * day / 1500);
        totalStat = (int)(totalStat * Random.Range(0.9f, 1.1f));

        var hp = Random.Range(0, totalStat + 1 - 6) + 3;
        var damage = totalStat - hp;

        _employHunters[index].Name = _nameTemplates[Random.Range(0, _nameTemplates.Length)];
        _employHunters[index].HP = hp;
        _employHunters[index].Damage = damage;
        _employHunters[index].RandomCustomize();
    }
}
