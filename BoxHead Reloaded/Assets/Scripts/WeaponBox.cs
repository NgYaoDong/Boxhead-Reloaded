using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* weapons[0] == Pistol
 * weapons[1] == SMG
 * weapons[2] == Shotgun
 * weapons[3] == Grenade
 * weapons[4] == AR
 * weapons[5] == Bazooka
 * weapons[6] == Railgun
 */

public class WeaponBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerWeapon>().weapons[0].AddAmmo(); // make weapon become max ammo
            Destroy(gameObject);
        }
    }
}
