using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class EmptyPrefab : MonoBehaviour
{
    private CinemachineConfiner2D confiner;
    private void Awake()
    {
        confiner = GetComponentInChildren<CinemachineConfiner2D>();
        confiner.m_BoundingShape2D = GameObject.Find("CamBounds").GetComponent<Collider2D>();
    }
}
