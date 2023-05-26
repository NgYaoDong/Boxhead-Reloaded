using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public int noOfNonBoss;
    public int noOfBoss;
    public GameObject[] non_Boss;
    public GameObject[] Boss;
    public float spawnInterval;
    public float waveInterval;
}

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Wave[] waves;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject gameEnd;

    private Wave currentWave;
    private int currentWaveNumber;
    private float nextSpawnTime;
    private bool canSpawn = true;
    private bool waitFinish = false;
    private bool startWave = false;
    private bool finish = false;

    private void Start() 
    {
        StartCoroutine(Hold());
    }

    private void Update() 
    {
        if (startWave) {
            currentWave = waves[currentWaveNumber];
            SpawnWave();
            
            if (!canSpawn && currentWaveNumber + 1 != waves.Length && waitFinish) {
                currentWaveNumber++;
                canSpawn = true;
            }

            GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (totalEnemies.Length == 0 && !canSpawn) {
                if (currentWaveNumber < waves.Length) {
                    StopCoroutine(Wait());
                    StartCoroutine(FastWave());
                } else {
                    finish = true;
                    UI();
                }
            }
        }
    }

    private void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time && currentWaveNumber < waves.Length) {
            GameObject enemy = null;

            if (currentWave.noOfNonBoss > 0) {
                enemy = SpawnNonBoss();
                currentWave.noOfNonBoss--;
            } else if (currentWave.noOfBoss > 0) {
                enemy = SpawnBoss();
                currentWave.noOfBoss--;
            } else {
                canSpawn = false;
                waitFinish = false;
                StartCoroutine(Wait());
                return;
            }

            nextSpawnTime = Time.time + currentWave.spawnInterval;
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemy, randomPoint.position, Quaternion.identity);
        }  

    }

    private GameObject SpawnNonBoss() 
    {
        return currentWave.non_Boss[Random.Range(0, currentWave.non_Boss.Length)];
    }

    private GameObject SpawnBoss()
    {
        return currentWave.Boss[Random.Range(0, currentWave.Boss.Length)];
    }

    private void UI() {
        if (startWave && finish) {
            Instantiate(gameEnd, Camera.main.transform.position, Quaternion.identity);
            startWave = false;
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(currentWave.waveInterval);
        waitFinish = true;
    }

    IEnumerator FastWave()
    {
        yield return new WaitForSeconds(3);
        waitFinish = true;
    }

    IEnumerator Hold() 
    {
        yield return new WaitForSeconds(3);
        startWave = true;
    }
}
