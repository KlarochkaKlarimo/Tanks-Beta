using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using ChobiAssets.PTM;

public class LaserRangeFinder : MonoBehaviour
{
    // TODO sdelat sho bi rabotalo na odny knopachku

    [SerializeField] private Text _rangeFinderDistanceSight;
    [SerializeField] private Text _rangeFinderDistanceSmall;

    [SerializeField] private Transform bulletGeneratorTransform;
    [SerializeField] private Aiming_Control_CS aimingScript;
    [SerializeField] private Bullet_Generator_CS Bullet_Generator_Script;
    [SerializeField] private float Calculation_Time = 2f;
    [SerializeField] private Image markerImage;
    [SerializeField] private Gun_Camera_CS _gunCamera;
    [SerializeField] private Cannon_Vertical_CS cannon_Vertical_CS;
    private Transform _target;
    private Vector3 currentPos;
    private Vector3 calculatePos;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Calculating();
        }
    }

    private void Calculating()
    {
        var muzzlePos = bulletGeneratorTransform.position;
        var targetDir = aimingScript.Target_Position - muzzlePos;
        var targetBase = Vector2.Distance(Vector2.zero, new Vector2(targetDir.x, targetDir.z));
        var bulletVelocity = bulletGeneratorTransform.forward * Bullet_Generator_Script.Current_Bullet_Velocity;
        var i = 0;

        SetTarget();
        if (aimingScript.Target_Rigidbody)
        {
            bulletVelocity -= aimingScript.Target_Rigidbody.velocity;

            var lws = aimingScript.Target_Rigidbody.transform.gameObject.GetComponent<LWSSector>();
            if (lws != null)
            {
                lws.LaserWarning();
            }
        }
        CalculatePosition();
        void CalculatePosition()
        {
            var previousPos = muzzlePos;
            currentPos = previousPos;
            var count = 0.0f;
            while (count < Calculation_Time)
            {
                var virtualPos = bulletVelocity * count;
                virtualPos.y -= 0.5f * -Physics.gravity.y * Mathf.Pow(count, 2.0f);
                currentPos = virtualPos + muzzlePos;
                Debug.DrawLine(previousPos, currentPos, Color.green, 20f);

                if (Physics.Linecast(previousPos, currentPos, out RaycastHit raycastHit, Layer_Settings_CS.Aiming_Layer_Mask))
                {
                    currentPos = raycastHit.point;
                    VisualImage(raycastHit);
                    break;
                }
                previousPos = currentPos;
                count += Time.fixedDeltaTime;
            }
        }
        void SetTarget()
        {
            if (_target != null) return;

            var screenCenter = new Vector3();
            screenCenter.x = Screen.width * 0.5f;
            screenCenter.y = Screen.height * 0.5f;
            var ray = Camera.main.ScreenPointToRay(screenCenter);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                _target = hit.transform;
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 20f);
            }
        }
        void VisualImage(RaycastHit hited)
        {
            // Convert the hit point to the screen point.           
            var screenPos = Camera.main.WorldToScreenPoint(currentPos);
            if (i > 15) return;
            if (screenPos.z < 0.0f)
            { // The hit point is behind the camera.
                markerImage.enabled = false;
                _rangeFinderDistanceSight.text = "9999";
                return;
            }
            i++;
            int metrDist = (int)Vector3.Distance(muzzlePos, _target.position);
            _rangeFinderDistanceSight.text = metrDist.ToString();
            _rangeFinderDistanceSmall.text = metrDist.ToString() + " meters";
            if (_target == hited.transform)
            {
                cannon_Vertical_CS._cheak = false;
                markerImage.enabled = true;
                screenPos.z = 128.0f;
                markerImage.transform.position = screenPos;
                Camera.main.transform.LookAt(currentPos);
            }
            else
            {
                cannon_Vertical_CS._cheak = true;
                cannon_Vertical_CS.transform.eulerAngles += (Vector3.left);
                CalculatePosition();
            }

        }
    }
}
