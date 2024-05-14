using Sirenix.OdinInspector;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float _zoomSpeed;

    [SerializeField]
    private float _zoomSmooth;

    [MinMaxSlider(0, 10, true)]
    [SerializeField]
    private Vector2 _zoomLimits;

    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private float _moveSmooth;


    private Camera _camera;
    private float _zoomCurr;
    private float _zoomTarget;
    private Vector2 _velocity;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        _zoomTarget = _zoomCurr = _camera.orthographicSize;
    }

    private void LateUpdate()
    {
        var scroll = Input.mouseScrollDelta.y;
        if (scroll != 0)
        {
            _zoomTarget += scroll * Time.deltaTime * -_zoomSpeed;
            _zoomTarget = Mathf.Clamp(_zoomTarget, _zoomLimits.x, _zoomLimits.y);
        }
        _zoomCurr = Mathf.Lerp(_zoomCurr, _zoomTarget, Time.deltaTime * _zoomSmooth);

        _camera.orthographicSize = _zoomCurr;

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        var velocity = new Vector3(horizontal, vertical, 0) * _moveSpeed * Time.deltaTime;
        _velocity = Vector2.Lerp(_velocity, velocity, Time.deltaTime * _moveSmooth);
        transform.position += (Vector3)_velocity;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector3(0, 0, -10);
        }
    }

    public void MovePosition(Vector2 position)
    {
        transform.position = new Vector3(position.x, position.y, -10);
    }
}
