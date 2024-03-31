using UnityEngine;
using System.Collections;
using System;

namespace ChobiAssets.PTM
{

	public class Bullet_Control_CS : MonoBehaviour
	{
        /*
		 * This script is attached to bullet prefabs.
		 * This script controls the posture of the bullet, and supports the collision detecting by casting a ray while flying.
		 * When the bullet hits the target, this script sends the damage value to the "Damage_Control_##_##_CS" script in the hit collider.
		 * The damage value is calculated considering the hit angle.
		*/
        public BulletSettings settings;

        [SerializeField] protected Transform[] fragments;
        [SerializeField] protected int _fragmentsModulDamage;
        [SerializeField] protected Transform _fragmentsParent;
        [SerializeField] protected int modulDamage;
        [SerializeField] protected int _penetrationDamage;
        [SerializeField] protected float distanceWithoutPenetrationReductionByDistance;
        [SerializeField] protected float penetrationReductionByDistance; // X mm for 100 meterts
        [SerializeField] protected float _minimalPenetrationDamage = 100f;
        [SerializeField] protected float _fragmentDistance = 10;
        protected float _penetrationKoeficent = 5;
        private float _maximumDamageForAllFragments;
        
        private Vector3 _destination;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _noControlAtgmDistance;
        [SerializeField] private float _rotationSpeed;
        private bool isControlled = true;
        [SerializeField] private float speed;

        public Vector3 collisionPosition;
        public Vector3 startPosition;
        private float _shotDis;

        // User options >>
        private Rigidbody rigibody;
		// Only for AP
		public GameObject Impact_Object;
		public GameObject Ricochet_Object;
		// Only for HE
		public GameObject Explosion_Object;
		public float Explosion_Force;
		public float Explosion_Radius;
		// << User options
		
		public float Life_Time;
		public float Attack_Multiplier = 1.0f;
		public bool Debug_Flag = false;

        public int _burningModificator = 1;

        bool isLiving = true;


        void Start()
		{
            startPosition = transform.position;
            Initialize();
            _maximumDamageForAllFragments = _penetrationDamage/_penetrationKoeficent;
        }

        public void SetShootPoint(Transform shootPoint)
        {
            _shootPoint = shootPoint; 
        }

        private void FixedUpdate()
        {
            if (settings.bulletType != BulletType.ATGM)
            {
                return;
            }

            if (!isControlled)
            {
                return;
            }
            var distance = Vector3.Distance(transform.position, _shootPoint.position);
            if (distance > _noControlAtgmDistance)
            {
                isControlled = false;
                rigibody.AddForce(speed * transform.forward, ForceMode.Impulse);
                return;
            }
            var ray = new Ray(_shootPoint.position, _shootPoint.forward);           
            Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 0f);
            _destination = ray.origin + ray.direction* 1000f;

            rigibody.velocity = (_destination - transform.position).normalized * speed;
        }

        void Initialize()
        {
           
            if (rigibody == null)
            {
                rigibody = GetComponent<Rigidbody>();
            }

            // Set the collision detection mode.
            rigibody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            Destroy(this.gameObject, Life_Time);
        }

        private void SpawnFragments(float damageDone)
        {
            Debug.Log("spawn fragments");
            int layerMask = LayerMask.GetMask("ignoreAll");
            //_fragmentsParent.DetachChildren();
            var i = 0f;
            var fragmentsCount = damageDone / (_maximumDamageForAllFragments/fragments.Length);
            var finelFragmentDistance = damageDone/(_maximumDamageForAllFragments/_fragmentDistance);
            foreach (Transform fragment in fragments)
            {
                if (i>fragmentsCount)
                {
                    break;
                }
                RaycastHit hit;
                if (Physics.Raycast(fragment.position, fragment.TransformDirection(Vector3.forward), out hit, finelFragmentDistance, layerMask))
                {
                    Debug.DrawRay(fragment.position, fragment.TransformDirection(Vector3.forward) * hit.distance, Color.red, 14f);
                    Debug.Log("Did Hit "+ hit.transform.name);
                    var modul = hit.transform.GetComponent<ModulBase>();
                    if (modul != null)
                    {
                        Debug.Log("Modul Hit " + modul.name);
                        modul.setBullet(this);
                        modul.GetDamage(_fragmentsModulDamage);
                    }
                }
                else
                {
                    Debug.DrawRay(fragment.position, fragment.TransformDirection(Vector3.forward) * finelFragmentDistance, Color.yellow, 14f);
                    Debug.Log("Did not Hit");
                }
                i++;
            }
            // DestroyBullet();
        }

        void Update()
        {
            if (isLiving == false)
            {
                return;
            }
            if (settings.bulletType == BulletType.ATGM)
            {
                return;
            }
            // Set the posture.
            transform.LookAt(rigibody.position + rigibody.velocity);
        }

        void OnCollisionEnter(Collision collision)
        { // The collision has been detected by the physics engine.
            if (isLiving)
            {
                collisionPosition = transform.position;                                               
                var shotDis = Vector3.Distance(startPosition, collisionPosition);
                Debug.Log(shotDis);
                _shotDis = shotDis;               
                //Start the hit process.
                switch (settings.bulletType)
                {
                    case BulletType.APFSDS: // AP
                        NonExplosiveProjectile(collision, collision.relativeVelocity.magnitude, collision.contacts[0].normal);
                        break;

                    default: // Non rikoshet/explosive (HE, ATGM, HEAT, HESH, HERADIO)
                        ExplosiveProjectile(collision, collision.relativeVelocity.magnitude, collision.contacts[0].normal);
                        break;
                }
            }
        }
       
        private float CaculatePenetratiomDamage()
        {
            //tut budet samo snijenie penetracii
            if (_shotDis > distanceWithoutPenetrationReductionByDistance)
            {                
                float _raznost = _shotDis - distanceWithoutPenetrationReductionByDistance;
                return Mathf.Clamp(_penetrationDamage - ((_raznost/100f) * penetrationReductionByDistance), _minimalPenetrationDamage, 100000f);
            }
            return _penetrationDamage;
        }

        private void DamageSystem(Collision collision, float hitVelocity, Vector3 hitNormal)
        {
            var hitObject = collision.collider;
            var DZ = hitObject.gameObject.GetComponent<ExplosiveReactiveArmour>();
            if (DZ != null)
            {
                hitObject.enabled = false;
                DamageReduction(DZ.GetModulDamage(settings.bulletType), DZ.GetPenitrationDamage(settings.bulletType));               
                RaycastHit hit;

                if (Physics.Raycast(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.forward), out hit, 10f))
                {
                    Debug.DrawRay(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red, 14f);
                    Debug.Log("Did Hit " + hit.transform.name);
                    var armor = hit.transform.GetComponent<armor_panel>();
                    HitArmor(armor, collision);
                }
                else
                {
                    Debug.DrawRay(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.forward) * 100f, Color.yellow, 14f);
                    Debug.Log("Did not Hit");
                }
                Destroy(hitObject.gameObject);
            }
            else
            {              
                var armor = hitObject.gameObject.GetComponent<armor_panel>();
                HitArmor(armor, collision);
            }
            void HitArmor(armor_panel armor, Collision collision)
            {
                if (armor == null)
                {
                    if (Impact_Object)
                    {                      
                        Instantiate(Impact_Object, transform.position, Quaternion.identity);
                    }
                    return; 
                }
                //SISTEMA RASCHETA UGLA, VLAD JDEM FIX!!!!!!!

                //var angle = Math.Abs(90 - (Vector3.Angle(transform.forward, hitObject.contacts[0].normal)));
                //var isPinetrate = (_penetrationDamage-(armor.GetThicknes() / Math.Abs(Mathf.Cos(angle)))) > 0;
                var contactNormal = collision.contacts[0].normal;
                var bulletTraektractor = gameObject.transform.TransformDirection(Vector3.back);

                Debug.DrawRay(collision.contacts[0].point, contactNormal * 10, Color.cyan, 14f);
                Debug.DrawRay(collision.contacts[0].point, bulletTraektractor * 10, Color.green, 14f);

                var angle = Vector3.Angle(contactNormal, bulletTraektractor);

                float privedennayaArmor = armor.GetThicknes();
                if (angle != 0)
                {
                    var radianAngle = Mathf.Deg2Rad * angle;
                    var cos = Mathf.Cos(radianAngle);
                    privedennayaArmor /= cos; //Z = A/Cos(90-B) A=Armor, B=Ugol
                }
                var isPinetrate = CaculatePenetratiomDamage()-privedennayaArmor;

                Debug.Log("privedenka" + privedennayaArmor);
                Debug.Log("isPinetrate " + isPinetrate  /*" angle " + angle*/);
                if (isPinetrate>0)
                {
                    _fragmentsParent.position = collision.GetContact(0).point;
                    SpawnFragments(isPinetrate);
                }
                else
                {
                    Debug.Log("Not penitrate");
                }
                
            }
        }

        private void NonExplosiveProjectile(Collision hitObject, float hitVelocity, Vector3 hitNormal)
        {
            isLiving = false;

            // Set the collision detection mode.
            rigibody.collisionDetectionMode = CollisionDetectionMode.Discrete;

            if (hitObject.collider.gameObject == null)
            { // The hit object had been removed from the scene.
                return;
            }
            DamageSystem( hitObject,  hitVelocity,  hitNormal);
        }


        void ExplosiveProjectile (Collision hitObject, float hitVelocity, Vector3 hitNormal)
        {
            isLiving = false;
            if (Explosion_Object)
            {
                Instantiate(Explosion_Object, transform.position, Quaternion.identity);
            }

            DamageSystem(hitObject, hitVelocity, hitNormal);

            // Remove the useless components.
            Destroy(GetComponent<Renderer>());
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<Collider>());

            // Add the explosion force to the objects within the explosion radius.
            var colliders = Physics.OverlapSphere(transform.position, Explosion_Radius, Layer_Settings_CS.Layer_Mask);
            for (int i = 0; i < colliders.Length; i++)
            {
                Add_Explosion_Force(colliders[i]);
            }

            Destroy(gameObject, 0.01f * Explosion_Radius);

            void Add_Explosion_Force(Collider collider)
            {
                if (collider == null)
                {
                    return;
                }

                Vector3 direction = (collider.transform.position - transform.position).normalized;
                var ray = new Ray();
                ray.origin = rigibody.position;
                ray.direction = direction;
                if (Physics.Raycast(ray, out RaycastHit raycastHit, Explosion_Radius, Layer_Settings_CS.Layer_Mask))
                {
                    if (raycastHit.collider != collider)
                    { // The collider should be behind an obstacle.
                        return;
                    }

                    // Calculate the distance loss rate.
                    var loss = Mathf.Pow((Explosion_Radius - raycastHit.distance) / Explosion_Radius, 2);

                    // Add force to the rigidbody.
                    Rigidbody rigidbody = collider.GetComponent<Rigidbody>();
                    if (rigidbody)
                    {
                        rigidbody.AddForce(direction * Explosion_Force * loss);
                    }

                    // Send the damage value to "Damage_Control_##_##_CS" script in the collider.
                    var damageScript = collider.GetComponent<Damage_Control_00_Base_CS>();
                    if (damageScript != null)
                    { // The collider should be a breakable object.
                        var damageValue = settings.attackPoint * loss * Attack_Multiplier;
                        damageScript.Get_Damage(damageValue, settings.bulletType);
                        // Output for debugging.
                        if (Debug_Flag)
                        {
                            Debug.Log("HE Damage " + damageValue + " on " + collider.name);
                        }
                    }
                }
            }
        }


       
        public void DamageReduction(int modulReducation, int penetrationReducation)
        {
            modulDamage = Mathf.Clamp(modulDamage - modulReducation, 0, 10000);
            _penetrationDamage = Mathf.Clamp(_penetrationDamage - penetrationReducation, 0, 10000);
            Debug.Log("Modul damaged" + modulDamage + "penetration damage" + _penetrationDamage);
            if (_penetrationDamage <=0)
            {
                Destroy(gameObject);
            }
        }
    }

}