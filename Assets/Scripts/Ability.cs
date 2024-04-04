using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
public class Ability : ScriptableObject
{
    private string _id;
    private Sprite _icon;
    private string _displayName;
    private string _description;

    public string ID => _id;
    public Sprite Icon => _icon;
    public string DisplayName => _displayName;
    public string Description => _description;
}
