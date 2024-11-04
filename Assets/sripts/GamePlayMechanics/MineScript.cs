using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;
using UnityEngine.VFX;

public class MineScript : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private float _explForse;
    [SerializeField] private Collider _vzrivatel;
    [SerializeField] private bool _hasCollided = false;
    [SerializeField] private float _explosionDelay;
    [SerializeField] private GameObject _explosionFX;
    [SerializeField] private float _otorvatKotokDistancia;
    [SerializeField] private GameObject _mine;
    

    private void OnCollisionEnter(Collision collision)
    {
        var wheel = collision.collider.gameObject.GetComponent<TrackColider>();
        if (!_hasCollided && wheel!=null)
        {
            _hasCollided = true;
            StartCoroutine(ExplodeAfterDelay());            
        }
    }
    
    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(_explosionDelay);
        Explode();
    }

    private void Explode()
    {
        var damagedObjects = transform.ExplosionWave<Drive_Wheel_CS>(_radius, _explForse);

        foreach (var nearbyObject in damagedObjects)
        {            
            if (Vector3.Distance(transform.position, nearbyObject.transform.position)<_otorvatKotokDistancia)
            {
                nearbyObject.transform.SetParent(null);
                var hitJoint = nearbyObject.gameObject.GetComponent<HingeJoint>();
                if (hitJoint != null)
                {
                    Destroy(hitJoint);
                    nearbyObject.gameObject.GetComponent<Drive_Wheel_CS>().enabled = false;                    
                    nearbyObject.gameObject.GetComponent<Stabilizer_CS>().enabled = false;
                }
            }   
        }
        var vzriv = Instantiate(_explosionFX, transform.position, transform.rotation);
        vzriv.GetComponent<VisualEffect>().Play();
        Destroy(_mine);
    }
}
