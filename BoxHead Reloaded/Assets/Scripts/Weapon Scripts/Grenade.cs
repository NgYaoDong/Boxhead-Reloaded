using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    [SerializeField] private GameObject explosion;
    public float damage = 10f;
    private Vector3 dir;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
