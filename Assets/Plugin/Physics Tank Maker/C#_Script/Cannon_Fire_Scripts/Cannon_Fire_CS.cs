﻿using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

	public class Cannon_Fire_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the "Cannon_Base" in the tank.
		 * This script controls the firining of the tank.
		 * When firing, this script calls "Bullet_Generator_CS" and "Recoil_Brake_CS" scripts placed under this object in the hierarchy.
		 * In case of AI tank, this script works in combination with "AI_CS", "Turret_Horizontal_CS", "Cannon_Vertical_CS" and "Aiming_Control_CS".
		*/

		// User options >>
        [Range(0.01f, 60.0f)]
		public float Reload_Time = 2.0f;
        [Range(0.0f, 30000.0f)]
        public float Recoil_Force = 5000.0f;
        // << User options

        [SerializeField] private GameObject _isReloadingTxt;
        [SerializeField] private AudioSource _reloadSound;
         
		// Set by "inputType_Settings_CS".
		[HideInInspector] public int inputType = 0;

        // Referred to from "UI_Reloading_Circle_CS".
        [HideInInspector] public float Loading_Count;
        [HideInInspector] public bool Is_Loaded = true;

        Rigidbody bodyRigidbody;
        Transform thisTransform;
        int direction = 1; // For twin barrels, 1 = left, 2 = right.
        [HideInInspector] public Bullet_Generator_CS[] Bullet_Generator_Scripts; // Referred to from "Cannon_Fire_Input_##_###".
        Recoil_Brake_CS[] recoilScripts;

        protected Cannon_Fire_Input_00_Base_CS inputScript;

        bool isSelected;
        private bool _isCannonDamaged;
        private bool _isBreachGunDamaged;

        [SerializeField] private int _misFireChance;

        public void CannonDamage()
        {
            _isCannonDamaged = true;
        }

        public void DamagedBreachGun()
        {
            _isBreachGunDamaged = true;
        }

        void Start()
		{
			Initialize();
		}


        void Initialize()
        { // This function must be called in Start() after changing the hierarchy.
            thisTransform = transform;
            Bullet_Generator_Scripts = GetComponentsInChildren<Bullet_Generator_CS>();
            recoilScripts = thisTransform.parent.GetComponentsInChildren<Recoil_Brake_CS>();
            bodyRigidbody = GetComponentInParent<Rigidbody>();

            // Get the input type.
            if (inputType != 10)
            { // This tank is not an AI tank.
                inputType = General_Settings_CS.Input_Type;
            }

            // Set the "inputScript".
            Set_Input_Script(inputType);

            // Prepare the "inputScript".
            if (inputScript != null)
            {
                inputScript.Prepare(this);
            }
        }


        protected virtual void Set_Input_Script(int type)
        {
            switch (type)
            {
                case 0: // Mouse + Keyboard (Stepwise)
                case 1: // Mouse + Keyboard (Pressing)
                    inputScript = gameObject.AddComponent<Cannon_Fire_Input_01_Mouse_CS>();
                    break;

                case 2: // GamePad (Single stick)
                case 3: // GamePad (Twin stick)
                    inputScript = gameObject.AddComponent<Cannon_Fire_Input_02_For_Sticks_Drive_CS>();
                    break;

                case 4: // GamePad (Triggers)
                    inputScript = gameObject.AddComponent<Cannon_Fire_Input_03_For_Triggers_Drive_CS>();
                    break;

                case 10: // AI
                    inputScript = gameObject.AddComponent<Cannon_Fire_Input_99_AI_CS>();
                    break;
            }
        }


        void Update()
        {
            _isReloadingTxt.SetActive(Is_Loaded == false);

            if (Is_Loaded == false)
            {
                return;
            }

            if (isSelected || inputType == 10)
            { // The tank is selected, or AI.
                inputScript.Get_Input();
            }
        }


        public void Fire()
        { // Called from "Cannon_Fire_Input_##_###".
            // Call all the "Bullet_Generator_CS".
            if (_isBreachGunDamaged)

            {
                var chance = Random.Range(0, 100);
                _misFireChance = 50;
                var _isMisFire = chance< _misFireChance;
                Debug.Log(chance);
                if (_isMisFire)
                {
                    // Reload.
                    StartCoroutine("Reload");
                    return;
                }
            }
            var spread = _isCannonDamaged ? new Vector3(0, Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f)) : Vector3.zero;
            for (int i = 0; i < Bullet_Generator_Scripts.Length; i++)
            {
                Bullet_Generator_Scripts[i].Fire_Linkage(direction, spread);
            }

            // Call all the "Recoil_Brake_CS".
            for (int i = 0; i < recoilScripts.Length; i++)
            {
                recoilScripts[i].Fire_Linkage(direction);
            }

            // Add recoil shock force to the MainBody.
            bodyRigidbody.AddForceAtPosition(-thisTransform.forward * Recoil_Force, thisTransform.position, ForceMode.Impulse);

            // Reload.
            StartCoroutine("Reload");
            _reloadSound.Play();
        }


        public IEnumerator Reload()
        { // Called also from "Cannon_Fire_Input_##_###".
            Is_Loaded = false;
            Loading_Count = 0.0f;

            while (Loading_Count < Reload_Time)
            {
                Loading_Count += Time.deltaTime;
                yield return null;
            }

            Is_Loaded = true;
            Loading_Count = Reload_Time;

            // Set direction for twin barrels, 1 = left, 2 = right.
            if (direction == 1)
            {
                direction = 2;
            }
            else
            {
                direction = 1;
            }
        }


        void Get_AI_CS()
        { // Called from "AI_CS".
            inputType = 10;
        }


        void Selected(bool isSelected)
        { // Called from "ID_Settings_CS".
            this.isSelected = isSelected;
        }


        void Turret_Destroyed_Linkage()
        { // Called from "Damage_Control_Center_CS".
            Destroy(this);
        }


        void Pause(bool isPaused)
        { // Called from "Game_Controller_CS".
            this.enabled = !isPaused;
        }

    }

}