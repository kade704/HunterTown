using UnityEngine;

public class RoadMap : ConstructionMap
{
    public override Construction Set(Construction construction, Vector2Int cellPos)
    {
        var newRoad = base.Set(construction, cellPos) as Road;
        newRoad.UpdateSprite();
        UpdateNeighborRoadSprites(cellPos);
        return newRoad;
    }

    public override void Remove(Vector2Int cellPos)
    {
        base.Remove(cellPos);
        UpdateNeighborRoadSprites(cellPos);
    }

    private void UpdateNeighborRoadSprites(Vector2Int cellPos)
    {
        var xPos = new int[] { 1, -1, 0, 0 };
        var yPos = new int[] { 0, 0, 1, -1 };
        for (int i = 0; i < 4; i++)
        {
            var neighbor = Get(cellPos + new Vector2Int(xPos[i], yPos[i])) as Road;
            if (neighbor)
            {
                neighbor.UpdateSprite();
            }
        }
    }
}
