using UnityEngine;

public sealed class CameraMovement : MonoBehaviour
{
    private CameraInput _input;

    private Camera _cam;
    [SerializeField] private Vector3 _offset;
    private float _offsetSizeX;
    private float _offsetSizeY;

    private Vector3 _origin;
    private Vector3 _difference;
    private Vector3 _centerPosition;
    private bool _draging;

    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _zoomMinSize, _zoomMaxSize;
    [SerializeField] private float _zoomSmoothness = 0.01f;
    private float _currentZoom;

    [SerializeField] private MapDrawer _mapDrawer;
    private Vector2 _leftBottomCorner;
    private Vector2 _rightUpCorner;

    private void Awake() {
        _input = GetComponent<CameraInput>();
        _cam = GetComponent<Camera>();
        _currentZoom = _cam.orthographicSize;
    }

    private void Start() {
        Init();
    }

    private void Init() {
        _leftBottomCorner = _mapDrawer.MapLeftBottomCornerCoordinates();
        _rightUpCorner = _mapDrawer.MapRightUpCornerCoordinates();

        _centerPosition = Vector3.Lerp(_leftBottomCorner, _rightUpCorner, 0.5f) + _offset;

        gameObject.transform.position = _centerPosition;

        UpdateCameraOrthographicSizeOffset(_cam);
    }

    private void Update() {
        if (GameController.Instance.CurentGameStage() != GameController.GameStage.Play) {
            return;
        }

        Move();
        Zoom();
    }

    private void LateUpdate() {
        ClampCameraViewToMap();
    }

    private void Move() {
        if (_input.DragButtonPressed()) {
            _difference = (_input.PointerPosition()) - gameObject.transform.position;

            if (_draging == false) {
                _draging = true;
                _origin = _input.PointerPosition();
            }

        } else {
            _draging = false;
        }

        if (!_draging) {
            return;
        }
    }

    private void Zoom() {
        _currentZoom = Mathf.Clamp(_currentZoom, _zoomMinSize, _zoomMaxSize);
        _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, _currentZoom, _zoomSmoothness);
        UpdateCameraOrthographicSizeOffset(_cam);
        
        if (_input.ZoomDelta() == 0) {
            return;
        }

        if (_input.ZoomDelta() > 0) {
            _currentZoom -= _zoomSpeed * Time.deltaTime * Mathf.Abs(_input.ZoomDelta());
        }

        if (_input.ZoomDelta() < 0) {
            _currentZoom += _zoomSpeed * Time.deltaTime * Mathf.Abs(_input.ZoomDelta());
        }
    }

    private void ClampCameraViewToMap() {
        gameObject.transform.position = _origin - _difference;
        var clampedX = Mathf.Clamp(gameObject.transform.position.x, _leftBottomCorner.x + _offsetSizeX, _rightUpCorner.x - _offsetSizeX);
        var clampedY = Mathf.Clamp(gameObject.transform.position.y, _leftBottomCorner.y + _offsetSizeY, _rightUpCorner.y - _offsetSizeY);
        gameObject.transform.position = new Vector3(clampedX, clampedY, 0f) + _offset;
    }

    private void UpdateCameraOrthographicSizeOffset(Camera cam) {
        _offsetSizeY = (cam.orthographicSize * 2) / 2;
        _offsetSizeX = ((cam.orthographicSize * 2) * cam.aspect) / 2;
    }

}
