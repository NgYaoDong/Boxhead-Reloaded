using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float defDistanceRay = 100;
    [SerializeField] private Transform laserFirePoint;
    [SerializeField] private LineRenderer m_lineRenderer;
    [SerializeField] private AudioClip laserClip;

    [Header("Damage Settings")]
    [SerializeField] private float damageInterval = 3f;
    [SerializeField] private float damage = 10f;

    private bool canShoot = true;
    private float damageTimer = 0f;
    public bool turnOn = false;

    private void Update()
    {
        if (turnOn)
        {
            ShootLaser();
        }

        if (!canShoot)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                canShoot = true;
                damageTimer = 0f;
            }
        }
    }

    public void ShootLaser()
    {
        if (Physics2D.Raycast(transform.position, transform.up, defDistanceRay, LayerMask.GetMask("Player")))
        {
            RaycastHit2D playerHit = Physics2D.Raycast(laserFirePoint.position, transform.up, defDistanceRay, LayerMask.GetMask("Player"));
            Draw2DRay(laserFirePoint.position, playerHit.point);
            if (playerHit && canShoot)
            {
                canShoot = false;
                playerHit.transform.GetComponent<PlayerHealth>().Attacked(damage);
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(laserFirePoint.position, transform.up, defDistanceRay, LayerMask.GetMask("Wall"));
            Draw2DRay(laserFirePoint.position, hit.point);
        }
    }

    public void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, endPos);
    }
}
