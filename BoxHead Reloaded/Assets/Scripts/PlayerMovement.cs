using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField] private float recoveryTime = 0.5f;

    private Rigidbody2D rigidBody;
    [SerializeField] private Animator bottomAnimator;
    private float timer;
    private Vector2 movement;
    private CinemachineConfiner2D confiner;

    private void Start()
    {
        confiner = GetComponentInChildren<CinemachineConfiner2D>();
        confiner.m_BoundingShape2D = GameObject.Find("CamBounds").GetComponent<Collider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!InGame.isPaused)
        {
            Recovery();
            Animate();
        }
    }
    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + moveSpeed * Time.fixedDeltaTime * movement.normalized);
    }

    private void Animate() 
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        bottomAnimator.SetFloat("Horizontal", movement.x);
        bottomAnimator.SetFloat("Vertical", movement.y);
        bottomAnimator.SetFloat("Speed", movement.sqrMagnitude);

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            bottomAnimator.SetFloat("LastMoveX", Input.GetAxisRaw("Horizontal"));
            bottomAnimator.SetFloat("LastMoveY", Input.GetAxisRaw("Vertical"));
        }
    }

    private void Recovery()
    {
        if (moveSpeed < 5f)
        {
            timer += Time.deltaTime;
            if (timer > recoveryTime)
            {
                timer = 0;
                moveSpeed = 5f;
            }
        }
    }
}
