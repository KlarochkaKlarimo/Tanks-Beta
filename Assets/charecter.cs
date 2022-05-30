using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charecter : MonoBehaviour
{

    private Animator animator;
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.back, Time.deltaTime*speed);
            animator.SetInteger("movement type", 1);
        }
        else
        {
            animator.SetInteger("movement type", 0);
        }

    }
}
