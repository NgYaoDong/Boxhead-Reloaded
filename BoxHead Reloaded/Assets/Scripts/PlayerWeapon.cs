using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class PlayerWeapon : MonoBehaviour
{
    public Weapon currWeapon;
    private float nextTimeOfFire = 0;

    private Transform aimTransform;
    [SerializeField] private Animator topAnimator;

    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().sprite = currWeapon.currWeaponSpr;
    }

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (Time.time >= nextTimeOfFire)
            {
                currWeapon.Shoot();
                nextTimeOfFire = Time.time + 1 / currWeapon.fireRate;
            }
        }

        Aiming();
    }

    private void Aiming()
    {
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 aimLocalScale = Vector3.one;
        if (angle > 90 || angle < -90) 
        {
            aimLocalScale.y = -1f;
        }
        else
        {
            aimLocalScale.y = +1f;
        }
        aimTransform.localScale = aimLocalScale;
        topAnimator.SetFloat("Horizontal", aimDirection.x);
        topAnimator.SetFloat("Vertical", aimDirection.y);
    }
}
