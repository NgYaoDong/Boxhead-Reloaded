using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float damage = 50f;

    private void Awake()
    {
        Destroy(gameObject, 0.183f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) 
            collision.gameObject.GetComponent<EnemyMovement>().Attacked(damage);
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<PlayerHealth>().Attacked(damage / 2);
    }
}
