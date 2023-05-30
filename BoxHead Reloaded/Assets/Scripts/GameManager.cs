using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> characters = new();
    [SerializeField] private float minX, maxX, minY, maxY;
    [SerializeField] private GameObject gameEnd;
    [SerializeField] private GameObject empty;
    void Start()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        GameObject player = Instantiate(characters[PlayerPrefs.GetInt("SpawnInd")], new Vector2(x, y), Quaternion.identity);
    }

    public void GameOver()
    {
        gameEnd.GetComponentInChildren<GameEnd>().CheckDead(true);
        Instantiate(gameEnd, Camera.main.transform.position, Quaternion.identity);
        CinemachineConfiner2D confiner = empty.GetComponentInChildren<CinemachineConfiner2D>();
        confiner.m_BoundingShape2D = GameObject.Find("CamBounds").GetComponent<Collider2D>();
        Instantiate(empty, Camera.main.transform.position, Quaternion.identity);
    }

    public void LevelComplete()
    {
        gameEnd.GetComponentInChildren<GameEnd>().CheckDead(false);
        Instantiate(gameEnd, Camera.main.transform.position, Quaternion.identity);
    }
}
