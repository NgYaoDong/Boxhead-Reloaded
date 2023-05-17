using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using Unity.Mathematics;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private List<Transform> firePoints = new List<Transform>();
    private Transform firePt;

    private Animator animator;

    private Camera PlayerCamera;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            PlayerCamera = Camera.main;
            PlayerCamera.transform.position = new Vector2(transform.position.x, transform.position.y);
            PlayerCamera.transform.SetParent(transform);
        }
        else
        {
            gameObject.GetComponent<PlayerShoot>().enabled = false;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        firePt = firePoints[2];
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") == 1) // FireRight
            firePt = firePoints[0];
        else if (Input.GetAxisRaw("Horizontal") == -1) // FireLeft
            firePt = firePoints[1];
        else if (Input.GetAxisRaw("Vertical") == 1) // FireDown
            firePt = firePoints[2];
        else if (Input.GetAxisRaw("Vertical") == -1) // FireUp
            firePt = firePoints[3];
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Shoot());
        }
    }
    
    IEnumerator Shoot()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(firePt.position, firePt.right);

        if (hitInfo)
        {
            lineRenderer.SetPosition(0, firePt.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            lineRenderer.SetPosition(0, firePt.position);
            lineRenderer.SetPosition(1, firePt.position + firePt.right * 100);
        }

        animator.SetBool("Shoot", true);
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.02f);
        animator.SetBool("Shoot", false);
        lineRenderer.enabled = false;
    }
}
