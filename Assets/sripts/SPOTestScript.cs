using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPOTestScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1000f))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.black, 14f);
                var lws = hit.transform.gameObject.GetComponent<LWSSector>();
                if (lws != null)
                {
                    lws.LaserWarning();
                }
            }
        }
    }
}
