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
    public int damage = 20;

    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, GameObject.Find("FirePoint").transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(_clip, bullet.transform.position, 0.1f);
    }
}
