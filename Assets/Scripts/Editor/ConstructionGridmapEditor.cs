using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConstructionGridmap))]
public class ConstructionGridmapEditor : Editor
{
    private Construction _selectedConstruction;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var constructionGridmap = (ConstructionGridmap)target;

        if (!_selectedConstruction)
        {
            GUI.color = Color.green;
        }
        else
        {
            GUI.color = Color.white;
        }
        if (GUILayout.Button("선택 지우기"))
        {
            _selectedConstruction = null;
        }

        var constructionDatabase = FindObjectOfType<ConstructionDatabase>();
        if (constructionDatabase)
        {
            foreach (var construction in constructionDatabase.ConstructionPrefabs)
            {
                if (construction == _selectedConstruction)
                {
                    GUI.color = Color.green;
                }
                else
                {
                    GUI.color = Color.white;
                }

                if (GUILayout.Button($"{construction.DisplayName}"))
                {
                    _selectedConstruction = construction;
                }
            }
        }

        GUILayout.Space(10);

        var lastRect = GUILayoutUtility.GetLastRect();

        var mapTexture = GenerateMapTexture(constructionGridmap);
        EditorGUI.DrawPreviewTexture(new Rect(lastRect.x, lastRect.y + lastRect.height, 256, 256), mapTexture);
        GUILayout.Space(256);
    }

    private void OnSceneGUI()
    {
        var constructionGridmap = (ConstructionGridmap)target;

        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            var mousePosition = Event.current.mousePosition * EditorGUIUtility.pixelsPerPoint;
            mousePosition.y = Camera.current.pixelHeight - mousePosition.y;

            var cellPos = constructionGridmap.WorldToCell(Camera.current.ScreenToWorldPoint(mousePosition));
            if (_selectedConstruction)
            {
                constructionGridmap.BuildConstruction(_selectedConstruction, cellPos);
            }
            else
            {
                constructionGridmap.DestroyConstruction(cellPos);
            }
        }
    }

    private Texture GenerateMapTexture(ConstructionGridmap constructionGridmap)
    {
        var mapTexture = new Texture2D(ConstructionGridmap.GRID_SIZE, ConstructionGridmap.GRID_SIZE)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };
        for (var y = 0; y < ConstructionGridmap.GRID_SIZE; y++)
        {
            for (var x = 0; x < ConstructionGridmap.GRID_SIZE; x++)
            {
                var construction = constructionGridmap.GetConstructionAt(new Vector2Int(x, y));
                if (construction)
                {
                    mapTexture.SetPixel(x, y, Color.white);
                }
                else
                {
                    mapTexture.SetPixel(x, y, Color.black);
                }
            }
        }

        mapTexture.Apply();

        return mapTexture;
    }
}
