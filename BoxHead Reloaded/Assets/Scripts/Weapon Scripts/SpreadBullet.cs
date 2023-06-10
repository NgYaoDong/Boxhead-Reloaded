using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadBullet : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    public float damage = 10f;
    [SerializeField] private float timeTillDestroy = 0.2f;
    [SerializeField] private float angleRange = 15f;

    private Rigidbody2D rb;
    private float spreadAngle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spreadAngle = Random.Range(-angleRange, angleRange);
    }

    private void Start()
    {
        float x = transform.position.x - GameObject.FindGameObjectWithTag("Player").transform.position.x;
        float y = transform.position.y - GameObject.FindGameObjectWithTag("Player").transform.position.y;
        float rotateAngle = spreadAngle + (Mathf.Atan2(y, x) * Mathf.Rad2Deg);
        Vector2 MovementDirection = new Vector2(Mathf.Cos(rotateAngle * Mathf.Deg2Rad), Mathf.Sin(rotateAngle * Mathf.Deg2Rad)).normalized;
        rb.velocity = MovementDirection * speed;
    }

    private void Update()
    {
        Destroy(gameObject, timeTillDestroy);
    }

    private void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.CompareTag("Enemy")) target.gameObject.GetComponent<EnemyMovement>().Attacked(damage);
        Destroy(gameObject);
    }
}