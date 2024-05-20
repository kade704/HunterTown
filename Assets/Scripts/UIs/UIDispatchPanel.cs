using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIDispatchPanel : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Text _powerText;
    [SerializeField] private Text _dangerText;
    [SerializeField] private Text _difficultyText;
    [SerializeField] private Text _rankText;
    [SerializeField] private UIDispatchSlot[] _dispatchSlots;
    [SerializeField] private Button _dispatchButton;
    [SerializeField] private UIAbilitySlot[] _abilitySlots;
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private Text _resultTitle;
    [SerializeField] private Text _resultSubTitle;
    [SerializeField] private UIDispatchResultSlot[] _dispatchResultSlots;
    [SerializeField] private Button _resultCloseButton;

    private Portal _targetPortal;
    private bool[] _hunterAlives = new bool[4];

    private void Start()
    {
        _closeButton.onClick.AddListener(() =>
        {
            _mainPanel.SetActive(false);
        });

        _resultCloseButton.onClick.AddListener(() =>
        {
            _resultPanel.SetActive(false);
            _mainPanel.SetActive(false);
            _closeButton.interactable = true;
            _dispatchButton.interactable = true;
            GameManager.Instance.GetSystem<DispatchDirector>().Initialize();
        });

        _dispatchButton.onClick.AddListener(() =>
        {
            _closeButton.interactable = false;
            _dispatchButton.interactable = false;
            StartCoroutine(DispatchRoutine());
        });

        var interactableSelector = FindObjectOfType<InteractableSelector>();
        interactableSelector.OnInteractableInteracted.AddListener((interactable, interaction) =>
        {
            if (interaction.ID == "#dispatch")
            {
                Initialize(interactable.GetComponent<Portal>());
                _mainPanel.SetActive(true);
            }
            else
            {
                _mainPanel.SetActive(false);
            }
        });
    }

    private IEnumerator DispatchRoutine()
    {
        GameManager.Instance.GetSystem<LoggerSystem>().LogInfo($"파견이 시작되었습니다.");

        yield return GameManager.Instance.GetSystem<DispatchDirector>().BattleRoutine(_hunterAlives);

        _resultPanel.SetActive(true);

        if (_hunterAlives.Any())
        {
            GameManager.Instance.GetSystem<LoggerSystem>().LogInfo("파견에 성공했습니다. 포탈이 사라집니다.");
            GameManager.Instance.GetSystem<PortalGenerator>().RemovePortal(_targetPortal.GetComponent<Portal>());
        }
        else
        {
            GameManager.Instance.GetSystem<LoggerSystem>().LogError("파견에 실패했습니다.");
        }
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

        var director = GameManager.Instance.GetSystem<DispatchDirector>();

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
                    _hunterAlives[i] = Random.Range(0, 100) > deathProbabilityPercent;
                }
                else
                {
                    _dispatchSlots[i].DeathProbability = $"사망 확률: ?%";
                }

                _dispatchResultSlots[i].Hunter = hunters[i];
                _dispatchResultSlots[i].Result = _hunterAlives[i] ? " - 생존" : " - 사망";

                director.SetHunter(i, hunters[i].GetComponent<Hunter>());
            }
            else
            {
                _hunterAlives[i] = false;

                _dispatchSlots[i].Hunter = null;
                _dispatchSlots[i].DeathProbability = "";

                _dispatchResultSlots[i].Hunter = null;
                _dispatchResultSlots[i].Result = "";

                director.SetHunter(i, null);
            }
        }

        _dispatchButton.interactable = hunters.Count() > 0;

        var aliveCount = _hunterAlives.Count(alive => alive);
        if (aliveCount == 0)
        {
            _resultTitle.text = "파견 실패";
            _resultSubTitle.text = "모든 헌터가 사망했습니다.";
        }
        else
        {
            _resultTitle.text = "파견 성공";
            _resultSubTitle.text = $"{aliveCount}명의 헌터가 생존했습니다.";
        }
    }
}
