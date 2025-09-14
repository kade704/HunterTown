using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIEmployPanel : MonoBehaviour
{
    private CanvasGroup _panel;
    private Button _closeButton;
    private Button _refreshButton;
    private Text _remainText;
    private Image _transitionImage;
    private UIEmploySlot[] _employmentSlots;

    private void Awake()
    {
        _panel = GetComponent<CanvasGroup>();
        _closeButton = transform.Find("CloseButton").GetComponent<Button>();
        _refreshButton = transform.Find("RefreshButton").GetComponent<Button>();
        _remainText = transform.Find("RemainText").GetComponent<Text>();
        _transitionImage = transform.Find("Director/TransitionImage").GetComponent<Image>();
        _employmentSlots = GetComponentsInChildren<UIEmploySlot>();
    }

    private void Start()
    {
        var interactableSelector = FindObjectOfType<InteractableSelector>();
        interactableSelector.OnInteractableInteracted.AddListener((interactable, interaction) =>
        {
            if (interaction.ID == "#employment")
            {
                UIUtil.ShowCanvasGroup(_panel);
                Initialize(interactable.GetComponent<Company>());
            }
            else
            {
                UIUtil.HideCanvasGroup(_panel);
            }
        });

        _closeButton.onClick.AddListener(() =>
        {
            UIUtil.HideCanvasGroup(_panel);
        });

        _refreshButton.onClick.AddListener(() =>
        {
            StartCoroutine(RefreshRoutine());
        });
    }

    private void Initialize(Company company)
    {
        _remainText.text = $"고용 가능 인원: {company.RemainEmployeeCount}";
        var employment = GameManager.Instance.GetSystem<EmployDirector>();
        for (int i = 0; i < 4; i++)
        {
            var index = i;
            _employmentSlots[index].EmployButton.onClick.RemoveAllListeners();
            _employmentSlots[index].EmployButton.onClick.AddListener(() =>
            {
                StartCoroutine(EmployRoutine(company, index));
            });
            _employmentSlots[index].EmployHunter = employment.EmployHunters[i];
        }
    }

    private IEnumerator RefreshRoutine()
    {
        _refreshButton.interactable = false;
        GameManager.Instance.GetSystem<MoneySystem>().Money -= 500;

        yield return HideTransitionRoutine();

        var employDirector = GameManager.Instance.GetSystem<EmployDirector>();
        for (int i = 0; i < 4; i++)
        {
            employDirector.SetRandomEmployHunter(i);
            _employmentSlots[i].EmployHunter = employDirector.EmployHunters[i];
        }

        yield return ShowTransitionRoutine();

        _refreshButton.interactable = true;
    }

    private IEnumerator HideTransitionRoutine()
    {
        _transitionImage.fillOrigin = 0;
        _transitionImage.fillAmount = 0;
        while (_transitionImage.fillAmount < 1)
        {
            _transitionImage.fillAmount += Time.deltaTime * 2;
            yield return null;
        }
    }

    private IEnumerator ShowTransitionRoutine()
    {
        _transitionImage.fillOrigin = 1;
        _transitionImage.fillAmount = 1;
        while (_transitionImage.fillAmount > 0)
        {
            _transitionImage.fillAmount -= Time.deltaTime * 2;
            yield return null;
        }
    }

    private IEnumerator EmployRoutine(Company company, int index)
    {
        if (company.RemainEmployeeCount <= 0) yield break;
        company.RemainEmployeeCount -= 1;
        _remainText.text = $"고용 가능 인원: {company.RemainEmployeeCount}";

        GameManager.Instance.GetSystem<MoneySystem>().Money -= 100;

        var employDirector = GameManager.Instance.GetSystem<EmployDirector>();

        var hunterSpawner = GameManager.Instance.GetSystem<HunterSpawner>();
        var newHunter = hunterSpawner.SpawnHunter(company.Construction.CellPos - Vector2Int.one);
        newHunter.Interactable.DisplayName = employDirector.EmployHunters[index].Name;
        newHunter.Interactable.Description = employDirector.EmployHunters[index].Description;
        newHunter.DefaultHp = employDirector.EmployHunters[index].HP;
        newHunter.DefaultDamage = employDirector.EmployHunters[index].Damage;
        newHunter.AvatarCustomize.CopyAvatar(employDirector.EmployHunters[index].AvatarCustomize);

        GameManager.Instance.GetSystem<NotificationSystem>().NotifyInfo("새로운 헌터가 고용되었습니다.");

        _refreshButton.interactable = false;
        _employmentSlots[index].EmployButton.interactable = false;

        yield return _employmentSlots[index].EmployHunter.ExitRoutine();

        employDirector.SetRandomEmployHunter(index);

        yield return _employmentSlots[index].EmployHunter.EnterRoutine();
        _employmentSlots[index].EmployHunter = employDirector.EmployHunters[index];
        _employmentSlots[index].EmployButton.interactable = true;
        _refreshButton.interactable = true;
    }
}
