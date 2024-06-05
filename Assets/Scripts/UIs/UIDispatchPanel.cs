using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIDispatchPanel : MonoBehaviour
{
    private CanvasGroup _panel;
    private CanvasGroup _resultPanel;
    private Button _closeButton;
    private Text _powerText;
    private Text _dangerText;
    private Text _difficultyText;
    private Text _rankText;
    private UIDispatchSlot[] _dispatchSlots;
    private Button _dispatchButton;
    private UIAbilitySlot[] _abilitySlots;
    private Image _transitionImage;
    private Button _skipButton;
    private Text _rewardText;
    private Text _totalDeathText;

    private Portal _targetPortal;
    private DispatchDirector _dispatchDirector;
    private Coroutine _dispatchRoutine;

    public Portal TargetPortal => _targetPortal;

    private void Awake()
    {
        _panel = GetComponent<CanvasGroup>();
        _resultPanel = transform.Find("../ResultPanel").GetComponent<CanvasGroup>();
        _closeButton = transform.Find("CloseButton").GetComponent<Button>();
        _powerText = transform.Find("StatusTexts/PowerText").GetComponent<Text>();
        _dangerText = transform.Find("StatusTexts/DangerText").GetComponent<Text>();
        _difficultyText = transform.Find("StatusTexts/DifficultyText").GetComponent<Text>();
        _rankText = transform.Find("RankText").GetComponent<Text>();
        _dispatchSlots = GetComponentsInChildren<UIDispatchSlot>();
        _dispatchButton = transform.Find("DispatchButton").GetComponent<Button>();
        _abilitySlots = transform.Find("AbilitySlots").GetComponentsInChildren<UIAbilitySlot>();
        _transitionImage = transform.Find("Director/TransitionImage").GetComponent<Image>();
        _skipButton = transform.Find("Director/SkipButton").GetComponent<Button>();
        _rewardText = transform.Find("RewardText").GetComponent<Text>();
        _totalDeathText = transform.Find("TotalDeathText").GetComponent<Text>();
        _dispatchDirector = GameManager.Instance.GetSystem<DispatchDirector>();
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(() =>
        {
            UIUtil.HideCanvasGroup(_panel);
        });

        _dispatchButton.onClick.AddListener(() =>
        {
            _closeButton.interactable = false;
            _dispatchButton.interactable = false;
            _dispatchRoutine = StartCoroutine(DispatchRoutine());
        });

        _skipButton.onClick.AddListener(() =>
        {
            if (_dispatchRoutine != null)
            {
                StopCoroutine(_dispatchRoutine);
                _dispatchRoutine = null;
            }

            _resultPanel.GetComponent<UIDispatchResultPanel>().Initialize();
            _dispatchDirector.FinishBattle(_targetPortal);
            UIUtil.ShowCanvasGroup(_resultPanel);
            UIUtil.HideCanvasGroup(_skipButton.GetComponent<CanvasGroup>());
            GameManager.Instance.GetSystem<TimeSystem>().Resume();
        });

        var interactableSelector = FindObjectOfType<InteractableSelector>();
        interactableSelector.OnInteractableInteracted.AddListener((interactable, interaction) =>
        {
            if (interaction.ID == "#dispatch")
            {
                Initialize(interactable.GetComponent<Portal>());
                UIUtil.ShowCanvasGroup(_panel);
            }
            else
            {
                UIUtil.HideCanvasGroup(_panel);
            }
        });
    }

    private IEnumerator DispatchRoutine()
    {
        GameManager.Instance.GetSystem<TimeSystem>().Pause();
        GameManager.Instance.GetSystem<NotificationSystem>().NotifyInfo($"파견이 시작되었습니다.");
        UIUtil.ShowCanvasGroup(_skipButton.GetComponent<CanvasGroup>());

        yield return _dispatchDirector.EnterPortal();

        yield return HideTransitionRoutine();

        _dispatchDirector.PrepareBattle();

        yield return ShowTransitionRoutine();

        yield return _dispatchDirector.BattleRoutine();

        _resultPanel.GetComponent<UIDispatchResultPanel>().Initialize();
        UIUtil.ShowCanvasGroup(_resultPanel);

        _dispatchDirector.FinishBattle(_targetPortal);
        UIUtil.HideCanvasGroup(_skipButton.GetComponent<CanvasGroup>());

        GameManager.Instance.GetSystem<TimeSystem>().Resume();
    }

    private void Initialize(Portal portal)
    {
        _targetPortal = portal;

        _dispatchDirector.Initialize(portal);

        _powerText.text = "능력치: " + (portal.PowerVisibility ? portal.Power.ToString("F1") : "???");
        _dangerText.text = "위험도: " + (portal.DangerVisibility ? portal.Danger.ToString("F1") : "???");
        _difficultyText.text = "복잡도: " + (portal.DifficultyVisibility ? portal.Difficulty.ToString("F1") : "???");
        _rankText.text = portal.Rank.ToString();
        _rewardText.text = $"성공시 보상: {(portal.PowerVisibility ? portal.Reward : "???")}원";

        var hunterCount = portal.Visitable.VisitedHunters.Count();
        _totalDeathText.text = $"{hunterCount}명 배치 <color=#00ff00>(사망확률: -{Mathf.Max(0, hunterCount - 1) * 10}%)</color>";

        for (int i = 0; i < 3; i++)
        {
            var abilitySlot = _abilitySlots[i];

            abilitySlot.Ability = portal.Abilities[i];
            abilitySlot.Hidden = !portal.AbilityVisibilities[i];
        }

        var hunters = portal.Visitable.VisitedHunters;

        for (int i = 0; i < 4; i++)
        {
            if (i < hunters.Length)
            {
                _dispatchSlots[i].Hunter = hunters[i];
                if (portal.DangerVisibility)
                {
                    var deathProbabilityPercent = (int)(portal.CalcHunterDeathProbability(hunters)[i] * 100);
                    _dispatchSlots[i].DeathProbability = $"사망 확률: {deathProbabilityPercent}%";
                }
                else
                {
                    _dispatchSlots[i].DeathProbability = $"사망 확률: ?%";
                }
            }
            else
            {
                _dispatchSlots[i].Hunter = null;
                _dispatchSlots[i].DeathProbability = "";
            }
        }

        _dispatchButton.interactable = hunters.Count() > 0;
        _closeButton.interactable = true;
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
}
