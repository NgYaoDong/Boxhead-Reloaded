﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PropsAltar : MonoBehaviour
{
    public List<SpriteRenderer> runes;
    public float lerpSpeed;

    private Color curColor;
    private Color targetColor;
    private Spikes[] spikes;
    private Laser[] lasers;
    public bool start = false;
    public bool end = false;
    public bool finish = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        targetColor = new Color(1, 1, 1, 1);

        if (other.CompareTag("Player"))
        {
            if (!finish) start = true;
            else
            {
                if (SceneManager.GetActiveScene().name == "Cursed Catacombs") end = true;
                else StartCoroutine(NextLvl());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        targetColor = new Color(1, 1, 1, 0);
    }

    private void Update()
    {
        curColor = Color.Lerp(curColor, targetColor, lerpSpeed * Time.deltaTime);

        foreach (var r in runes)
        {
            r.color = curColor;
        }
    }

    public void TurnOn()
    {
        spikes = FindObjectsOfType<Spikes>();
        lasers = FindObjectsOfType<Laser>();
        foreach(var spike in spikes) 
        {
            spike.TurnOff();
        }
        foreach(var laser in lasers)
        {
            laser.begin = false;
        }
        gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        foreach(var spike in spikes) 
        {
            spike.TurnOn();
        }
        foreach (var laser in lasers)
        {
            laser.begin = true;
        }
        gameObject.SetActive(false);
    }

    public void Switch()
    {
        start = false;
    }

    private IEnumerator NextLvl() 
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }
}
