using UnityEngine;

public class NatureGenerator : MonoBehaviour
{
    private ConstructionGridmap _constructionGridMap;

    private void Awake()
    {
        _constructionGridMap = FindObjectOfType<ConstructionGridmap>();
    }

    public void SpawnRandomNatures(int amount)
    {
        var naturePrefabs = Resources.LoadAll<Nature>("Constructions");

        var count = 0;
        while (count++ < amount)
        {
            var idx = Random.Range(0, naturePrefabs.Length);
            var cellPos = new Vector2Int(Random.Range(-10, 10), Random.Range(-10, 10));

            if (_constructionGridMap.GetConstructionAt(cellPos) != null)
                continue;

            var naturePrefab = naturePrefabs[idx];
            _constructionGridMap.BuildConstruction(naturePrefab.GetComponent<Construction>(), cellPos);
        }
    }

}
