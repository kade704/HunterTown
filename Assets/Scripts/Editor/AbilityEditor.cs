using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Ability))]
public class AbilityEditor : Editor
{
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        var ability = target as Ability;
        if (ability.Icon != null)
        {
            var texture = new Texture2D(width, height);
            EditorUtility.CopySerialized(ability.Icon.texture, texture);
            return texture;
        }
        return base.RenderStaticPreview(assetPath, subAssets, width, height);
    }
}
