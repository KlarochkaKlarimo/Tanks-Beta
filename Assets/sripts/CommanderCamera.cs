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
    [SerializeField] private Gun_Camera_CS _gunCameraScript;
    [SerializeField] private AudioListener _audioListener;
    private Camera _commanderCamera;
    private bool _mode;
    private Camera _camera;
    private void Awake()
    {
        _gunCameraScript = GetComponent<Gun_Camera_CS>();
        _camera = GetComponent<Camera>();
        _audioListener = GetComponent<AudioListener>();
        _commanderCamera = GetComponent<Camera>();
    }
    public void OffCamera()
    {
        if (!_mode) { return; }
        _mode = false;

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
        _camera.enabled = false;
        _audioListener.enabled = false;
        this.tag = "Untagged";

    }
    private void Update()
    {
        if (Input.GetKeyDown(_cameraKeyCode))
        {
            Debug.Log("_mode " + _mode);
            if (_mode)
            {
                _gunCameraScript.Switch_Mode(1);
                _mode = false;
            }
            else
            {
                //_gunCameraScript.Switch_Mode(1);
                OffCamera();
                _gunCameraScript.Switch_Mode(2);
                _mode = true;
            }

            //    _commanderCamera.enabled = _mode;
        }

        if (_mode)
        {
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
}