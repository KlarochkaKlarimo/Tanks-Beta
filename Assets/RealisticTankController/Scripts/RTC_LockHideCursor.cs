//----------------------------------------------
//            Realistic Tank Controller
//
// Copyright © 2014 - 2017 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

[AddComponentMenu("BoneCracker Games/Realistic Tank Controller/UI/Lock Cursor")]
public class RTC_LockHideCursor : MonoBehaviour {


	void Awake (){
		
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

	}
	
	

}
