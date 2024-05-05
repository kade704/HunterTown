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
        if (scroll != 0)
        {
            _zoomTarget += scroll * Time.deltaTime * -_zoomSpeed;
            _zoomTarget = Mathf.Clamp(_zoomTarget, _zoomLimits.x, _zoomLimits.y);

            float remap(float val, float in1, float in2, float out1, float out2)
            {
                return out1 + (val - in1) * (out2 - out1) / (in2 - in1);
            }

            GameManager.Instance.GetSystem<AudioController>().AmbienceVolumeScale = remap(_zoomCurr, _zoomLimits.x, _zoomLimits.y, 1, 0);
            GameManager.Instance.GetSystem<AudioController>().MusicVolume = remap(_zoomCurr, _zoomLimits.x, _zoomLimits.y, 0.5f, 1);
        }
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
