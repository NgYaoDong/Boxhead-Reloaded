using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float damage = 50f;
    [SerializeField] private AudioClip explode;

    private void Awake()
    {
        Destroy(gameObject, 0.183f);
        AudioSource.PlayClipAtPoint(explode, transform.position, 0.4f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) 
            collision.gameObject.GetComponent<EnemyMovement>().Attacked(damage);
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<PlayerHealth>().Attacked(0.25f * damage);
    }
}
