using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Skeleboar : MonoBehaviour
{
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackDelay = 0.5f;
    [SerializeField] private float setHealth = 100f;

    private float health;
    private float canAttack;
    private Transform destination;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private Collider2D colli; 

    private Vector2 movement;

    // Start is called before the first frame update
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colli = GetComponent<Collider2D>();
        health = setHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        if (destination == null) 
        {
            destination = GameObject.FindWithTag("Player").transform;
            GetComponent<AIDestinationSetter>().target = destination;
        }
        
        AIPath ai = GetComponent<AIPath>();
        animator.SetFloat("Horizontal", ai.velocity.x);
        animator.SetFloat("Vertical", ai.velocity.y);
        animator.SetFloat("Speed", ai.maxSpeed);
    }

    public void Attacked(float damage) 
    {
        health -= damage;

        if (health <= 0f) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Player") {
            if (attackDelay <= canAttack) {
                collision.gameObject.GetComponent<PlayerHealth>().Attacked(attackDamage);
                canAttack = 0f;
            } else {
                canAttack += Time.deltaTime;
            }
        } 
    }

    private void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "Bullet") {
            Attacked(target.gameObject.GetComponent<PistolBullet>().damage);
        }
    }
}
