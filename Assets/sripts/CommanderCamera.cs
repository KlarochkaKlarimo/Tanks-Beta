using ChobiAssets.PTM;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(AudioListener))]

public class CommanderCamera : MonoBehaviour
{
    [SerializeField] private KeyCode _cameraKeyCode;
    [SerializeField] private KeyCode _cammanderTurretControl;
    [SerializeField] private Turret_Horizontal_CS _turretHorizontalCS;
    [SerializeField] private Cannon_Vertical_CS _cannonVerticalCS;
    [SerializeField] private bool _isCommanderControl;
    [SerializeField] private Camera _gunCamera;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera_Points_Manager_CS _cameraPointsManager;
    private Camera _commanderCamera;
    private AudioListener _audioListener;
    private bool _mode;

    private float _zoomInput;
    private float _targetFOV;
    private float _currentFOV;
    [Range(0f, 100f)]
    [SerializeField] private float _minimumFOV = 2.0f;
    [Range(0f, 100f)]
    [SerializeField] private float _maximumFOV = 10.0f;
    [SerializeField] private float _currentZoomVelocity;

    private void Awake()
    {
        _commanderCamera = GetComponent<Camera>();
        _audioListener = GetComponent<AudioListener>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_cameraKeyCode))
        {
            _mode = !_mode;
            _mainCamera.enabled = _mode == false;
            _mainCamera.GetComponent<AudioListener>().enabled = _mode == false;
            _commanderCamera.enabled = _mode;
            _audioListener.enabled = _mode;

            _commanderCamera.gameObject.tag = _mode ? "MainCamera" : "Untagged";

            if (_mode)
            {
                _cameraPointsManager.SendMessage("Disable_Camera", SendMessageOptions.DontRequireReceiver);
                _gunCamera.enabled = false;
                _gunCamera.GetComponent<AudioListener>().enabled = false;
            }
            else
            {
                Vector3 lookAtPos;
                var ray = new Ray(transform.position, transform.forward);
                if (Physics.Raycast(ray, out RaycastHit raycastHit, 2048.0f, Layer_Settings_CS.Aiming_Layer_Mask))
                {
                    lookAtPos = raycastHit.point;
                }
                else
                {
                    lookAtPos = transform.position + (transform.forward * 2048.0f);
                }
                _cameraPointsManager.SendMessage("Enable_Camera", lookAtPos, SendMessageOptions.DontRequireReceiver);
            }
        }

        if (_mode)
        {
            _zoomInput = -Input.GetAxis("Mouse ScrollWheel") * 2.0f;
            Zoom();
            if (_isCommanderControl && Input.GetKey(_cammanderTurretControl))
            {
                _turretHorizontalCS.enabled = true;
                _cannonVerticalCS.enabled = true;
            }

            else
            {
                _turretHorizontalCS.enabled = false;
                _cannonVerticalCS.enabled = false;
            }
        }

        else
        {
            _turretHorizontalCS.enabled = true;
            _cannonVerticalCS.enabled = true;
        }
    }

    void Zoom()
    {
        _targetFOV *= 1.0f + _zoomInput;
        _targetFOV = Mathf.Clamp(_targetFOV, _minimumFOV, _maximumFOV);

        if (_currentFOV != _targetFOV)
        {
            _currentFOV = Mathf.SmoothDamp(_currentFOV, _targetFOV, ref _currentZoomVelocity, 2.0f * Time.deltaTime);
            _commanderCamera.fieldOfView = _currentFOV;
        }
    }
}