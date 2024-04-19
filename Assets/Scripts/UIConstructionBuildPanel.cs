using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIFade))]
public class UIConstructionBuildPanel : MonoBehaviour
{
    private UIFade _fade;
    private Button _buildMenuButton;

    private void Awake()
    {
        _fade = GetComponent<UIFade>();

        _buildMenuButton = transform.Find("../BuildOpenButton").GetComponent<Button>();
        _buildMenuButton.onClick.AddListener(() =>
        {
            _fade.FadeIn();
            _buildMenuButton.gameObject.SetActive(false);
        });

        var closeButton = transform.Find("CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(() =>
        {
            _fade.FadeOut();
            _buildMenuButton.gameObject.SetActive(true);
        });

        var destructionButton = transform.Find("DestructionButton").GetComponent<Button>();
        destructionButton.onClick.AddListener(() =>
        {
            var constructionBuilder = FindObjectOfType<ConstructionBuilder>();
            constructionBuilder.SetDestructionMode(true);
            _fade.FadeOut();
        });

        var constructionBuilder = FindObjectOfType<ConstructionBuilder>();

        constructionBuilder.OnExitBuildMode.AddListener(() =>
        {
            _fade.FadeOut();
            _buildMenuButton.gameObject.SetActive(true);
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
                _fade.FadeOut();
                constructionBuilder.ConstructionToBuild = construction;
            });
        }
    }
}
