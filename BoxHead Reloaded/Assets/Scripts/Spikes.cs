using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip spikesClip;
    [SerializeField] private float damageInterval = 1f;
    private float damageTimer = 0f;
    private bool activated;
    private bool canShoot;

    private void Update()
    {
        if (!canShoot)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                canShoot = true;
                damageTimer = 0f;
            }
        }
    }

    public void TurnOn()
    {
        activated = true;
        animator.SetBool("Spikes", true);
        StartCoroutine(Delay());
    }

    public void TurnOff()
    {
        activated = false;
        animator.SetBool("Spikes", false);
    }

    private void OnTriggerStay2D(Collider2D target)
    {
        if (target.CompareTag("Player") && canShoot && activated) {
            target.gameObject.GetComponent<PlayerHealth>().Attacked(10);
            canShoot = false;
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.6f);
        TurnOff();
        yield return new WaitForSeconds(3);
        TurnOn();
    }
}
