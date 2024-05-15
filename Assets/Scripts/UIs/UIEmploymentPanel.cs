using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIEmploymentPanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _closeButton;
    [SerializeField] private UIEmploymentSlot[] _employmentSlots;

    private void Start()
    {
        var interactableSelector = FindObjectOfType<InteractableSelector>();
        interactableSelector.OnInteractableInteracted.AddListener((interactable, interaction) =>
        {
            if (interaction.ID == "#employment")
            {
                _panel.SetActive(true);
                Initialize();
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
    }

    private void Initialize()
    {
        var employment = GameManager.Instance.GetSystem<Employment>();
        for (int i = 0; i < 4; i++)
        {
            var index = i;
            _employmentSlots[i].EmployHunter = employment.EmployHunter[i];
            _employmentSlots[i].EmployButton.onClick.AddListener(() =>
            {
                StartCoroutine(EmployRoutine(index));
            });
        }
    }

    private IEnumerator EmployRoutine(int index)
    {
        var employment = GameManager.Instance.GetSystem<Employment>();

        var hunterSpawner = GameManager.Instance.GetSystem<HunterSpawner>();
        var newHunter = hunterSpawner.SpawnHunter();
        newHunter.DisplayName = employment.EmployHunter[index].Name;
        newHunter.DefaultHp = employment.EmployHunter[index].HP;
        newHunter.DefaultDamage = employment.EmployHunter[index].Damage;
        newHunter.AvatarCustomize.CopyAvatar(employment.EmployHunter[index].AvatarCustomize);

        _employmentSlots[index].EmployButton.interactable = false;
        yield return _employmentSlots[index].EmployHunter.ExitRoutine();

        employment.SetRandomEmployHunter(index);
        _employmentSlots[index].EmployHunter = employment.EmployHunter[index];

        yield return _employmentSlots[index].EmployHunter.EnterRoutine();
        _employmentSlots[index].EmployButton.interactable = true;
    }
}
