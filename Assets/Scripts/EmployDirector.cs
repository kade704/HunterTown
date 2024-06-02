using UnityEngine;

public class EmployDirector : MonoBehaviour
{
    [SerializeField] private string[] _nameTemplates;
    [SerializeField] private string[] _descriptionTemplates;


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

        var educations = FindObjectsOfType<Education>();
        var statScale = 0f;
        foreach (var education in educations)
        {
            statScale += education.IncleaseEmployStat;
        }

        var day = GameManager.Instance.GetSystem<TimeSystem>().Day.Total;
        var totalStat = 13 + (day * day / 1500);
        totalStat = (int)(totalStat * (1 + statScale));
        totalStat = (int)(totalStat * Random.Range(0.9f, 1.1f));

        var hp = Random.Range(0, totalStat + 1 - 6) + 3;
        var damage = totalStat - hp;

        _employHunters[index].Name = _nameTemplates[Random.Range(0, _nameTemplates.Length)];
        _employHunters[index].Description = _descriptionTemplates[Random.Range(0, _descriptionTemplates.Length)];
        _employHunters[index].HP = hp;
        _employHunters[index].Damage = damage;
        _employHunters[index].RandomCustomize();
    }
}
