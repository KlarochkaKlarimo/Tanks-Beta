using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveColision : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Collider collider ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionStay(Collision collision)
    {
        Collider myCollider = collision.GetContact(0).thisCollider;
        var isGround = collision.gameObject.layer == LayerMask.NameToLayer("ground");
        if (isGround&& myCollider== collider)
        {
            print("hello");
            _playerMovement.setIsOnGround(true);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Collider myCollider = collision.GetContact(0).thisCollider;
        var isGround = collision.gameObject.layer == LayerMask.NameToLayer("ground");
        if (isGround && myCollider == collider)
        {
            print("hello");
            _playerMovement.setIsOnGround(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        var isGround = collision.gameObject.layer == LayerMask.NameToLayer("ground");
        if (isGround )
        {
            print("bue");
            _playerMovement.setIsOnGround(false);


        }
    }
}
