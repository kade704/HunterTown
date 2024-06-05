using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIExaminePanel : MonoBehaviour
{
    private CanvasGroup _panel;
    private Button _closeButton;
    private Text _powerText;
    private Text _dangerText;
    private Text _difficultyText;
    private Text _rankText;
    private Button _examineButton;
    private UIAbilitySlot[] _abilitySlots;
    private Transform _searchIconParent;
    private Image _searchIcon;
    private Button _skipButton;

    private Portal _targetPortal;
    private Coroutine _examineRoutine;

    private void Awake()
    {
        _panel = GetComponent<CanvasGroup>();
        _closeButton = transform.Find("CloseButton").GetComponent<Button>();
        _powerText = transform.Find("StatusTexts/PowerText").GetComponent<Text>();
        _dangerText = transform.Find("StatusTexts/DangerText").GetComponent<Text>();
        _difficultyText = transform.Find("StatusTexts/DifficultyText").GetComponent<Text>();
        _rankText = transform.Find("RankText").GetComponent<Text>();
        _examineButton = transform.Find("ExamineButton").GetComponent<Button>();
        _abilitySlots = transform.Find("AbilitySlots").GetComponentsInChildren<UIAbilitySlot>();
        _searchIconParent = transform.Find("SearchIcon");
        _searchIcon = transform.Find("SearchIcon/Image").GetComponent<Image>();
        _skipButton = transform.Find("SkipButton").GetComponent<Button>();
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(() =>
        {
            UIUtil.HideCanvasGroup(_panel);
        });

        _examineButton.onClick.AddListener(() =>
        {
            _examineRoutine = StartCoroutine(ExamineRoutine());
        });

        _skipButton.onClick.AddListener(() =>
        {
            if (_examineRoutine != null)
            {
                StopCoroutine(_examineRoutine);
                _examineRoutine = null;
            }

            FastExamine();
            _searchIcon.enabled = false;
            _closeButton.interactable = true;
            _examineButton.interactable = true;
            UIUtil.HideCanvasGroup(_skipButton.GetComponent<CanvasGroup>());

            GameManager.Instance.GetSystem<NotificationSystem>().NotifyInfo("탐색이 완료되었습니다.");
            GameManager.Instance.GetSystem<TimeSystem>().Resume();
        });

        var interactableSelector = FindObjectOfType<InteractableSelector>();
        interactableSelector.OnInteractableInteracted.AddListener((interactable, interaction) =>
        {
            if (interaction.ID == "#examine")
            {
                Initialize(interactable.GetComponent<Portal>());
                UIUtil.ShowCanvasGroup(_panel);
            }
            else
            {
                UIUtil.HideCanvasGroup(_panel);
            }
        });

        StartCoroutine(SearchAnimateRoutine());
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

        var hunters = portal.GetComponent<Visitable>().VisitedHunters;
        _examineButton.interactable = hunters.Count() > 0;
    }

    private IEnumerator ExamineRoutine()
    {
        _closeButton.interactable = false;
        _examineButton.interactable = false;

        GameManager.Instance.GetSystem<TimeSystem>().Pause();
        GameManager.Instance.GetSystem<MoneySystem>().Money -= 100;
        GameManager.Instance.GetSystem<NotificationSystem>().NotifyInfo("탐색이 시작되었습니다.");

        UIUtil.ShowCanvasGroup(_skipButton.GetComponent<CanvasGroup>());

        if (_targetPortal.ContainAbility("understood_world"))
        {
            _targetPortal.PowerVisibility = true;
            _targetPortal.DangerVisibility = true;
            _targetPortal.DifficultyVisibility = true;
            for (int i = 0; i < 3; i++)
            {
                _targetPortal.AbilityVisibilities[i] = true;
            }
        }
        else
        {
            if (!_targetPortal.PowerVisibility)
            {
                _searchIcon.enabled = true;
                _searchIconParent.transform.position = _powerText.transform.position;
                yield return new WaitForSeconds(Random.Range(2f, 5f));
                _targetPortal.PowerVisibility = Random.value < 0.5f;
                _powerText.text = "능력치: " + (_targetPortal.PowerVisibility ? _targetPortal.Power.ToString("F1") : "???");
            }
            if (!_targetPortal.DangerVisibility)
            {
                _searchIcon.enabled = true;
                _searchIconParent.transform.position = _dangerText.transform.position;
                yield return new WaitForSeconds(Random.Range(2f, 5f));
                _targetPortal.DangerVisibility = Random.value < 0.5f;
                _dangerText.text = "위험도: " + (_targetPortal.DangerVisibility ? _targetPortal.Danger.ToString("F1") : "???");
            }
            if (!_targetPortal.DifficultyVisibility)
            {
                _searchIcon.enabled = true;
                _searchIconParent.transform.position = _difficultyText.transform.position;
                yield return new WaitForSeconds(Random.Range(2f, 5f));
                _targetPortal.DifficultyVisibility = Random.value < 0.5f;
                _difficultyText.text = "복잡도: " + (_targetPortal.DifficultyVisibility ? _targetPortal.Difficulty.ToString("F1") : "???");
            }
            for (int i = 0; i < 3; i++)
            {
                if (!_targetPortal.AbilityVisibilities[i])
                {
                    _searchIcon.enabled = true;
                    _searchIconParent.transform.position = _abilitySlots[i].transform.position;
                    yield return new WaitForSeconds(Random.Range(2f, 5f));
                    _targetPortal.AbilityVisibilities[i] = Random.value < 0.5f;
                    _abilitySlots[i].Hidden = !_targetPortal.AbilityVisibilities[i];
                }
            }
            _searchIcon.enabled = false;
        }

        _closeButton.interactable = true;
        _examineButton.interactable = true;

        GameManager.Instance.GetSystem<TimeSystem>().Resume();
        GameManager.Instance.GetSystem<NotificationSystem>().NotifyInfo("탐색이 완료되었습니다.");

        UIUtil.HideCanvasGroup(_skipButton.GetComponent<CanvasGroup>());
    }

    private void FastExamine()
    {
        if (_targetPortal.ContainAbility("understood_world"))
        {
            _targetPortal.PowerVisibility = true;
            _targetPortal.DangerVisibility = true;
            _targetPortal.DifficultyVisibility = true;
            for (int i = 0; i < 3; i++)
            {
                _targetPortal.AbilityVisibilities[i] = true;
            }
        }
        else
        {
            if (!_targetPortal.PowerVisibility)
            {
                _targetPortal.PowerVisibility = Random.value < 0.5f;
                _powerText.text = "능력치: " + (_targetPortal.PowerVisibility ? _targetPortal.Power.ToString("F1") : "???");
            }
            if (!_targetPortal.DangerVisibility)
            {
                _targetPortal.DangerVisibility = Random.value < 0.5f;
                _dangerText.text = "위험도: " + (_targetPortal.DangerVisibility ? _targetPortal.Danger.ToString("F1") : "???");
            }
            if (!_targetPortal.DifficultyVisibility)
            {
                _targetPortal.DifficultyVisibility = Random.value < 0.5f;
                _difficultyText.text = "복잡도: " + (_targetPortal.DifficultyVisibility ? _targetPortal.Difficulty.ToString("F1") : "???");
            }
            for (int i = 0; i < 3; i++)
            {
                if (!_targetPortal.AbilityVisibilities[i])
                {
                    _targetPortal.AbilityVisibilities[i] = Random.value < 0.5f;
                    _abilitySlots[i].Hidden = !_targetPortal.AbilityVisibilities[i];
                }
            }
        }
    }

    private IEnumerator SearchAnimateRoutine()
    {
        int[] x = new int[] { 1, 1, -1, -1 };
        int[] y = new int[] { 1, -1, -1, 1 };

        int i = 0;
        while (true)
        {
            _searchIcon.transform.localPosition = new Vector3(x[i], y[i], 0) * 5;
            yield return new WaitForSeconds(0.5f);
            i = (i + 1) % 4;
        }
    }
}
