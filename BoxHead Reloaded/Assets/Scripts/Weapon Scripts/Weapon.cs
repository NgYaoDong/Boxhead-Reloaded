using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public Sprite currWeaponSpr;
    public GameObject bulletPrefab;
    public AudioClip _clip;

    public float fireRate = 1;
    public int pellets = 1;
    public long ammo = 30;

    public void Shoot()
    {
        for (int i = 0; i < pellets; i++)
        {
            Instantiate(bulletPrefab, GameObject.Find("FirePoint").transform.position, Quaternion.identity);
        }
        if (_clip) AudioSource.PlayClipAtPoint(_clip, GameObject.Find("FirePoint").transform.position, 0.1f);
    }
}