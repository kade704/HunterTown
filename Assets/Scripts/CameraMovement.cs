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
    private float _targetZoom;
    private Vector2 _targetPosition;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        _targetPosition = transform.position;
        _targetZoom = _camera.orthographicSize;

        var options = GameManager.Instance.GetSystem<Options>();
        _zoomSpeed = options.CameraZoomSpeed;
        _moveSpeed = options.CameraMoveSpeed;
    }

    private void LateUpdate()
    {
        var scroll = Input.mouseScrollDelta.y;
        if (scroll != 0)
        {
            _targetZoom += scroll * Time.deltaTime * -_zoomSpeed;
            _targetZoom = Mathf.Clamp(_targetZoom, _zoomLimits.x, _zoomLimits.y);
        }
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetZoom, Time.deltaTime * _zoomSmooth);

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        var velocity = _moveSpeed * Time.deltaTime * new Vector2(horizontal, vertical);

        _targetPosition += velocity;

        transform.position = Vector2.Lerp(transform.position, _targetPosition, Time.deltaTime * _moveSmooth);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    public void MovePosition(Vector2 position)
    {
        _targetPosition = position;
    }
}
