using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
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
    }

    private void Update()
    {
        transform.rotation = Healthcamera.transform.rotation;
        transform.position = target.position + offset;
    }
}
