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

    private void Reload()
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
            reloadWeapon.AddAmmo();
            FindObjectOfType<GameManager>().Reloading(reloadWeapon, 0);
        }
    }

    private void FastFire()
    {
        List<Weapon> possibleFF = new();
        foreach (Weapon weapon in weapons)
        {
            if (weapon.isActive && !weapon.fastFire)
            {
                possibleFF.Add(weapon);
            }
        }
        if (possibleFF.Count > 0)
        {
            Weapon FFWeapon = possibleFF[Random.Range(0, possibleFF.Count)];
            FFWeapon.FastFireOn();
            FindObjectOfType<GameManager>().Reloading(FFWeapon, 1);
        }
        else
            Reload();
    }

    private void DoubleDamage()
    {
        List<Weapon> possibleDD = new();
        foreach (Weapon weapon in weapons)
        {
            if (weapon.isActive && !weapon.damageUp)
            {
                possibleDD.Add(weapon);
            }
        }
        if (possibleDD.Count > 0)
        {
            Weapon DDWeapon = possibleDD[Random.Range(0, possibleDD.Count)];
            DDWeapon.DoubleDamageOn();
            FindObjectOfType<GameManager>().Reloading(DDWeapon, 2);
        }
        else
            Reload();
    }

    private void DoubleAmmo()
    {
        List<Weapon> possibleDA = new();
        foreach (Weapon weapon in weapons)
        {
            if (weapon.isActive && !weapon.ammoUp && weapon.name != "Pistol")
            {
                possibleDA.Add(weapon);
            }
        }
        if (possibleDA.Count > 0)
        {
            Weapon DAWeapon = possibleDA[Random.Range(0, possibleDA.Count)];
            DAWeapon.DoubleAmmoOn();
            FindObjectOfType<GameManager>().Reloading(DAWeapon, 3);
        }
        else
            Reload();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int weaponUpgrade = Random.Range(0, 5);
            if (weaponUpgrade == 0 || weaponUpgrade == 1)
                Reload();
            else if (weaponUpgrade == 2)
                FastFire();
            else if (weaponUpgrade == 3)
                DoubleDamage();
            else if (weaponUpgrade == 4)
                DoubleAmmo();
            AudioSource.PlayClipAtPoint(reloadClip, transform.position, PlayerPrefs.GetFloat("SFX"));
            collision.transform.Find("Pickup").GetComponent<Animator>().SetTrigger("Pickup");
            Destroy(gameObject);
        }
    }
}