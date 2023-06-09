using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> characters = new();
    [SerializeField] private float minX, maxX, minY, maxY;
    [SerializeField] private GameObject inGame;
    [SerializeField] private GameObject empty;
    private bool canPause = true;
    void Start()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        GameObject player = Instantiate(characters[PlayerPrefs.GetInt("SpawnInd")], new Vector2(x, y), Quaternion.identity);
        Weapon[] weapons = player.GetComponent<PlayerWeapon>().weapons;
        foreach (Weapon weapon in weapons)
            weapon.AddAmmo();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPause) 
        {
            if (InGame.isPaused) inGame.GetComponent<InGame>().ResumeGame();
            else inGame.GetComponent<InGame>().PauseGame();
        }
    }

    public void GameOver()
    {
        canPause = false;
        inGame.GetComponent<InGame>().CheckDead(true);
        CinemachineConfiner2D confiner = empty.GetComponentInChildren<CinemachineConfiner2D>();
        confiner.m_BoundingShape2D = GameObject.Find("CamBounds").GetComponent<Collider2D>();
        Instantiate(empty, Camera.main.transform.position, Quaternion.identity);
    }

    public void LevelComplete()
    {
        canPause = false;
        inGame.GetComponent<InGame>().CheckDead(false);
    }
}
