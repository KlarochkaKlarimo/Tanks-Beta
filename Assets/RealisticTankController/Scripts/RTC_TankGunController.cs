using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class RTC_TankGunController : MonoBehaviour {


	#region RTC Settings Instance

	private RTC_Settings RTCSettingsInstance;
    private RTC_Settings RTCSettings
    {
        get
        {
            if (RTCSettingsInstance == null)
            {
                RTCSettingsInstance = RTC_Settings.Instance;
            }
            return RTCSettingsInstance;
        }
    }

    #endregion

    private Rigidbody tankRigid;
	private Rigidbody mainGunRigid;
	private RTC_TankController tank;
	private RTC_MainCamera tankCamera;

	internal AimType aimType;
	public enum AimType{Orbit, Direct}

	[Range(0f, 1f)]public float horizontalSensitivity = .05f;
	[Range(0f, 1f)]public float verticalSensitivity = .15f;

	[Space()]
	public GameObject mainGun;

	public bool canControl = true;

	[Space()]
	public GameObject barrel;
	public Transform barrelOut;
	 
	private float steerInput = 0f;
	private float elevatorInput = 0f;

	public int rotationTorque = 1000;

	[Space()]
	public float maximumAngularVelocity = 1.5f;
	public int maximumRotationLimit = 160;
	public float minimumElevationLimit = 10;
	public float maximumElevationLimit = 25;

	internal float rotationVelocity;
	internal float rotationOfTheGun;

	internal Transform locatedTarget;
	internal Transform directedTarget;

	public AmmoList[] _ammoLists;

	public int selectedAmmunation = 0;

	public GameObject projectile{

		get
		{
			return _ammoLists[selectedAmmunation].prefab;
		}

	}
	public int bulletVelocity{

		get{
			return _ammoLists[selectedAmmunation].velocity;
		}

	}
	public int recoilForce{

		get{
			return _ammoLists[selectedAmmunation].recoilForce;
		}

	}

	public int currentAmmo = 15;

	public float reloadTime{

		get{
			return _ammoLists[selectedAmmunation].reloadTime;
		}

	}

	private float loadingTime = 0f;

	public bool reloading{

		get{
			return loadingTime < reloadTime ? true : false;
		}

	}

	private AudioSource fireSoundSource;
	public AudioClip fireSoundClip{

		get{
			return _ammoLists[selectedAmmunation].fireSoundClip;
		}

	}

	public GameObject groundSmoke{

		get{
			return _ammoLists[selectedAmmunation].groundSmoke;
		}

	}
	public GameObject fireSmoke{

		get{
			return _ammoLists[selectedAmmunation].fireSmoke;
		}

	}

	#if RTC_REWIRED
	private static Player player;
	#endif
		
	void Awake () {
		tankRigid = GetComponent<Rigidbody>();
		mainGunRigid = mainGun.GetComponent<Rigidbody>();
		tank = gameObject.GetComponent<RTC_TankController>();
		tankCamera = GameObject.FindObjectOfType<RTC_MainCamera>();

		GameObject newTarget = new GameObject("Located Target");
		locatedTarget = newTarget.transform;

		GameObject newTarget2 = new GameObject("Directed Target");
		directedTarget = newTarget2.transform;
	
		mainGunRigid.maxAngularVelocity = maximumAngularVelocity;
		mainGunRigid.interpolation = RigidbodyInterpolation.None;
		mainGunRigid.interpolation = RigidbodyInterpolation.Interpolate;

		#if RTC_REWIRED
		player = Rewired.ReInput.players.GetPlayer(0);
		#endif

	}

	void Update(){

		if(!tank.canControl || !canControl)
			return;

		if (!tankCamera) {
			Debug.LogError ("RTC Tank Camera not found! Camera needed for aiming...");
			return;
		}

		loadingTime += Time.deltaTime;
		locatedTarget.position = tankCamera.cam.transform.position + (tankCamera.cam.transform.forward * 100f);
		directedTarget.position = barrelOut.transform.position + (barrelOut.transform.forward * 100f);

		Inputs ();

		AmmoChanger();

	}

	void Inputs(){

		switch(tankCamera.cameraMode){

		case RTC_MainCamera.CameraMode.FPS:
			aimType = AimType.Direct;
			break;

		case RTC_MainCamera.CameraMode.ORBIT:
			aimType = AimType.Orbit;
			break;

		}

		Vector3 targetPosition = mainGun.transform.InverseTransformPoint(locatedTarget.transform.position);
		Vector3 targetPosition2 = barrel.transform.InverseTransformPoint(locatedTarget.transform.position);

		switch (RTCSettings.controllerType) {

		case RTC_Settings.ControllerType.Keyboard:

			switch (aimType) {

			case AimType.Orbit:
				steerInput = (targetPosition.x / targetPosition.magnitude);
				elevatorInput = -(targetPosition2.y / targetPosition2.magnitude);
				break;

			case AimType.Direct:
				steerInput = Mathf.Lerp(steerInput, Input.GetAxis (RTCSettings.mainGunXInput) * horizontalSensitivity, Time.deltaTime * 10f);
				elevatorInput = Mathf.Lerp(elevatorInput, -Input.GetAxis (RTCSettings.mainGunYInput) * verticalSensitivity, Time.deltaTime * 10f);
				break;

			}

			if (Input.GetKeyDown (RTCSettings.fireKB)) {
				Fire ();
			}

			break;

		case RTC_Settings.ControllerType.Custom:

			#if RTC_REWIRED
			switch (aimType) {

			case AimType.Orbit:
				steerInput = (targetPosition.x / targetPosition.magnitude);
				elevatorInput = -(targetPosition2.y / targetPosition2.magnitude);
				break;

			case AimType.Direct:
				steerInput = Mathf.Lerp(steerInput, player.GetAxis (RTCSettings.RW_mainGunXInput) * horizontalSensitivity, Time.deltaTime * 100f);
				elevatorInput = Mathf.Lerp(elevatorInput, -player.GetAxis (RTCSettings.RW_mainGunYInput) * verticalSensitivity, Time.deltaTime * 100f);
				break;

			}

			if (player.GetButtonDown (RTCSettings.RW_fireKB)) {
				Fire ();
			}

			if(player.GetButtonDown(RTCSettings.RW_changeAmmunation.ToString())){
				if (selectedAmmunation < RTC_Ammunation.Instance.ammunations.Length - 1)
					selectedAmmunation ++;
				else
					selectedAmmunation = 0;
			}
			#endif

			break;

		}
			
	}

	void FixedUpdate () {

		if(!tank.canControl || !canControl)
			return;

		rotationVelocity = mainGunRigid.angularVelocity.y;

		if(mainGun.transform.localEulerAngles.y > 0 && mainGun.transform.localEulerAngles.y < 180)
			rotationOfTheGun = mainGun.transform.localEulerAngles.y;
		else
			rotationOfTheGun = mainGun.transform.localEulerAngles.y - 360;
	
		mainGunRigid.AddRelativeTorque(0, (rotationTorque) * steerInput, 0, ForceMode.Acceleration);
		barrel.transform.localRotation = barrel.transform.localRotation * Quaternion.AngleAxis(((elevatorInput) * 10f), Vector3.right);


		if (barrel) {

			if (barrel.transform.localEulerAngles.x > 0 && barrel.transform.localEulerAngles.x < 180)
				barrel.transform.localRotation = Quaternion.Euler (new Vector3 (Mathf.Clamp (barrel.transform.localEulerAngles.x, -360, minimumElevationLimit), 0, 0));
			if (barrel.transform.localEulerAngles.x > 180 && barrel.transform.localEulerAngles.x < 360)
				barrel.transform.localRotation = Quaternion.Euler (new Vector3 (Mathf.Clamp (barrel.transform.localEulerAngles.x - 360, -maximumElevationLimit, 360), 0, 0));

		}

	}

	public void Fire(){

		if (loadingTime < reloadTime || currentAmmo <= 0f)
			return;

		tankRigid.AddForce(-mainGun.transform.forward * recoilForce, ForceMode.Impulse);

		var _bullet = Instantiate(projectile, barrelOut.position, transform.rotation);
		_bullet.GetComponent<Bullet>().SetVariables(_ammoLists[selectedAmmunation].bulletPenetration, 40, false, mainGun);
		var atgm = _bullet.GetComponent<Atgm>();
		if (atgm != null)
		{
			atgm.SetShootPoint(barrelOut);
		}
		currentAmmo --;
		loadingTime = 0;

	}
	public void AmmoChanger()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			ChangeAmmo(0);
		}

		if (Input.GetKey(KeyCode.Alpha2))
		{
			ChangeAmmo(1);
		}

		if (Input.GetKey(KeyCode.Alpha3))
		{
			ChangeAmmo(2);
		}
	}

	private void ChangeAmmo(int index)
	{
		selectedAmmunation = index;
		loadingTime = 0;
	}

	public void ChangeAmmunation(){

		if (selectedAmmunation < RTC_Ammunation.Instance.ammunations.Length - 1)
			selectedAmmunation ++;
		else
			selectedAmmunation = 0;

	}

	public void ChangeAmmunation(int ammunationIndex){

		if (ammunationIndex < RTC_Ammunation.Instance.ammunations.Length - 1)
			selectedAmmunation = ammunationIndex;

	}

}
