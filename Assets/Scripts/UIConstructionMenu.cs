using UnityEngine;
using UnityEngine.UI;

public class UIConstructionMenu : UIPanel
{
    private Button _menuOpenButton;

    protected override void Awake()
    {
        base.Awake();

        _menuOpenButton = transform.Find("../MenuOpenButton").GetComponent<Button>();
        _menuOpenButton.onClick.AddListener(() =>
        {
            ShowPanel();
            _menuOpenButton.gameObject.SetActive(false);
        });

        var menuCloseButton = transform.Find("CloseButton").GetComponent<Button>();
        menuCloseButton.onClick.AddListener(() =>
        {
            HidePanel();
            _menuOpenButton.gameObject.SetActive(true);
        });

        var constructionBuilder = FindObjectOfType<ConstructionBuilder>();

        constructionBuilder.OnExitBuildMode.AddListener(() =>
        {
            HidePanel();
            _menuOpenButton.gameObject.SetActive(true);
        });

        var group = transform.Find("Group");
        var constructionSlotRef = transform.Find("ConstructionSlotRef").GetComponent<UIConstructionSlot>();

        var constructions = Resources.LoadAll<Construction>("Constructions");

        foreach (var construction in constructions)
        {
            if (!construction.Buildable) continue;

            var constuctionSlot = Instantiate(constructionSlotRef, group);
            constuctionSlot.Construction = construction;
            constuctionSlot.OnClick.AddListener(() =>
            {
                HidePanel();
                constructionBuilder.ConstructionToBuild = construction;
            });
        }
    }
}
