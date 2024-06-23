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
    [SerializeField] private Gun_Camera_CS _gunCamera;
    private Camera _commanderCamera;
    private bool _mode;
    private Gun_Camera_CS _gunCameraScript;
    private void Awake()
    {
        _gunCameraScript = GetComponent<Gun_Camera_CS>();
        _commanderCamera = GetComponent<Camera>();
    }
    public void OffCamera()
    {
        _mode = false;
        _gunCameraScript.Switch_Mode(1);
     //   _gunCameraScript.enabled = false;
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
                _gunCamera.Switch_Mode(1);
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