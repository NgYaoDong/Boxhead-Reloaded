using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public int arkzom;
    public int skeleboar;
    public int dopant;
    public int orphenoch;
    public float spawnInterval;
    public float waveInterval;
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Wave[] waves;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private WaveUI waveUI;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private PropsAltar trigger;
    [SerializeField] private bool infinite;

    private Wave currentWave;
    private Coroutine coroutine;
    private int currentWaveNumber;
    private float nextSpawnTime;
    private bool canSpawn = true;
    private bool waitFinish = false;
    private bool startWave = false;

    private void Start()
    {
        waveUI.transform.Find("WaveCompleted").gameObject.SetActive(false);
        if (infinite) {
            waveUI.SetCount(99);
            Wave temp = waves[0];
            waves = new Wave[99];
            waves[0] = temp;
        } else {
            waveUI.SetCount(waves.Length);
        }
        currentWaveNumber = 0;
        canSpawn = true;
        waitFinish = false;
        startWave = false;
        waveUI.transform.Find("WaveReady").gameObject.SetActive(true);
        trigger.finish = false;
        trigger.TurnOn();
    }

    private void Update() 
    {
        if (trigger.end) UI();

        if (trigger.start) {
            StartCoroutine(StartWave());
        }

        if (startWave) {
            currentWave = waves[currentWaveNumber];
            SpawnWave();
            UpdateSpawn();
            SkipSpawn();
        }
    }

    private void SkipSpawn()
    {
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (totalEnemies.Length == 0 && !canSpawn) {
                if (currentWaveNumber + 1 < waves.Length) {
                    StartCoroutine(NextWave());
                } else {
                    trigger.finish = true;
                    trigger.TurnOn();
                    waveUI.transform.Find("WaveCompleted").gameObject.SetActive(true);
                }
            }
    }

    private void UpdateSpawn()
    {
        if (!canSpawn && currentWaveNumber + 1 != waves.Length && waitFinish) {
                currentWaveNumber++;
                waveUI.UpdateWaveCount();
                canSpawn = true;
            }
    }

    private void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time && currentWaveNumber < waves.Length) {
            if (infinite && currentWaveNumber != waves.Length) {
                Wave waveRec = new Wave();
                waveRec.arkzom = 2;
                waveRec.skeleboar = 2;
                waveRec.dopant = 2;
                waveRec.orphenoch = 2;
                waveRec.spawnInterval = 0.5f;
                waveRec.waveInterval = 60f;
                waves[currentWaveNumber + 1] = waveRec;
            }

            GameObject enemy = null;

            if (currentWave.arkzom > 0) {
                enemy = enemies[0];
                currentWave.arkzom--;
            } else if (currentWave.skeleboar > 0) {
                enemy = enemies[1];
                currentWave.skeleboar--;
            } else if (currentWave.dopant > 0) {
                enemy = enemies[2];
                currentWave.dopant--;
            } else if (currentWave.orphenoch > 0) {
                enemy = enemies[3];
                currentWave.orphenoch--;
            } else {
                canSpawn = false;
                waitFinish = false;
                if (currentWaveNumber + 1 < waves.Length) coroutine = StartCoroutine(Wait());
                return;
            }

            nextSpawnTime = Time.time + currentWave.spawnInterval;
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemy, randomPoint.position, Quaternion.identity);
        }  
    }

    private void UI() 
    {
        FindObjectOfType<GameManager>().LevelComplete();
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(currentWave.waveInterval - 3);
        if (currentWaveNumber < waves.Length) StartCoroutine(NextWave());
    }

    private IEnumerator NextWave()
    {
        StopCoroutine(coroutine);
        StartCoroutine(waveUI.StartAnimation());
        yield return new WaitForSeconds(3);
        waitFinish = true;
    }

    private IEnumerator StartWave() 
    {
        trigger.Switch();
        waveUI.transform.Find("WaveReady").gameObject.SetActive(false);
        StartCoroutine(waveUI.StartAnimation());
        trigger.TurnOff();
        yield return new WaitForSeconds(3);
        startWave = true;
    }
}
