using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UIConstructionBuildPanel : MonoBehaviour
{
    [SerializeField] private Button _destructionButton;
    [SerializeField] private CanvasGroup _residencePanel;
    [SerializeField] private CanvasGroup _parkPanel;
    [SerializeField] private CanvasGroup _roadPanel;
    [SerializeField] private Button _residenceButton;
    [SerializeField] private Button _parkButton;
    [SerializeField] private Button _roadButton;
    [SerializeField] private GameObject _informationPanel;
    [SerializeField] private Text _informationTitle;
    [SerializeField] private Text _informationDescription;
    [SerializeField] private UIBuildToggle _selectButton;
    [SerializeField] private UIBuildToggle _desctructButton;
    private UIBuildToggle[] _buildToggles;
    private ToggleGroup _toggleGroup;

    private ConstructionBuilder _constructionBuilder;

    private void Awake()
    {
        _toggleGroup = GetComponent<ToggleGroup>();
        _constructionBuilder = FindObjectOfType<ConstructionBuilder>();
        _buildToggles = GetComponentsInChildren<UIBuildToggle>();
    }


    private void Start()
    {
        _residenceButton.onClick.AddListener(() =>
        {
            if (UIUtil.IsCanvasGroupVisible(_residencePanel))
            {
                UIUtil.HideCanvasGroup(_residencePanel);
            }
            else
            {
                UIUtil.HideCanvasGroup(_parkPanel);
                UIUtil.HideCanvasGroup(_roadPanel);
                UIUtil.ShowCanvasGroup(_residencePanel);
            }
        });

        _parkButton.onClick.AddListener(() =>
        {
            if (UIUtil.IsCanvasGroupVisible(_parkPanel))
            {
                UIUtil.HideCanvasGroup(_parkPanel);
            }
            else
            {
                UIUtil.HideCanvasGroup(_residencePanel);
                UIUtil.HideCanvasGroup(_roadPanel);
                UIUtil.ShowCanvasGroup(_parkPanel);
            }
        });

        _roadButton.onClick.AddListener(() =>
        {
            if (UIUtil.IsCanvasGroupVisible(_roadPanel))
            {
                UIUtil.HideCanvasGroup(_roadPanel);
            }
            else
            {
                UIUtil.HideCanvasGroup(_residencePanel);
                UIUtil.HideCanvasGroup(_parkPanel);
                UIUtil.ShowCanvasGroup(_roadPanel);
            }
        });


        foreach (var toggle in _buildToggles)
        {
            toggle.OnValueChanged.AddListener((value) =>
            {
                if (value == false) return;

                if (toggle == _selectButton)
                {
                    _constructionBuilder.BulidMode = ConstructionBuilder.BuildMode.Select;

                    UIUtil.HideCanvasGroup(_residencePanel);
                    UIUtil.HideCanvasGroup(_parkPanel);
                    UIUtil.HideCanvasGroup(_roadPanel);

                }
                else if (toggle == _desctructButton)
                {
                    _constructionBuilder.BulidMode = ConstructionBuilder.BuildMode.Destruct;

                    UIUtil.HideCanvasGroup(_residencePanel);
                    UIUtil.HideCanvasGroup(_parkPanel);
                    UIUtil.HideCanvasGroup(_roadPanel);
                }
                else
                {
                    _constructionBuilder.SelectedConstructionPrefab = toggle.ConstructionPrefab;
                    _constructionBuilder.BulidMode = ConstructionBuilder.BuildMode.Construct;
                }
            });
        }

        var constructionBuilder = GameManager.Instance.GetSystem<ConstructionBuilder>();
        constructionBuilder.OnBuildModeChanged.AddListener((mode) =>
        {
            if (mode == ConstructionBuilder.BuildMode.Select)
            {
                _selectButton.SetOn();

                UIUtil.HideCanvasGroup(_residencePanel);
                UIUtil.HideCanvasGroup(_parkPanel);
                UIUtil.HideCanvasGroup(_roadPanel);
            }
            else if (mode == ConstructionBuilder.BuildMode.Destruct)
            {
                _desctructButton.SetOn();

                UIUtil.HideCanvasGroup(_residencePanel);
                UIUtil.HideCanvasGroup(_parkPanel);
                UIUtil.HideCanvasGroup(_roadPanel);
            }
        });
    }

    private void Update()
    {
        var hoveredButton = UIUtil.GetUIObjectTypeOverPointer<UIBuildToggle>();
        if (hoveredButton)
        {
            if (hoveredButton == _selectButton)
            {
                _informationTitle.text = "선택 모드";
                _informationDescription.text = "건물을 선택할수 있습니다";
                _informationPanel.SetActive(true);
                return;
            }
            else if (hoveredButton == _desctructButton)
            {
                _informationTitle.text = "파괴 모드";
                _informationDescription.text = "건물을 파괴할수 있습니다";
                _informationPanel.SetActive(true);
                return;
            }
            else if (hoveredButton.ConstructionPrefab)
            {
                var construction = hoveredButton.ConstructionPrefab;

                _informationTitle.text = $"[{construction.DisplayName}] - {construction.Cost}G";
                _informationDescription.text = construction.Description;
                _informationPanel.SetActive(true);
            }
        }
        else
        {
            _informationTitle.text = string.Empty;
            _informationDescription.text = string.Empty;
            _informationPanel.SetActive(false);
        }
    }
}
