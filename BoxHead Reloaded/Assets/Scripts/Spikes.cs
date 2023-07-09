using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] Animator animator;
    private bool activated;

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

    private void OnTriggerExit2D(Collider2D target)
    {
        if (target.CompareTag("Player") && activated) {
            target.gameObject.GetComponent<PlayerHealth>().Attacked(10);
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.8f);
        TurnOff();
        yield return new WaitForSeconds(3);
        TurnOn();
    }
}
