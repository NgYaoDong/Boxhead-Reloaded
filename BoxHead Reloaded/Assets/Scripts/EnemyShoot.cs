using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float attackDelay = 2f;
    private Transform aimTransform;
    private float timer;

    private void Awake()
    {
        aimTransform = transform.Find("Enemy FP");
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > attackDelay)
        {
            timer = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        Instantiate(bullet, aimTransform.position, Quaternion.identity);
    }
}
