using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIBuildToggle : MonoBehaviour
{
    [AssetSelector]
    [SerializeField]
    private Construction _constructionPrefab;

    [SerializeField]
    private int _populationCondition;

    private Toggle _toggle;


    public Construction ConstructionPrefab => _constructionPrefab;
    public int PopulationCondition => _populationCondition;
    public UnityEvent<bool> OnValueChanged => _toggle.onValueChanged;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
    }

    private void Update()
    {
        var player = GameManager.Instance.GetSystem<Player>();
        _toggle.interactable = player.Population >= _populationCondition;
    }

    public void SetOn()
    {
        _toggle.SetIsOnWithoutNotify(true);
    }
}
