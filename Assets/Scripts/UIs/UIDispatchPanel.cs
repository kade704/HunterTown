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
    [SerializeField] private Button _resultCloseButton;

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
        yield return GameManager.Instance.GetSystem<DispatchDirector>().BattleRoutine();
        _resultPanel.SetActive(true);
    }

    private void Initialize(Portal portal)
    {
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

        var battle = GameManager.Instance.GetSystem<DispatchDirector>();

        var hunters = portal.Construction.VisitedHunters;
        for (int i = 0; i < 4; i++)
        {
            if (i < hunters.Length)
            {
                _dispatchSlots[i].SetHunter(hunters[i], portal);
                battle.SetHunter(i, hunters[i].GetComponent<Hunter>());
            }
            else
            {
                _dispatchSlots[i].SetHunter(null, portal);
                battle.SetHunter(i, null);
            }
        }

        _dispatchButton.interactable = hunters.Count() > 0;
    }
}
