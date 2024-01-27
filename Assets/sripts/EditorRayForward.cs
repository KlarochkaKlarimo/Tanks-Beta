
using UnityEngine;

public class EditorRayForward : MonoBehaviour
{
    private void Awake()
    {
        Destroy(this);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward*100);
    }
}
