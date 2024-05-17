using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIEmployPanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Text _remainText;
    [SerializeField] private UIEmploySlot[] _employmentSlots;
    private Company _company;

    private void Start()
    {
        var interactableSelector = FindObjectOfType<InteractableSelector>();
        interactableSelector.OnInteractableInteracted.AddListener((interactable, interaction) =>
        {
            if (interaction.ID == "#employment")
            {
                _panel.SetActive(true);
                Initialize(interactable.GetComponent<Company>());
            }
            else
            {
                _panel.SetActive(false);
            }
        });

        _closeButton.onClick.AddListener(() =>
        {
            _panel.SetActive(false);
        });

        for (int i = 0; i < 4; i++)
        {
            var index = i;
            _employmentSlots[index].EmployButton.onClick.AddListener(() =>
            {
                StartCoroutine(EmployRoutine(index));
            });
        }
    }

    private void Initialize(Company company)
    {
        _company = company;
        _remainText.text = $"고용 가능 인원: {company.RemainEmployeeCount}";
        var employment = GameManager.Instance.GetSystem<EmployDirector>();
        for (int i = 0; i < 4; i++)
        {
            _employmentSlots[i].EmployHunter = employment.EmployHunter[i];
        }
    }

    private IEnumerator EmployRoutine(int index)
    {
        if (_company.RemainEmployeeCount <= 0) yield break;
        _company.RemainEmployeeCount -= 1;
        _remainText.text = $"고용 가능 인원: {_company.RemainEmployeeCount}";

        GameManager.Instance.GetSystem<Player>().Money -= 100;

        var employment = GameManager.Instance.GetSystem<EmployDirector>();

        var hunterSpawner = GameManager.Instance.GetSystem<HunterSpawner>();
        var newHunter = hunterSpawner.SpawnHunter();
        newHunter.DisplayName = employment.EmployHunter[index].Name;
        newHunter.DefaultHp = employment.EmployHunter[index].HP;
        newHunter.DefaultDamage = employment.EmployHunter[index].Damage;
        newHunter.AvatarCustomize.CopyAvatar(employment.EmployHunter[index].AvatarCustomize);

        _employmentSlots[index].Hide();
        yield return _employmentSlots[index].EmployHunter.ExitRoutine();

        employment.SetRandomEmployHunter(index);


        _employmentSlots[index].EmployHunter = employment.EmployHunter[index];

        yield return _employmentSlots[index].EmployHunter.EnterRoutine();
        _employmentSlots[index].Show();
    }
}
