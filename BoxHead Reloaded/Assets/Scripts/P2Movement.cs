using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;

public class P2Movement : NetworkBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private Collider2D colli;

    Vector2 movement;
    private Camera P2Camera;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            P2Camera = Camera.main;
            P2Camera.transform.position = new Vector2(transform.position.x, transform.position.y);
            P2Camera.transform.SetParent(transform);
        }
        else
        {
            gameObject.GetComponent<P1Movement>().enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colli = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
