using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIBuildToggle : MonoBehaviour
{
    [AssetSelector]
    [SerializeField]
    private Construction _constructionPrefab;

    private Toggle _toggle;


    public Construction ConstructionPrefab => _constructionPrefab;
    public UnityEvent<bool> OnValueChanged => _toggle.onValueChanged;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
    }

    public void SetOn()
    {
        _toggle.SetIsOnWithoutNotify(true);
    }
}
