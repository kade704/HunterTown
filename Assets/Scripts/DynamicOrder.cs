using UnityEngine;
using UnityEngine.Rendering;

public class DynamicOrder : MonoBehaviour
{
    [SerializeField] private int _defaultOrder;

    private SortingGroup _sortingGroup;

    private void Awake()
    {
        _sortingGroup = GetComponent<SortingGroup>();
    }

    void Update()
    {
        _sortingGroup.sortingOrder = 300 - Mathf.FloorToInt(transform.position.y * 10) + _defaultOrder;
    }
}
