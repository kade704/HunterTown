using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIDispatchResultPanel : MonoBehaviour
{
    private CanvasGroup _panel;
    private CanvasGroup _mainPanel;
    private Text _titleText;
    private Text _subTitleText;
    private UIDispatchResultSlot[] _dispatchResultSlots;
    private Button _closeButton;

    private void Awake()
    {
        _panel = GetComponent<CanvasGroup>();
        _mainPanel = transform.Find("../MainPanel").GetComponent<CanvasGroup>();

        _titleText = transform.Find("TitleText").GetComponent<Text>();
        _subTitleText = transform.Find("SubTitleText").GetComponent<Text>();
        _closeButton = transform.Find("CloseButton").GetComponent<Button>();
        _dispatchResultSlots = GetComponentsInChildren<UIDispatchResultSlot>();
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(() =>
        {
            UIUtil.HideCanvasGroup(_panel);
            UIUtil.HideCanvasGroup(_mainPanel.GetComponent<CanvasGroup>());
        });
    }

    public void Initialize()
    {
        var dispatchHunters = GameManager.Instance.GetSystem<DispatchDirector>().DispatchHunters;
        var dispatchUI = GameManager.Instance.GetSystem<UIDispatchPanel>();

        var activeDispatchHunters = dispatchHunters.Where(hunter => hunter.Hunter != null).ToArray();

        _titleText.text = activeDispatchHunters.All(hunter => hunter.WillDeath) ? "파견 실패..." : "파견 성공!";

        var aliveCount = activeDispatchHunters.Count(hunter => !hunter.WillDeath);
        _subTitleText.text = aliveCount > 0 ? $"{dispatchUI.TargetPortal.Reward}원을 획득하였습니다." : "모든 헌터가 사망했습니다.";

        for (int i = 0; i < _dispatchResultSlots.Length; i++)
        {
            _dispatchResultSlots[i].DispatchHunter = dispatchHunters[i];
        }
    }
}
