using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float setHealth = 100f;

    private float health;
    private float canAttack;
    private Transform destination;
    private Animator animator;
    private Collider2D colli;

    private void Start()
    {
        animator = GetComponent<Animator>();
        colli = GetComponent<Collider2D>();
        health = setHealth;
    }

    private void Update()
    {
        if (destination == null)
        {
            destination = GameObject.FindWithTag("Player").transform;
            GetComponent<AIDestinationSetter>().target = destination;
        }

        AIPath ai = GetComponent<AIPath>();
        animator.SetFloat("Horizontal", ai.velocity.normalized.x);
        animator.SetFloat("Vertical", ai.velocity.normalized.y);
        animator.SetFloat("Speed", ai.maxSpeed);

        if (ai.velocity.x == 1 || ai.velocity.x == -1 || ai.velocity.y == 1 || ai.velocity.y == -1)
        {
            animator.SetFloat("LastMoveX", ai.velocity.normalized.x);
            animator.SetFloat("LastMoveY", ai.velocity.normalized.y);
        }
    }

    public void Attacked(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            StartCoroutine(Death());
        }
    }

    private IEnumerator Death()
    {
        animator.SetTrigger("Death");
        colli.enabled = false;
        AIPath ai = GetComponent<AIPath>();
        ai.maxSpeed = 0f;
        yield return new WaitForSeconds(1.017f);
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (attackDelay <= canAttack)
            {
                collision.gameObject.GetComponent<PlayerHealth>().Attacked(attackDamage);
                canAttack = 0f;
            }
            else
            {
                canAttack += Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.CompareTag("Bullet")) {
            Attacked(target.gameObject.GetComponent<Bullet>().damage);
        } else if (target.gameObject.CompareTag("SpreadBullet")) {
            Attacked(target.gameObject.GetComponent<SpreadBullet>().damage);
        }
    }
}
