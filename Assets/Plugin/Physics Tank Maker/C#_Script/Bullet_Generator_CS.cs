using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace ChobiAssets.PTM
{
    [Serializable]
    public class BulletSettings
    {
        public GameObject prefab;
        [Range(0f, 10000f)]
        public float attackPoint = 500f;
        [Range(0f, 10000f)]
        public float initialVelocity = 500f;
        public BulletType bulletType;
        public string bulletName;
        public int ammoCount;
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

        [SerializeField] private Text _currentProjectileName;
        [SerializeField] private Cannon_Fire_CS _cannonFireScript;

        public GameObject MuzzleFire_Object;

        public BulletSettings [] bullets;        

        public float Life_Time = 5.0f;
        public int Initial_Bullet_Type = 0;
        public float Offset = 0.5f;
        public bool Debug_Flag = false;

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
            bullets = Resources.Load<TanksSettings>("Tanks Settings").settings[0].bullets;
            currentBulletType = Initial_Bullet_Type - 1; // (Note.) The "currentBulletType" value is added by 1 soon in the "Switch_Bullet_Type()".
            //ChangeAmmoType(0);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                ChangeAmmoType(0);
            }

            if (Input.GetKey(KeyCode.Alpha2))
            {
                ChangeAmmoType(1);
            }

            if (Input.GetKey(KeyCode.Alpha3))
            {
                ChangeAmmoType(2);
            }

            if (Input.GetKey(KeyCode.Alpha4))
            {
                ChangeAmmoType(3);
            }

            if (Input.GetKey(KeyCode.Alpha5))
            {
                ChangeAmmoType(4);
            }
        }

        private void ChangeAmmoType(int bulletNumber)
        {
            if (bulletNumber >= bullets.Length) return;
            currentBulletType = bulletNumber;
            Current_Bullet_Velocity = bullets[currentBulletType].initialVelocity;
            _currentProjectileName.text = bullets[currentBulletType].bulletName + " ( " + bullets[currentBulletType].ammoCount + " )";
            _cannonFireScript.StartCoroutine("Reload");
        }

        public void Switch_Bullet_Type()
        { // Called from "Cannon_Fire_Input_##_##" scripts.

            //TODO vipelit k chertu

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
            var currentBullet = bullets[currentBulletType];
            if(currentBullet.ammoCount == 0)
            {
                yield break;
            }

            if (MuzzleFire_Object)
            {
                Instantiate(MuzzleFire_Object, transform.position, transform.rotation, transform);
            }
            currentBullet.ammoCount--;
            _currentProjectileName.text = bullets[currentBulletType].bulletName + " ( " + bullets[currentBulletType].ammoCount + " )";
            var bulletObject = Instantiate(currentBullet.prefab, transform.position + (transform.forward * Offset), transform.rotation) as GameObject;
            // Set values of "Bullet_Control_CS" in the bullet.
            Bullet_Control_CS bulletScript = bulletObject.GetComponent<Bullet_Control_CS>();
            bulletScript.settings = currentBullet;            
            bulletScript.Life_Time = Life_Time;
            bulletScript.Attack_Multiplier = Attack_Multiplier;
            bulletScript.Debug_Flag = Debug_Flag;
         
            bulletObject.tag = "Finish"; // (Note.) The ray cast for aiming does not hit any object with "Finish" tag.

            bulletObject.layer = Layer_Settings_CS.Bullet_Layer;
            if (bullets[currentBulletType].bulletType == BulletType.ATGM)
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