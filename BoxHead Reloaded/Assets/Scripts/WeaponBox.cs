using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<P1Movement>() || collision.GetComponent<P2Movement>()) this.gameObject.SetActive(false);
    }
}
