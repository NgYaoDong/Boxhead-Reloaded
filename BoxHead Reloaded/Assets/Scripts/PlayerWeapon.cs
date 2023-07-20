using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using TMPro;

public class PlayerWeapon : MonoBehaviour
{
    public int weaponNum = 0;
    public Weapon[] weapons;
    public Weapon currWeapon;
    [SerializeField] private AudioClip ammoEmpty;
    private float nextTimeOfFire = 0;

    private Transform aimTransform;
    private Transform crosshairTransform;
    private Transform weaponTransform;
    [SerializeField] private Animator topAnimator;
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private TextMeshPro ammoDisplay;

    [SerializeField] private GameObject weaponBox;

    private int clickCount = 0;

    private void Awake()
    {
        Cursor.visible = false;
        aimTransform = transform.Find("Aim");
        crosshairTransform = transform.Find("Crosshair");
        weaponTransform = aimTransform.Find("Weapon");
        currWeapon = weapons[weaponNum];
        weaponTransform.GetComponent<SpriteRenderer>().sprite = currWeapon.currWeaponSpr;
        Display();
    }

    private void Update()
    {
        if (!InGame.isPaused)
        {
            Aiming();

            if (Input.GetButton("Fire1")) Firing();

            if (Input.GetMouseButtonDown(1) || clickCount == 5) Switch();

            NumberSwitch();

            //if (Input.GetKeyDown(KeyCode.T)) Instantiate(weaponBox, transform.position, Quaternion.identity);

            Display();
        }
    }

    private void Display()
    {
        if (currWeapon.name == "Pistol") ammoDisplay.text = currWeapon.name + ": \u221E";
        else ammoDisplay.text = currWeapon.name + ": " + currWeapon.currAmmo.ToString();
    }    

    private void Firing()
    {
        if (currWeapon.currAmmo >= 0 && Time.time >= nextTimeOfFire)
        {
            if (currWeapon.currAmmo > 0)
            {
                clickCount = 0;
                currWeapon.Shoot();
                nextTimeOfFire = Time.time + 1 / currWeapon.fireRate;

                if (weaponNum == 3 || weaponNum == 6) return;
                gunAnimator.SetTrigger("Shoot");
            }
            else if (currWeapon.currAmmo == 0)
            {
                AudioSource.PlayClipAtPoint(ammoEmpty, GameObject.Find("FirePoint").transform.position, PlayerPrefs.GetFloat("SFX"));
                nextTimeOfFire = Time.time + 1 / currWeapon.fireRate;
                clickCount++;
            }
        }
    }

    private void NumberSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (weapons[0].currAmmo > 0 && weapons[0].isActive)
            {
                weaponNum = 0;
                currWeapon = weapons[weaponNum];
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (weapons[1].currAmmo > 0 && weapons[1].isActive)
            {
                weaponNum = 1;
                currWeapon = weapons[weaponNum];
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (weapons[2].currAmmo > 0 && weapons[2].isActive)
            {
                weaponNum = 2;
                currWeapon = weapons[weaponNum];
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (weapons[3].currAmmo > 0 && weapons[3].isActive)
            {
                weaponNum = 3;
                currWeapon = weapons[weaponNum];
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (weapons[4].currAmmo > 0 && weapons[4].isActive)
            {
                weaponNum = 4;
                currWeapon = weapons[weaponNum];
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (weapons[5].currAmmo > 0 && weapons[5].isActive)
            {
                weaponNum = 5;
                currWeapon = weapons[weaponNum];
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (weapons[6].currAmmo > 0 && weapons[6].isActive)
            {
                weaponNum = 6;
                currWeapon = weapons[weaponNum];
            }
        }
        weaponTransform.GetComponent<SpriteRenderer>().sprite = currWeapon.currWeaponSpr;
    }

    private void Switch()
    {
        do
        {
            weaponNum++;
            if (weaponNum + 1 > weapons.Length) weaponNum = 0;
            currWeapon = weapons[weaponNum];
        } while (currWeapon.currAmmo == 0 || !currWeapon.isActive);

        clickCount = 0;
        weaponTransform.GetComponent<SpriteRenderer>().sprite = currWeapon.currWeaponSpr;
    }

    private void Aiming()
    {
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        crosshairTransform.position = mousePosition;
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 aimLocalScale = Vector3.one;
        
        if (angle > 90 || angle < -90) aimLocalScale.y = -1f;
        else aimLocalScale.y = +1f;

        aimTransform.localScale = aimLocalScale;
        topAnimator.SetFloat("Horizontal", aimDirection.x);
        topAnimator.SetFloat("Vertical", aimDirection.y);
    }
}
