using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float speed = 50f;

    [Header("Explosion Settings")]
    public GameObject explosion;
    public float explosionDamage = 1f;

    private Vector3 dir;
    private Rigidbody2D rb;
    private Collider2D colli;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        colli = GetComponent<Collider2D>();
    }

    private void Start()
    {
        dir = GameObject.Find("Dir").transform.position;
        transform.position = GameObject.Find("FirePoint").transform.position;
        Vector3 moveDir = (dir - transform.position).normalized;
        rb.velocity = speed * moveDir;
    }

    private void Update()
    {
        if (rb.velocity.sqrMagnitude > 1)
        {
            rb.velocity /= 1.05f;
            transform.Rotate(0, 0, Random.Range(-90, 90));
        }
        else if (rb.velocity.sqrMagnitude <= 1)
        {
            rb.velocity *= 0f;
            StartCoroutine(Explode());
        }
    }

    private void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.CompareTag("Enemy"))
        {
            rb.velocity *= 0f;
            colli.enabled = false;
        }
    }
    
    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.75f);
        Destroy(gameObject);
        explosion.GetComponent<Explosion>().damage = explosionDamage;
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
