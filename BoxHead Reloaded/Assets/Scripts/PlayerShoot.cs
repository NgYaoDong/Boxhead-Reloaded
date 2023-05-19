using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private List<Transform> firePoints = new List<Transform>();
    private Transform firePt;

    private Animator animator;   
    private void Start()
    {
        animator = GetComponent<Animator>();
        firePt = firePoints[2];
        lineRenderer.enabled = false;
    }
    // Update is called once per frame
    private void Update()
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
    
    public IEnumerator Shoot()
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
