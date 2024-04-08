using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _zoomSmooth;

    private Camera _camera;
    private float _zoomCurr;
    private float _zoomTarget;
    private Vector2 _dragOrigin;
    private bool _isUIClicked;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        _zoomTarget = _zoomCurr = _camera.orthographicSize;
    }

    private void Update()
    {
        var scroll = Input.mouseScrollDelta.y;

        _zoomTarget += scroll * Time.deltaTime * -_zoomSpeed;
        _zoomTarget = Mathf.Clamp(_zoomTarget, 1, 5);
        _zoomCurr = Mathf.Lerp(_zoomCurr, _zoomTarget, Time.deltaTime * _zoomSmooth);

        _camera.orthographicSize = _zoomCurr;

        if (Input.GetMouseButtonDown(0))
        {
            if (UIManager.IsUIObjectOverPointer())
            {
                _isUIClicked = true;
            }
            else
            {
                _dragOrigin = _camera.ScreenToWorldPoint(Input.mousePosition);

            }
        }

        if (Input.GetMouseButton(0) && !_isUIClicked)
        {
            var difference = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            var position = (Vector3)_dragOrigin - difference;
            position.z = -10;
            transform.position = position;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isUIClicked = false;
        }
    }
}
