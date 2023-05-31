using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rigidBody;
    [SerializeField] private Animator bottomAnimator;
    private Collider2D colli;

    private Vector2 movement;
    private CinemachineConfiner2D confiner;

    private void Start()
    {
        confiner = GetComponentInChildren<CinemachineConfiner2D>();
        confiner.m_BoundingShape2D = GameObject.Find("CamBounds").GetComponent<Collider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        colli = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (!InGame.isPaused)
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
    }
    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + moveSpeed * Time.fixedDeltaTime * movement.normalized);
    }

    private IEnumerator OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.CompareTag("Bullet"))
        {
            colli.enabled = false;
            yield return new WaitForSeconds(0.04f);
            colli.enabled = true;
        }
    }
}
