using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float bulletDamage = 20f;
    public float bulletSpeed = 5f;
    [SerializeField] private float timeTillDestroy = 5f;

    [Header("Slow Settings")]
    [SerializeField] private bool slow = false;
    [SerializeField] private float slowedSpeed = 2.5f;

    private GameObject player;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2 (direction.x, direction.y).normalized * bulletSpeed;
        Destroy(gameObject, timeTillDestroy);
    }

    private void OnTriggerEnter2D (Collider2D target)
    {
        if (target.gameObject.CompareTag("Player") || target.gameObject.CompareTag("Wall"))
        {
            if (target.gameObject.CompareTag("Player") && !slow)
                target.gameObject.GetComponent<PlayerHealth>().Attacked(bulletDamage);
            if (target.gameObject.CompareTag("Player") && slow)
            {
                target.gameObject.GetComponent<PlayerMovement>().moveSpeed = slowedSpeed;
                target.gameObject.GetComponent<PlayerMovement>().SlowEffect();
            }
            Destroy(gameObject);
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
