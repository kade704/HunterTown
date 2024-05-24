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

    private Portal _targetPortal;
    private DispatchDirector _dispatchDirector;
    private Coroutine _dispatchRoutine;

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
        _abilitySlots = GetComponentsInChildren<UIAbilitySlot>();
        _transitionImage = transform.Find("Director/TransitionImage").GetComponent<Image>();
        _skipButton = transform.Find("Director/SkipButton").GetComponent<Button>();
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
            _dispatchDirector.Initialize();
            UIUtil.ShowCanvasGroup(_resultPanel);
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
        GameManager.Instance.GetSystem<LoggerSystem>().LogInfo($"파견이 시작되었습니다.");
        UIUtil.ShowCanvasGroup(_skipButton.GetComponent<CanvasGroup>());

        yield return _dispatchDirector.EnterPortal();

        yield return HideTransitionRoutine();

        _dispatchDirector.PrepareBattle();

        yield return ShowTransitionRoutine();

        yield return _dispatchDirector.BattleRoutine();

        _resultPanel.GetComponent<UIDispatchResultPanel>().Initialize();
        UIUtil.ShowCanvasGroup(_resultPanel);

        _dispatchDirector.FinishBattle(_targetPortal);
        _dispatchDirector.Initialize();
        UIUtil.HideCanvasGroup(_skipButton.GetComponent<CanvasGroup>());
    }

    private void Initialize(Portal portal)
    {
        _targetPortal = portal;

        _powerText.text = "능력치: " + (portal.PowerVisibility ? portal.Power.ToString("F1") : "???");
        _dangerText.text = "위험도: " + (portal.DangerVisibility ? portal.Danger.ToString("F1") : "???");
        _difficultyText.text = "복잡도: " + (portal.DifficultyVisibility ? portal.Difficulty.ToString("F1") : "???");
        _rankText.text = portal.Rank.ToString();

        for (int i = 0; i < 3; i++)
        {
            var abilitySlot = _abilitySlots[i];

            abilitySlot.Ability = portal.Abilities[i];
            abilitySlot.Hidden = !portal.AbilityVisibilities[i];
        }

        var hunters = portal.Construction.VisitedHunters;
        for (int i = 0; i < 4; i++)
        {
            if (i < hunters.Length)
            {
                _dispatchSlots[i].Hunter = hunters[i];
                if (portal.DangerVisibility)
                {
                    var deathProbabilityPercent = (int)(portal.CalcHunterDeathProbability(hunters[i]) * 100);
                    _dispatchSlots[i].DeathProbability = $"사망 확률: {deathProbabilityPercent}%";
                }
                else
                {
                    _dispatchSlots[i].DeathProbability = $"사망 확률: ?%";
                }
                _dispatchDirector.SetHunter(i, hunters[i].GetComponent<Hunter>(), portal);
            }
            else
            {
                _dispatchSlots[i].Hunter = null;
                _dispatchSlots[i].DeathProbability = "";


                _dispatchDirector.SetHunter(i, null, portal);
            }
        }

        _dispatchButton.interactable = hunters.Count() > 0;
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
