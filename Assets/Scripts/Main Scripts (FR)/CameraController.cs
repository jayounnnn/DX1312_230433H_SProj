namespace jayounnnn_HeroBrew
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices.WindowsRuntime;
    using UnityEditor.PackageManager;
    using UnityEngine;
    using UnityEngine.Windows;

    public class CameraController : MonoBehaviour
    {
        private static CameraController _instance = null; public static CameraController instance { get { return _instance; } }

        [Header("Camera Settings")]
        [SerializeField] private Camera _camera = null;
        [SerializeField] private float _moveSpeed = 50;
        [SerializeField] private float _zoomSpeed = 5.0f;
        [SerializeField] private float _moveSmooth = 5;
        [SerializeField] private float _zoomSmooth = 5;

        private InputActions _inputActions = null;

        private bool _zooming = false;
        private bool _moving = false;
        private Vector3 _center = Vector3.zero;
        private float _left = 10;
        private float _right = 10;
        private float _up = 10;
        private float _down = 10;
        private float _angle = 45;
        private float _zoom = 5;
        private float _zoomMin = 1;
        private float _zoomMax = 10;
        private Vector2 _zoomPositionOnScreen = Vector2.zero;
        private Vector3 _zoomPositionInWorld = Vector3.zero;
        private float _zoomBaseValue = 0;
        private float _zoomBaseDistance = 0;


        private Transform _root = null;
        private Transform _pivot = null;
        private Transform _target = null;

        private bool _building = false; public bool isPlacingBuilding {  get { return _building; } set { _building = value; } }
        private Vector3 _buildBasePosition = Vector3.zero;
        private bool _movingBuilding = false;

        private void Awake()
        {
            _instance = this;
            _inputActions = new InputActions();
            _root = new GameObject("CameraHelper").transform;
            _pivot = new GameObject("CameraPivot").transform;
            _target = new GameObject("CameraTarget").transform;
            _camera.orthographic = true;
            _camera.nearClipPlane = 0;
        }

        private void Start()
        {
            Initialize(Vector3.zero, 40, 40, 40, 40, 45, 10, 3, 20);
        }

        public void Initialize(Vector3 center, float left, float right, float up, float down, float angle, float zoom, float zoomMin, float zoomMax)
        {
            _center = center;
            _left = left;
            _right = right;
            _up = up;
            _down = down;
            _angle = angle;
            _zoom = zoom;
            _zoomMin = zoomMin;
            _zoomMax = zoomMax;

            _camera.orthographicSize = _zoom;

            _zooming = false;
            _moving = false;
            _pivot.SetParent(_root);
            _target.SetParent(_pivot);

            _root.position = _center;
            _root.localEulerAngles = Vector3.zero;

            _pivot.localPosition = Vector3.zero;
            _pivot.localEulerAngles = new Vector3(_angle, 0, 0);

            _target.localPosition = new Vector3(0, 0, -100);
            _target.localEulerAngles = Vector3.zero;
        }

        private void OnEnable()
        {
            _inputActions.Enable();
            _inputActions.Main.Move.started += _ => MoveStarted();
            _inputActions.Main.Move.canceled += _ => MoveCanceled();
            _inputActions.Main.TouchZoom.started += _ => ZoomStarted();
            _inputActions.Main.TouchZoom.canceled += _ => ZoomCanceled();
            //_inputActions.Main.PointerClick.performed += _ => ScreenClicked();
        }

        private void OnDisable()
        {
            _inputActions.Main.Move.started -= _ => MoveStarted();
            _inputActions.Main.Move.canceled -= _ => MoveCanceled();
            _inputActions.Main.TouchZoom.started -= _ => ZoomStarted();
            _inputActions.Main.TouchZoom.canceled -= _ => ZoomCanceled();
            //_inputActions.Main.PointerClick.performed -= _ => ScreenClicked();
            _inputActions.Disable();
        }

        private void MoveStarted()
        {
            if (UI_Main.instance.isActive)
            {
                if (_building)
                {
                    _buildBasePosition = CameraScreenPositionToPlanePosition(_inputActions.Main.PointerPosition.ReadValue<Vector2>());
                    if (UI_Main.instance._grid.IsWorldPositionIsOnPlane(_buildBasePosition, Building.instance.currentX, Building.instance.currentY, Building.instance.rows, Building.instance.columns))
                    {
                        Building.instance.StartMovingOnGrid();
                        _movingBuilding = true;
                    }
                }
                }
                if (_movingBuilding == false) {
                {
                    _moving = true;
                }
            }
        }

        private void MoveCanceled()
        {
            _moving = false;
            _movingBuilding = false;
        }
        private void ZoomStarted()
        {
            if (UI_Main.instance.isActive)
            {
                Vector2 touch0 = _inputActions.Main.TouchPosition0.ReadValue<Vector2>();
                Vector2 touch1 = _inputActions.Main.TouchPosition1.ReadValue<Vector2>();
                _zoomPositionOnScreen = Vector2.Lerp(touch0, touch1, 0.5f);
                _zoomPositionInWorld = CameraScreenPositionToPlanePosition(_zoomPositionOnScreen);
                _zoomBaseValue = _zoom;

                touch0.x /= Screen.width;
                touch1.x /= Screen.width;
                touch0.y /= Screen.height;
                touch1.y /= Screen.height;

                _zoomBaseDistance = Vector2.Distance(touch0, touch1);
                _zooming = true;
            }
        }

        private void ZoomCanceled()
        {
            _zooming = false;
        }


        private void Update()
        {
            if (_moving)
            {
                Vector2 move =_inputActions.Main.MoveDelta.ReadValue<Vector2>();

                if (UnityEngine.Input.touchSupported == false)
                {
                    float mouseScroll = _inputActions.Main.MouseScroll.ReadValue<float>();
                    if (mouseScroll > 0)
                    {
                        _zoom -= 3.0f * Time.deltaTime;
                    }
                    else if (mouseScroll < 0)
                    {
                        _zoom += 3.0f * Time.deltaTime;
                    }
                }

                if (_zooming)
                {
                    Vector2 touch0 = _inputActions.Main.TouchPosition0.ReadValue<Vector2>();
                    Vector2 touch1 = _inputActions.Main.TouchPosition1.ReadValue<Vector2>();

                    touch0.x /= Screen.width;
                    touch1.x /= Screen.width;
                    touch0.y /= Screen.height;
                    touch1.y /= Screen.height;

                    float currentDistance = Vector2.Distance(touch0, touch1);
                    float deltaDistance = currentDistance - _zoomBaseDistance;
                    _zoom = _zoomBaseValue - (deltaDistance * _zoomSpeed);

                    Vector3 zoomCenter = CameraScreenPositionToWorldPosition(_zoomPositionOnScreen);
                    _root.position = (_zoomPositionInWorld - zoomCenter);
                }
                else if (move != Vector2.zero)
                {
                    move.x /= Screen.width;
                    move.y /= Screen.height;
                    _root.position -= _root.right.normalized * move.x * _moveSpeed;
                    _root.position -= _root.forward.normalized * move.y * _moveSpeed;
                }

                AdjustBounds();

                if (_camera.orthographicSize != _zoom)
                {
                    _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _zoom, _zoomSmooth * Time.deltaTime);
                }
                if (_camera.transform.position != _target.position)
                {
                    _camera.transform.position = Vector3.Lerp(_camera.transform.position, _target.position, _moveSmooth * Time.deltaTime);
                }
                if (_camera.transform.rotation != _target.rotation)
                {
                    _camera.transform.rotation = _target.rotation;
                }
            }

            if (_building && _movingBuilding)
            {
                Vector3 pos = CameraScreenPositionToPlanePosition(_inputActions.Main.PointerPosition.ReadValue<Vector2>());
                Building.instance.UpdateGridPosition(_buildBasePosition, pos);
            }
        }

        private void AdjustBounds()
        {
            if (_zoom < _zoomMin)
            {
                _zoom = _zoomMin;
            }
            if (_zoom > _zoomMax)
            {
                _zoom = _zoomMax;
            }

            float height = PlaneOrtographicSize();
            float width = height * _camera.aspect;

            if (height > (_up + _down) / 2.0f)
            {
                float n = (_up + _down) / 2.0f;
                _zoom = n  * Mathf.Sin(_angle * Mathf.Deg2Rad);
            }
            if (width > (_right + _left) / 2.0f)
            {
                float n = (_right + _left) / 2.0f;
                _zoom = n * Mathf.Sin(_angle * Mathf.Deg2Rad) / _camera.aspect;

            }

            height = PlaneOrtographicSize();
            width = height * _camera.aspect;

            Vector3 tr = _root.position + _root.right.normalized * width + _root.forward.normalized * height;
            Vector3 tl = _root.position - _root.right.normalized * width + _root.forward.normalized * height;
            Vector3 dr = _root.position + _root.right.normalized * width - _root.forward.normalized * height;
            Vector3 dl = _root.position - _root.right.normalized * width - _root.forward.normalized * height;

            if (tr.x > _center.x + _right)
            {
                _root.position += Vector3.left * Mathf.Abs(tr.x - (_center.x + _right));
            }
            if (tl.x < _center.x - _left)
            {
                _root.position += Vector3.right * Mathf.Abs((_center.x - _left) - tl.x);
            }
            if (tr.z > _center.z + _up)
            {
                _root.position += Vector3.back * Mathf.Abs(tr.z - (_center.z + _up));
            }
            if (dl.z < _center.z - _down)
            {
                _root.position += Vector3.forward * Mathf.Abs((_center.z - _down) - dl.z);
            }
        }
        private Vector3 CameraScreenPositionToWorldPosition(Vector2 position)
        {
            float h = _camera.orthographicSize * 2f;
            float w = _camera.aspect * h;
            Vector3 ancher = _camera.transform.position - (_camera.transform.right.normalized * w / 2f) - (_camera.transform.up.normalized * h / 2f);
            return ancher + (_camera.transform.right.normalized * position.x / Screen.width * w) + (_camera.transform.up.normalized * position.y / Screen.height * h);
        }

        private float PlaneOrtographicSize()
        {
            float h = _zoom * 2f;
            return h / Mathf.Sin(_angle * Mathf.Deg2Rad) / 2f;
        }

        public Vector3 CameraScreenPositionToPlanePosition(Vector2 position)
        {
            Vector3 point = CameraScreenPositionToWorldPosition(position);
            float h = point.y - _root.position.y;
            float x = h / Mathf.Sin(_angle * Mathf.Deg2Rad);
            return point + _camera.transform.forward.normalized * x;
        }
    }
}