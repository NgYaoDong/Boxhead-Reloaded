using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class PlayerWeapon : MonoBehaviour
{
    public Weapon currWeapon;
    private float nextTimeOfFire = 0;

    private Transform aimTransform;
    private Transform crosshairTransform;
    [SerializeField] private Animator topAnimator;
    [SerializeField] private Animator gunAnimator;

    private void Awake()
    {
        Cursor.visible = false;
        aimTransform = transform.Find("Aim");
        crosshairTransform = transform.Find("Crosshair");
        aimTransform.Find("Weapon").GetComponent<SpriteRenderer>().sprite = currWeapon.currWeaponSpr;
    }

    private void Update()
    {
        if (!InGame.isPaused)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeOfFire)
            {
                currWeapon.Shoot();
                gunAnimator.SetTrigger("Shoot");
                nextTimeOfFire = Time.time + 1 / currWeapon.fireRate;
            }

            Aiming();
        }
    }

    private void Aiming()
    {
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        crosshairTransform.position = mousePosition;
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
