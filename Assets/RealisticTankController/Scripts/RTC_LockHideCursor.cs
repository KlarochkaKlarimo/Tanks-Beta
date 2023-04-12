using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Tank Controller/UI/Lock Cursor")]
public class RTC_LockHideCursor : MonoBehaviour {
	void Awake (){
		
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
}
