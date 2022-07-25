
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Material red;
    [SerializeField] private Material blue;
    private MeshRenderer meshRenderer;


    private void OnCollisionEnter(Collision collision)
    {
        meshRenderer.material = red;
        print("OnCollisionEnter");

    }

    private void OnCollisionStay(Collision collision)
    {
        print("OnCollisionStay");
        meshRenderer.material = red;
    }
    private void OnCollisionExit(Collision collision)
    {
        print("OnCollisionExit");
        meshRenderer.material = blue;
    }
    private void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }
}
