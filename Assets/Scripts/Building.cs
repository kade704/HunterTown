using UnityEngine;

public class Building : MonoBehaviour
{
    private Construction _construction;

    public Construction Construction => _construction;

    private void Awake()
    {
        _construction = GetComponent<Construction>();
    }
}
