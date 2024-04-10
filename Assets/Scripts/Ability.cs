using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
public class Ability : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _displayName;
    [SerializeField] private string _effect;
    [SerializeField] private string _description;
    [SerializeField] private bool _advantage;

    public string ID => _id;
    public Sprite Icon => _icon;
    public string DisplayName => _displayName;
    public string Effect => _effect;
    public string Description => _description;
    public bool Advantage => _advantage;
}
