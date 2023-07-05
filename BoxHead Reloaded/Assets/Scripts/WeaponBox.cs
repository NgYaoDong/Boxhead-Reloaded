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
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private Weapon[] weapons;

    private void Start()
    {
        Destroy(gameObject, 60f);
    }

    Weapon Reload()
    {
        List<Weapon> possibleReload = new();
        foreach (Weapon weapon in weapons)
        {
            if (weapon.name != "Pistol" && weapon.isActive)
            {
                possibleReload.Add(weapon);
            }
        }
        if (possibleReload.Count > 0)
        {
            Weapon reloadWeapon = possibleReload[Random.Range(0, possibleReload.Count)];
            return reloadWeapon;
        }
        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Weapon reloadWeapon = Reload();
            FindObjectOfType<GameManager>().Reloading(reloadWeapon);
            AudioSource.PlayClipAtPoint(reloadClip, transform.position, PlayerPrefs.GetFloat("SFX"));
            reloadWeapon.AddAmmo();
            collision.transform.Find("Pickup").GetComponent<Animator>().SetTrigger("Pickup");
            Destroy(gameObject);
        }
    }
}
