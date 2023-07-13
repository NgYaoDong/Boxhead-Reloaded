using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public Sprite currWeaponSpr;
    public GameObject bulletPrefab;
    public AudioClip _clip;

    public float fireRate = 1;
    public int pellets = 1;
    public float currAmmo = Mathf.Infinity;
    public float maxAmmo = Mathf.Infinity;

    public bool isActive;

    [Header("Default Settings")]
    public float defaultFireRate = 1;
    public float defaultDamage = 1;
    public float defaultCampaignAmmo = 1;
    public float defaultInfiniteAmmo = 1;

    [Header("Fast Fire Settings")]
    public float fastFireRate = 1;
    public bool fastFire = false;

    [Header("Damage Up Settings")]
    public float increasedDamage = 1;
    public bool damageUp = false;

    [Header("Ammo Up Settings")]
    public float campaignAmmoUp = 1;
    public float infiniteAmmoUp = 1;
    public bool ammoUp = false;

    public void Shoot()
    {
        for (int i = 0; i < pellets; i++)
        {
            Instantiate(bulletPrefab, GameObject.Find("FirePoint").transform.position, Quaternion.identity);
        }
        AudioSource.PlayClipAtPoint(_clip, GameObject.Find("FirePoint").transform.position, 0.25f * PlayerPrefs.GetFloat("SFX"));
        currAmmo--;
    }

    public void Default(int mode)
    {
        fastFire = false;
        damageUp = false;
        ammoUp = false;
        fireRate = defaultFireRate;
        if (mode == 0)
            maxAmmo = defaultCampaignAmmo;
        else if (mode == 1)
            maxAmmo = defaultInfiniteAmmo;
        if (bulletPrefab.GetComponent<Bullet>())
        {
            if (bulletPrefab.GetComponent<Bullet>().explosion) //Bazooka
                bulletPrefab.GetComponent<Bullet>().explosion.GetComponent<Explosion>().damage = defaultDamage;
            else //Pistol, SMG, AR
                bulletPrefab.GetComponent<Bullet>().damage = defaultDamage;
        }
        else if (bulletPrefab.GetComponent<SpreadBullet>()) //Shotgun
            bulletPrefab.GetComponent<SpreadBullet>().damage = defaultDamage;
        else if (bulletPrefab.GetComponent<RailgunBeam>()) //Railgun
            bulletPrefab.GetComponent<RailgunBeam>().damage = defaultDamage;
        else if (bulletPrefab.GetComponent<Grenade>()) //Grenade
            bulletPrefab.GetComponent<Grenade>().explosion.GetComponent<Explosion>().damage = defaultDamage;
    }

    public void AddAmmo()
    {
        currAmmo = maxAmmo;
    }

    public void FastFireOn()
    {
        fastFire = true;
        fireRate = fastFireRate;
        AddAmmo();
    }

    public void DoubleDamageOn()
    {
        damageUp = true;
        if (bulletPrefab.GetComponent<Bullet>())
        {
            if (bulletPrefab.GetComponent<Bullet>().explosion) //Bazooka
                bulletPrefab.GetComponent<Bullet>().explosion.GetComponent<Explosion>().damage = increasedDamage;
            else //Pistol, SMG, AR
                bulletPrefab.GetComponent<Bullet>().damage = increasedDamage;
        }
        else if (bulletPrefab.GetComponent<SpreadBullet>()) //Shotgun
            bulletPrefab.GetComponent<SpreadBullet>().damage = increasedDamage;
        else if (bulletPrefab.GetComponent<RailgunBeam>()) //Railgun
            bulletPrefab.GetComponent<RailgunBeam>().damage = increasedDamage;
        else if (bulletPrefab.GetComponent<Grenade>()) //Grenade
            bulletPrefab.GetComponent<Grenade>().explosion.GetComponent<Explosion>().damage = increasedDamage;
        AddAmmo();
    }

    public void DoubleAmmoOn()
    {
        ammoUp = true;
        if (PlayerPrefs.GetInt("Mode") == 0)
            maxAmmo = campaignAmmoUp;
        else if (PlayerPrefs.GetInt("Mode") == 1)
            maxAmmo = infiniteAmmoUp;
        AddAmmo();
    }
}
