using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public string waveName;
    public int noOfNonBoss;
    public int noOfBoss;
    public GameObject[] non_Boss;
    public GameObject[] Boss;
    public float spawnInterval;
    public float waveInterval;
}

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] Wave[] waves;
    [SerializeField] Transform[] spawnPoints;

    private Wave currentWave;
    private int currentWaveNumber;
    private float nextSpawnTime;
    private bool canSpawn = true;
    private bool waitFinish = false;

    private void Update() 
    {
        currentWave = waves[currentWaveNumber];
        SpawnWave();
        
        if (!canSpawn && currentWaveNumber + 1 != waves.Length && waitFinish)
        {
            currentWaveNumber++;
            canSpawn = true;
        }
    }

    private void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time) {
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

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(currentWave.waveInterval);
        waitFinish = true;
    }
}
