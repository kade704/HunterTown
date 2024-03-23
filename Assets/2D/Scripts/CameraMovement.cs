using UnityEngine;

public class CameraMovement : MonoBehaviour {
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _zoomSmooth;
    [SerializeField] private float _dragSpeed;

    private Camera _camera;
    private float _zoomCurr;
    private float _zoomTarget;
    private Vector2 _dragOrigin;

    private void Awake() {
        _camera = GetComponent<Camera>();
    }

    private void Start() {
        _zoomTarget = _zoomCurr = _camera.orthographicSize;
    }

    private void Update() {
        var scroll = Input.mouseScrollDelta.y;

        _zoomTarget += scroll * Time.deltaTime * -_zoomSpeed;
        _zoomTarget = Mathf.Clamp(_zoomTarget, 1, 5);
        _zoomCurr = Mathf.Lerp(_zoomCurr, _zoomTarget, Time.deltaTime * _zoomSmooth);

        _camera.orthographicSize = _zoomCurr;

        //Not Working
        //if (Input.GetMouseButtonDown(0)) {
        //    _dragOrigin = _camera.ScreenToViewportPoint(Input.mousePosition);
        //}

        //if (Input.GetMouseButton(0)) {
        //    Vector2 diff = transform.position - _camera.ScreenToViewportPoint(Input.mousePosition);
        //    Vector3 pos = _dragOrigin - diff;
        //    pos.z = -10;
        //    transform.position = pos;
        //}
    }
}
