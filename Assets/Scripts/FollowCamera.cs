using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Camera _camera;
    private Transform _target;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetEnabled(false);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void LateUpdate()
    {
        if (_target)
        {
            transform.position = new Vector3(_target.position.x, _target.position.y, transform.position.z);
        }
    }

    public void SetEnabled(bool enabled)
    {
        _camera.enabled = enabled;
    }
}
