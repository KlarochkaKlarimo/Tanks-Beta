using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using ChobiAssets.PTM;

public class LaserRangeFinder : MonoBehaviour
{
    [SerializeField] private Text _rangeFinderDistance;

    [SerializeField] private Transform bulletGeneratorTransform;
    [SerializeField] private Aiming_Control_CS aimingScript;
    [SerializeField] private Bullet_Generator_CS Bullet_Generator_Script;
    [SerializeField] private float Calculation_Time = 2f;
    [SerializeField] private Image markerImage;

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

        var previousPos = muzzlePos;
        var currentPos = previousPos;
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
                _rangeFinderDistance.text = "9999";
                return;
            }

            int metrDist = (int)Vector3.Distance(muzzlePos, currentPos);
            _rangeFinderDistance.text = metrDist.ToString();

            // Set the position.
            markerImage.enabled = true;
            screenPos.z = 128.0f;
            markerImage.transform.position = screenPos;

            Camera.main.transform.LookAt(currentPos);
        }
    }
}
