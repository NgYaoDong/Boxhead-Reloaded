using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image healthBar;
    [SerializeField] private Camera Healthcamera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        Healthcamera = Camera.main;
    }

    public void UpdateHealthBar(float currHealth, float maxHealth)
    {
        slider.value = currHealth/maxHealth;
        if (slider.value > 0.5f) healthBar.color = Color.green;
        else if (slider.value <= 0.5f && slider.value > 0.25f) healthBar.color = Color.yellow;
        else healthBar.color = Color.red;
    }

    private void Update()
    {
        transform.SetPositionAndRotation(target.position + offset, Healthcamera.transform.rotation);
    }
}
