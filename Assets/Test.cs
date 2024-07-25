using ChobiAssets.PTM;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    Transform bulletGeneratorTransform;
    [SerializeField] Aiming_Control_CS aimingScript;
    [SerializeField] Bullet_Generator_CS Bullet_Generator_Script;
    [SerializeField] float Calculation_Time = 2f;
    [SerializeField] Image markerImage;
    private void Awake()
    {
        bulletGeneratorTransform = Bullet_Generator_Script.transform;
    }
    private void LateUpdate()
    {
        T();
    }
    private void T()
    {
        var muzzlePos = bulletGeneratorTransform.position;
        var targetDir = aimingScript.Target_Position - muzzlePos;
        var targetBase = Vector2.Distance(Vector2.zero, new Vector2(targetDir.x, targetDir.z));
        var bulletVelocity = bulletGeneratorTransform.forward * Bullet_Generator_Script.Current_Bullet_Velocity;
        if (aimingScript.Target_Rigidbody)
        { // The target has a rigidbody.
          // Reduce the target's velocity to help the lead-shooting.
            bulletVelocity -= aimingScript.Target_Rigidbody.velocity;
        }

        var previousPos = muzzlePos;
        var currentPos = previousPos;
        var count = 0.0f;
        while (count < Calculation_Time)
        {
            var virtualPos = bulletVelocity * count;
            virtualPos.y -= 0.5f * -Physics.gravity.y * Mathf.Pow(count, 2.0f);
            currentPos = virtualPos + muzzlePos;

            if (Physics.Linecast(previousPos, currentPos, out RaycastHit raycastHit, Layer_Settings_CS.Aiming_Layer_Mask))
            {               
                currentPos = raycastHit.point;
                VisualImage();
                break;
            }

            // Check the ray has exceeded the target.
            var currenBase = Vector2.Distance(Vector2.zero, new Vector2(virtualPos.x, virtualPos.z));
            if (currenBase > targetBase)
            {
                VisualImage();
                break;
            }

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
                return;
            }

            // Set the position.
            markerImage.enabled = true;
            screenPos.z = 128.0f;
            markerImage.transform.position = screenPos;
        }
    }
}
