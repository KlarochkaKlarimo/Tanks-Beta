using UnityEngine;

public class MoveColision : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Collider colliderLeft ;
    [SerializeField] private Collider colliderRight ;
    private bool left = false;
    private bool right = false;
    private void OnCollisionStay(Collision collision)
    {
        var d = collision.contacts;
        foreach (ContactPoint contact in d)
        {
            if (contact.thisCollider == colliderLeft)
            {
                left=true;
            }
            if (contact.thisCollider == colliderRight)
            {
                right = true;
            }
        }
        var isGround = collision.gameObject.layer == LayerMask.NameToLayer("ground");
        if (isGround && left&& right)
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
            left = false;
             right = false;
                print("bue");
            _playerMovement.setIsOnGround(false);
        }
    }
}
