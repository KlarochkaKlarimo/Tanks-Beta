using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using ChobiAssets.PTM;

public class LaserRangeFinder : MonoBehaviour
{
    [SerializeField] private Text _rangeFinderDistanceSight;
    [SerializeField] private Text _rangeFinderDistanceSmall;

    [SerializeField] private Transform bulletGeneratorTransform;
    [SerializeField] private Aiming_Control_CS aimingScript;
    [SerializeField] private Bullet_Generator_CS Bullet_Generator_Script;
    [SerializeField] private float Calculation_Time = 2f;
    [SerializeField] private Image markerImage;
    [SerializeField] private Gun_Camera_CS _gunCamera;

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
        
        var previousPos = muzzlePos;
        var currentPos = previousPos;

        //var screenCenter = new Vector3();

        //screenCenter.x = Screen.width * 0.5f;
        //screenCenter.y = Screen.height * 0.5f;

        //var ray = Camera.main.ScreenPointToRay(screenCenter);

        //RaycastHit hit;

        //if (Physics.Raycast(ray.origin, ray.direction, out hit))
        //{
        //    aimingScript.Target_Transform = hit.transform;
        //    aimingScript.Target_Rigidbody = hit.rigidbody;
        //    aimingScript.Target_Position = hit.point;
        //    aimingScript.Mode = 2;
        //    aimingScript.Switch_Mode();

        //    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 20f);
        //    return;      
        //}

        if (aimingScript.Target_Rigidbody)
        { // The target has a rigidbody.
          // Reduce the target's velocity to help the lead-shooting.
            
            bulletVelocity -= aimingScript.Target_Rigidbody.velocity;

            var lws = aimingScript.Target_Rigidbody.transform.gameObject.GetComponent<LWSSector>();
            if (lws != null)
            {
                lws.LaserWarning();
            }
        }

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
                VisualImage();
                break;
            }

            // Check the ray has exceeded the target.
            //var currenBase = Vector2.Distance(Vector2.zero, new Vector2(virtualPos.x, virtualPos.z));
            //if (currenBase > targetBase)
            //{
            //    VisualImage();
            //    break;
            //}

            previousPos = currentPos;
            count += Time.fixedDeltaTime;
        }

        void VisualImage()
        {
            // Convert the hit point to the screen point.           
            var screenPos = Camera.main.WorldToScreenPoint(currentPos);
            if (screenPos.z < 0.0f)
            { // The hit point is behind the camera.
                markerImage.enabled = false;
                _rangeFinderDistanceSight.text = "9999";
                return;
            }

            int metrDist = (int)Vector3.Distance(muzzlePos, currentPos);
            _rangeFinderDistanceSight.text = metrDist.ToString();
            _rangeFinderDistanceSmall.text = metrDist.ToString() + " meters";

            // Set the position.
            markerImage.enabled = true;
            screenPos.z = 128.0f;
            markerImage.transform.position = screenPos;

            Camera.main.transform.LookAt(currentPos);
        }
    }
}
