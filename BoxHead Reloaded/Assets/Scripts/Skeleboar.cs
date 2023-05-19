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
    private Rigidbody2D rigidBody;
    private Animator animator;
    private Collider2D colli; 

    private Vector2 movement;

    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<AIDestinationSetter>().target = GameObject.FindWithTag("Player").transform;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colli = GetComponent<Collider2D>();
        health = setHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        AIPath ai = GetComponent<AIPath>();
        animator.SetFloat("Horizontal", ai.velocity.x);
        animator.SetFloat("Vertical", ai.velocity.y);
        animator.SetFloat("Speed", ai.maxSpeed);
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
}
