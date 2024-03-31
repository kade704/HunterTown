using System;
using UnityEngine;

[Serializable]
public struct ConstructionInteraction
{
    [SerializeField] private string _id;
    [SerializeField] private string _displayName;
    [SerializeField] private Sprite _icon;

    public string ID => _id;
    public string DisplayName => _displayName;
    public Sprite Icon => _icon;
}
