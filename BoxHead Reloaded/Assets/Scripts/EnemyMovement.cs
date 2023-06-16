using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float setHealth = 100f;
    [SerializeField] private GameObject weaponBox;
    [SerializeField] private Animator blood;
    [SerializeField] private AudioClip enemySpawn;
    [SerializeField] private AudioClip enemyHurt;
    [SerializeField] private AudioClip enemyDie;
    [SerializeField] private AudioClip enemyAttack;

    private float health;
    private float canAttack;
    private Transform destination;
    private Animator animator;
    private Collider2D colli;
    private Renderer rend;
    private int dropChance;

    private void Start()
    {
        animator = GetComponent<Animator>();
        colli = GetComponent<Collider2D>();
        rend = GetComponent<Renderer>();
        health = setHealth;
        dropChance = Random.Range(0, 5);
        AudioSource.PlayClipAtPoint(enemySpawn, transform.position, 0.2f);
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

        if (health <= 0.4 * setHealth) blood.SetBool("LowHealth", true);
    }

    public void Attacked(float damage)
    {
        health -= damage;
        if (health > 0)
        {
            AudioSource.PlayClipAtPoint(enemyHurt, transform.position, 0.4f);
            StartCoroutine(ChangeColor());
        }
        else if (health <= 0f)
        {
            blood.transform.gameObject.SetActive(false);
            StartCoroutine(Death());
        }
    }

    private IEnumerator ChangeColor()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        rend.material.color = Color.white;
    }

    private IEnumerator Death()
    {
        animator.SetTrigger("Death");
        colli.enabled = false;
        AIPath ai = GetComponent<AIPath>();
        ai.maxSpeed = 0f;
        AudioSource.PlayClipAtPoint(enemyDie, transform.position, 0.6f);
        yield return new WaitForSeconds(1.017f);
        if (dropChance == 2) Instantiate(weaponBox, transform.position, Quaternion.identity); 
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (attackDelay <= canAttack)
            {
                collision.gameObject.GetComponent<PlayerHealth>().Attacked(attackDamage);
                if (enemyAttack) AudioSource.PlayClipAtPoint(enemyAttack, transform.position, 0.3f);
                canAttack = 0f;
            }
            else
            {
                canAttack += Time.deltaTime;
            }
        }
    }
}
