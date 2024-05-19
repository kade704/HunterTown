using UnityEngine;
using UnityEngine.Rendering;

public class Building : MonoBehaviour
{
    private Construction _construction;
    private SortingGroup _sortingGroup;

    public Construction Construction => _construction;

    private void Awake()
    {
        _construction = GetComponent<Construction>();
        _sortingGroup = GetComponent<SortingGroup>();
    }

    private void Update()
    {
        _sortingGroup.sortingOrder = 300 - Mathf.FloorToInt(transform.position.y * 10);
    }
}
