using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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
    public void Shoot()
    {
        for (int i = 0; i < pellets; i++)
        {
            Instantiate(bulletPrefab, GameObject.Find("FirePoint").transform.position, Quaternion.identity);
        }
        AudioSource.PlayClipAtPoint(_clip, GameObject.Find("FirePoint").transform.position, 0.25f);
        currAmmo--;
    }

    public void AddAmmo()
    {
        currAmmo = maxAmmo;
    }
}
