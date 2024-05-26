using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class ConstructionDatabase : MonoBehaviour
{
    [AssetSelector][SerializeField] private Construction[] _constructionPrefabs;

    public Construction GetConstructionPrefab(string id)
    {
        return _constructionPrefabs.Where(c => c.ID == id).FirstOrDefault();
    }

    public Construction[] ConstructionPrefabs => _constructionPrefabs;
}
