﻿using UnityEngine;

// Visualises the minimum translation vectors required to separate apart from other colliders found in a given radius
// Attach to a GameObject that has a Collider attached.
[ExecuteInEditMode()]
public class ShowPenetration : MonoBehaviour
{
    public float radius = 3f; // show penetration into the colliders located inside a sphere of this radius
    public int maxNeighbours = 16; // maximum amount of neighbours visualised

    private Collider[] neighbours;

    public void Start()
    {
        neighbours = new Collider[maxNeighbours];
    }

    public void OnDrawGizmos()
    {
        var thisCollider = GetComponent<Collider>();

        if (!thisCollider)
            return; // nothing to do without a Collider attached

        int count = Physics.OverlapSphereNonAlloc(transform.position, radius, neighbours);

        for (int i = 0; i < count; ++i)
        {
            var collider = neighbours[i];

            if (collider == thisCollider)
                continue; // skip ourself

            Vector3 otherPosition = collider.gameObject.transform.position;
            Quaternion otherRotation = collider.gameObject.transform.rotation;

            Vector3 direction;
            float distance = 10f;

            bool overlapped = Physics.ComputePenetration(
                thisCollider, transform.position, transform.rotation,
                collider, otherPosition, otherRotation,
                out direction, out distance
            );

            // draw a line showing the depenetration direction if overlapped
            if (overlapped)
            {
                distance = 10f;
                Debug.Log("overlap");
                Gizmos.color = Color.red;
                Gizmos.DrawRay(otherPosition, direction * distance);
            }
        }
    }
}