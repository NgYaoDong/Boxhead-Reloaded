using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    public float damage = 10f;
    private Vector2 dir;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void DestroyOffScreen()
    {
        Vector2 screenPos = cam.WorldToScreenPoint(transform.position);
        if (screenPos.x < 0 || screenPos.x > cam.pixelWidth || screenPos.y < 0 || screenPos.y > cam.pixelHeight) 
            Destroy(gameObject);
    }

    private void Start()
    {
        dir = GameObject.Find("Dir").transform.position;
        transform.position = GameObject.Find("FirePoint").transform.position;
        transform.eulerAngles = new Vector3(0, 0, GameObject.Find("Aim").transform.eulerAngles.z);
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, dir, speed * Time.deltaTime);
        DestroyOffScreen();
    }

    private void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.CompareTag("Player")) return;
        Destroy(gameObject);
    }
}
