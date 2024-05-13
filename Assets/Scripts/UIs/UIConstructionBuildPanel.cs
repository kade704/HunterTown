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
    [SerializeField] private UIBuildButton _selectButton;
    [SerializeField] private UIBuildButton _desctructButton;
    private UIBuildButton[] _buildButtons;

    private ConstructionBuilder _constructionBuilder;

    private void Awake()
    {
        _constructionBuilder = FindObjectOfType<ConstructionBuilder>();
        _buildButtons = GetComponentsInChildren<UIBuildButton>();
    }


    private void Start()
    {
        _residenceButton.onClick.AddListener(() =>
        {
            if (_residencePanel.alpha == 1)
            {
                _residencePanel.alpha = 0;
                _residencePanel.blocksRaycasts = false;
            }
            else
            {
                _residencePanel.alpha = 1;
                _residencePanel.blocksRaycasts = true;

                _parkPanel.alpha = 0;
                _parkPanel.blocksRaycasts = false;

                _roadPanel.alpha = 0;
                _roadPanel.blocksRaycasts = false;
            }
        });

        _parkButton.onClick.AddListener(() =>
        {
            if (_parkPanel.alpha == 1)
            {
                _parkPanel.alpha = 0;
                _parkPanel.blocksRaycasts = false;
            }
            else
            {
                _residencePanel.alpha = 0;
                _residencePanel.blocksRaycasts = false;

                _parkPanel.alpha = 1;
                _parkPanel.blocksRaycasts = true;

                _roadPanel.alpha = 0;
                _roadPanel.blocksRaycasts = false;
            }
        });

        _roadButton.onClick.AddListener(() =>
        {
            if (_roadPanel.alpha == 1)
            {
                _roadPanel.alpha = 0;
                _roadPanel.blocksRaycasts = false;
            }
            else
            {
                _residencePanel.alpha = 0;
                _residencePanel.blocksRaycasts = false;

                _parkPanel.alpha = 0;
                _parkPanel.blocksRaycasts = false;

                _roadPanel.alpha = 1;
                _roadPanel.blocksRaycasts = true;
            }
        });

        foreach (var button in _buildButtons)
        {
            button.OnClick.AddListener(() =>
            {
                if (button == _selectButton)
                {
                    _constructionBuilder.BulidMode = ConstructionBuilder.BuildMode.Select;

                    _residencePanel.alpha = 0;
                    _residencePanel.blocksRaycasts = false;

                    _parkPanel.alpha = 0;
                    _parkPanel.blocksRaycasts = false;

                    _roadPanel.alpha = 0;
                    _roadPanel.blocksRaycasts = false;

                }
                else if (button == _desctructButton)
                {
                    _constructionBuilder.BulidMode = ConstructionBuilder.BuildMode.Destruct;

                    _residencePanel.alpha = 0;
                    _residencePanel.blocksRaycasts = false;

                    _parkPanel.alpha = 0;
                    _parkPanel.blocksRaycasts = false;

                    _roadPanel.alpha = 0;
                    _roadPanel.blocksRaycasts = false;
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
