using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildButton : MonoBehaviour
{
    [AssetSelector]
    [SerializeField]
    private Construction _constructionPrefab;

    private Button _button;
    private Outline _outline;

    public Button.ButtonClickedEvent OnClick => _button.onClick;
    public Outline Outline => _outline;

    public Construction ConstructionPrefab => _constructionPrefab;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _outline = GetComponent<Outline>();
    }
}
