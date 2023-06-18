using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private AudioClip spawnClip;
    public float moveSpeed = 5f;
    [SerializeField] private float recoveryTime = 0.5f;
    [SerializeField] private AudioClip slowClip;

    private Rigidbody2D rigidBody;
    [SerializeField] private Animator bottomAnimator;
    private float timer;
    private Vector2 movement;
    private CinemachineConfiner2D confiner;
    private Renderer rend;

    private GameObject checkpoint;
    private Transform arrow;

    private void Start()
    {
        confiner = GetComponentInChildren<CinemachineConfiner2D>();
        confiner.m_BoundingShape2D = GameObject.Find("CamBounds").GetComponent<Collider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        rend = GetComponent<Renderer>();
        AudioSource.PlayClipAtPoint(spawnClip, transform.position);
        checkpoint = GameObject.Find("Checkpoint");
        arrow = transform.Find("Arrow");
    }

    private void Update()
    {
        if (!InGame.isPaused)
        {
            Recovery();
            Animate();
            Arrow();
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
            Color32 lightBlue = new(0, 169, 255, 255);
            rend.material.color = lightBlue;
            bottomAnimator.GetComponentInParent<Renderer>().material.color = lightBlue;
            if (timer > recoveryTime)
            {
                timer = 0;
                moveSpeed = 5f;
                rend.material.color = Color.white;
                bottomAnimator.GetComponentInParent<Renderer>().material.color = Color.white;
            }
        }
    }

    public void SlowEffect()
    {
        FindObjectOfType<InGame>().Slow();
        AudioSource.PlayClipAtPoint(slowClip, transform.position, 0.4f);
    }

    private void Arrow()
    {
        Vector2 dir = checkpoint.transform.position - arrow.position;
        if (checkpoint.activeSelf && dir.sqrMagnitude > 20f)
        {
            arrow.gameObject.SetActive(true);
            float angle = Mathf.Atan2(dir.y + 1.2f, dir.x) * Mathf.Rad2Deg;
            arrow.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else arrow.gameObject.SetActive(false);
    }
}
