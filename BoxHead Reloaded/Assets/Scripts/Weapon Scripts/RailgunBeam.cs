using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunBeam : MonoBehaviour
{
    public float damage = 20f;
    private LineRenderer beam;
    private Vector2 dir;

    private void Awake()
    {
        beam = GetComponent<LineRenderer>();
        dir = GameObject.Find("Dir").transform.position;
    }

    private void Start()
    {
        RaycastHit2D hitInfo =  Physics2D.Raycast(transform.position, dir, 50, LayerMask.GetMask("Wall"));
        beam.SetPosition(0, GameObject.Find("Weapon").transform.position);
        beam.SetPosition(1, hitInfo.point);

        RaycastHit2D[] enemyHitInfo =  Physics2D.RaycastAll(transform.position, dir, hitInfo.distance, LayerMask.GetMask("Enemy"));
        foreach (RaycastHit2D enemyHit in enemyHitInfo)
        {
            if (enemyHit && enemyHit.collider.CompareTag("Enemy"))
            {
                enemyHit.collider.GetComponent<EnemyMovement>().Attacked(damage);
            }
        }
        Destroy(gameObject, 0.04f);
    }
}
