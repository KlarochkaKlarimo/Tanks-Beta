using ChobiAssets.PTM;
using Den.Tools.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Missile : MonoBehaviour
{
    // TODO dodelat
    [SerializeField] private float _radius;
    [SerializeField] private float _explForse;
    [SerializeField] private Collider _deadCollider;
    [SerializeField] private GameObject _explosionFX;

    private void OnCollisionEnter(Collision collision)
    {
        transform.ExplosionWave<Rigidbody>(_radius, _explForse);
        _deadCollider.enabled = true;
        var vzriv = Instantiate(_explosionFX, transform.position, transform.rotation);
        vzriv.GetComponent<VisualEffect>().Play();
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        var ammorack = other.gameObject.GetComponent<ModulAmmorack>();
        if (ammorack!=null)
        {
            ammorack.ModulDestroyed();
        }
    }
}
