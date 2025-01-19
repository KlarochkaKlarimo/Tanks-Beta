using UnityEngine;

public class InteractSystem : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                var interactable = hit.rigidbody.transform.GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactable.Interact(gameObject);
                }
            }
        }
    }
}
