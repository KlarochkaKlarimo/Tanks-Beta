using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        Instantiate(explosion, transform.position,Quaternion.identity);

        print("collision"+collision.gameObject.name);
        print(transform.position);
    }
}
