using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float setHealth = 100f;
    [SerializeField] private float healDelay = 3f;
    [SerializeField] private float healAmount = 10f;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject bottom;

    private float health = 0f;
    private float time;
    private Animator animator;

    private void Start() 
    {
        health = setHealth;
        animator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<HealthBar>();
    }

    private void Update() 
    {
        SelfHeal();
    }

    public void Attacked(float damage) 
    {
        health -= damage;
        healthBar.UpdateHealthBar(health, setHealth);
        time = 0f;

        if (health > setHealth) {
            health = setHealth;
        } else if (health <= 0f) {
            health = 0f;
            StartCoroutine(Death());
        }
    }

    private IEnumerator Death() 
    {
        animator.SetBool("Death", true);
        bottom.SetActive(false);
        yield return new WaitForSeconds(0.433f);
        Dead();
    }

    private void Dead() 
    {
        Destroy(gameObject);
    }

    private void SelfHeal() 
    {
        if (health >= setHealth) {
            return;
        } else if (healDelay <= time) {
            health += healAmount;
            time = healDelay - 0.5f;
        } else {
            time += Time.deltaTime;
        }
        healthBar.UpdateHealthBar(health, setHealth);
    }
}