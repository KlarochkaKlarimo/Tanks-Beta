using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ChobiAssets.PTM
{

	[ RequireComponent (typeof(Camera))]

	public class Reticle_Control_CS : MonoBehaviour
	{
        [SerializeField] private GameObject reticleObject;
        [SerializeField] private GameObject thirdPersonAim;
        [SerializeField] private RectTransform reticleImage;

        public Gun_Camera_CS Gun_Camera_Script;
              
        bool isSelected;

        void Start()
        {            
            if (Gun_Camera_Script == null)
            {
                Gun_Camera_Script = GetComponent<Gun_Camera_CS>();
            }
        }


        void Update()
        {
            if (isSelected == false)
            {
                return;
            }

            reticleObject.SetActive(Gun_Camera_Script.Gun_Camera.enabled);
            thirdPersonAim.SetActive(!Gun_Camera_Script.Gun_Camera.enabled);

            if (reticleObject.activeInHierarchy)
            {
                // Change the scale according to the FOV.
                var currentScale = Gun_Camera_Script.Maximum_FOV / Gun_Camera_Script.Gun_Camera.fieldOfView;
                reticleImage.localScale = Vector3.one * currentScale;
            }
        }


        void Selected(bool isSelected)
        { // Called from "ID_Settings_CS".
            if (isSelected)
            {
                this.isSelected = true;
            }
            else
            {
                if (this.isSelected)
                { // This tank is selected until now.
                    this.isSelected = false;
                    reticleObject.SetActive(false);
                }
            }
        }


        void Turret_Destroyed_Linkage()
        { // Called from "Damage_Control_Center_CS".

            // Turn off the image.
            if (isSelected)
            {
                reticleObject.SetActive(false);
            }

            Destroy(this);
        }


        void Pause(bool isPaused)
        { // Called from "Game_Controller_CS".
            this.enabled = !isPaused;
        }

    }

}

