using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UIConstructionBuildPanel : MonoBehaviour
{
    [SerializeField] private Button _destructionButton;
    [SerializeField] private UIFade _residencePanel;
    [SerializeField] private UIFade _parkPanel;
    [SerializeField] private UIFade _roadPanel;
    [SerializeField] private Button _residenceButton;
    [SerializeField] private Button _parkButton;
    [SerializeField] private Button _roadButton;
    [SerializeField] private UIFade _informationPanel;
    [SerializeField] private Text _informationTitle;
    [SerializeField] private Text _informationDescription;
    [SerializeField] private UIBuildButton _selectButton;
    [SerializeField] private UIBuildButton _desctructButton;

    private ConstructionBuilder _constructionBuilder;
    private UIBuildButton[] _buildButtons;

    private void Awake()
    {
        _constructionBuilder = FindObjectOfType<ConstructionBuilder>();
        _buildButtons = GetComponentsInChildren<UIBuildButton>();
    }


    private void Start()
    {
        _residenceButton.onClick.AddListener(() =>
        {
            if (_residencePanel.IsFadedIn)
            {
                _residencePanel.FadeOut();
            }
            else
            {
                _residencePanel.FadeIn();
                _parkPanel.FadeOut();
                _roadPanel.FadeOut();
            }
        });

        _parkButton.onClick.AddListener(() =>
        {
            if (_parkPanel.IsFadedIn)
            {
                _parkPanel.FadeOut();
            }
            else
            {
                _residencePanel.FadeOut();
                _parkPanel.FadeIn();
                _roadPanel.FadeOut();
            }
        });

        _roadButton.onClick.AddListener(() =>
        {
            if (_roadPanel.IsFadedIn)
            {
                _roadPanel.FadeOut();
            }
            else
            {
                _residencePanel.FadeOut();
                _parkPanel.FadeOut();
                _roadPanel.FadeIn();
            }
        });

        foreach (var button in _buildButtons)
        {
            button.OnClick.AddListener(() =>
            {
                if (button == _selectButton)
                {
                    _constructionBuilder.BulidMode = ConstructionBuilder.BuildMode.Select;
                    _residencePanel.FadeOut();
                    _parkPanel.FadeOut();
                    _roadPanel.FadeOut();

                }
                else if (button == _desctructButton)
                {
                    _constructionBuilder.BulidMode = ConstructionBuilder.BuildMode.Destruct;
                    _residencePanel.FadeOut();
                    _parkPanel.FadeOut();
                    _roadPanel.FadeOut();
                }
                else
                {
                    _constructionBuilder.SelectedConstructionPrefab = button.ConstructionPrefab;
                    _constructionBuilder.BulidMode = ConstructionBuilder.BuildMode.Construct;
                }

                foreach (var btn in _buildButtons)
                {
                    btn.Outline.enabled = false;
                }
                button.Outline.enabled = true;
            });
        }
    }

    private void Update()
    {
        var hoveredButton = UIUtil.GetUIObjectTypeOverPointer<UIBuildButton>();
        if (hoveredButton)
        {
            if (hoveredButton == _selectButton)
            {
                _informationTitle.text = "선택 모드";
                _informationDescription.text = "건물을 선택할수 있습니다";
                _informationPanel.FadeIn();
                return;
            }
            else if (hoveredButton == _desctructButton)
            {
                _informationTitle.text = "파괴 모드";
                _informationDescription.text = "건물을 파괴할수 있습니다";
                _informationPanel.FadeIn();
                return;
            }
            else if (hoveredButton.ConstructionPrefab)
            {
                var construction = hoveredButton.ConstructionPrefab;

                _informationTitle.text = $"[{construction.DisplayName}] - {construction.Cost}G";
                _informationDescription.text = construction.Description;
                _informationPanel.FadeIn();
            }
        }
        else
        {
            _informationTitle.text = string.Empty;
            _informationDescription.text = string.Empty;
            _informationPanel.FadeOut();
        }
    }
}
