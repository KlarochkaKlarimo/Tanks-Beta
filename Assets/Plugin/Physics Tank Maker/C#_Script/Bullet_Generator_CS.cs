using UnityEngine;
using System.Collections;
using System;

namespace ChobiAssets.PTM
{
    [Serializable]
    public class BulletSettings
    {
        public GameObject prefab;
        public float attackPoint = 500f;
        public float initialVelocity = 500f;
        public BulletType bulletType;
        public string bulletName;
    }

    [Serializable]
    public enum BulletType 
    { 
        APFSDS,
        HE,
        ATGM,
        HEAT,
        HESH
    }

    public class Bullet_Generator_CS : MonoBehaviour
    {
        /* 
		 * This script is attached to the "Bullet_Generator" under the "Barrel_Base" in the tank.
		 * This script instantiates the bullet prefab and shoot it from the muzzle.
		 * Also you can create a prefab for the bullet using this script.
		*/
     
        public GameObject MuzzleFire_Object;

        public BulletSettings [] bullets;

        public float Life_Time = 5.0f;
        public int Initial_Bullet_Type = 0;
        public float Offset = 0.5f;
        public bool Debug_Flag = false;
        // << User options

        public float Attack_Multiplier = 1.0f; // Set by "Special_Settings_CS".
        public int Barrel_Type = 0; // Set by "Barrel_Base". (0 = Single barrel, 1 = Left of twins, 2 = Right of twins)
        public float Current_Bullet_Velocity; // Referred to from "Turret_Horizontal_CS", "Cannon_Vertical_CS", "UI_Lead_Marker_Control_CS".
        int currentBulletType;

        // Only for AI tank.
        public bool Can_Aim; // Set by "AI_CS", and referred to from "Cannon_Fire_Input_99_AI_CS" script.

        private Vector3 _spread;

        void Start()
        {
            // Switch the bullet type at the first time.
            currentBulletType = Initial_Bullet_Type - 1; // (Note.) The "currentBulletType" value is added by 1 soon in the "Switch_Bullet_Type()".
            Switch_Bullet_Type();
        }

        public void Switch_Bullet_Type()
        { // Called from "Cannon_Fire_Input_##_##" scripts.
            currentBulletType ++;
            if (currentBulletType > bullets.Length)
            {
                currentBulletType = 0;
            }      
            Current_Bullet_Velocity = bullets[currentBulletType].initialVelocity;
        }

        public void Fire_Linkage(int direction, Vector3 spread)
        { // Called from "Cannon_Fire_CS".
            if (Barrel_Type == 0 || Barrel_Type == direction)
            { // Single barrel, or the same direction.
                _spread = spread; 
                // Generate the bullet and shoot it.
                StartCoroutine("Generate_Bullet");
            }
        }

        IEnumerator Generate_Bullet()
        {           
            if (MuzzleFire_Object)
            {
                Instantiate(MuzzleFire_Object, transform.position, transform.rotation, transform);
            }     
            var currentBullet = bullets[currentBulletType];
            var bulletObject = Instantiate(currentBullet.prefab, transform.position + (transform.forward * Offset), transform.rotation) as GameObject;                      

            // Set values of "Bullet_Control_CS" in the bullet.
            Bullet_Control_CS bulletScript = bulletObject.GetComponent<Bullet_Control_CS>();
            bulletScript.Attack_Point = currentBullet.attackPoint;
            bulletScript.Initial_Velocity = Current_Bullet_Velocity;
            bulletScript.Life_Time = Life_Time;
            bulletScript.Attack_Multiplier = Attack_Multiplier;
            bulletScript.Debug_Flag = Debug_Flag;
         
            bulletObject.tag = "Finish"; // (Note.) The ray cast for aiming does not hit any object with "Finish" tag.

            bulletObject.layer = Layer_Settings_CS.Bullet_Layer;
            if (currentBulletType == 2)
            {
                bulletScript.SetShootPoint(transform);
                yield break;
            }

            yield return new WaitForFixedUpdate();
            Rigidbody rigidbody = bulletObject.GetComponent<Rigidbody>();
            Vector3 currentVelocity = (bulletObject.transform.forward + _spread) * Current_Bullet_Velocity;
            rigidbody.velocity = currentVelocity;

        }
    }
}