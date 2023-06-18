using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public double arkzom;
    public double skeleboar;
    public double dopant;
    public double orphenoch;
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
    private bool waveCreated = false;
    private double scale1 = 1.5;
    private double scale2 = 1.4;
    private double scale3 = 1.3;

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
            if (infinite && currentWaveNumber != waves.Length && !waveCreated) {
                Wave waveRec = new Wave();
                waveRec.arkzom = currentWave.arkzom * scale3;
                waveRec.skeleboar = currentWave.skeleboar * scale2;
                waveRec.dopant = currentWaveNumber > 1 ? currentWave.dopant * scale2 : 0.9;
                waveRec.orphenoch = currentWaveNumber > 3 ? currentWave.orphenoch * scale1 : 0.9;
                waveRec.spawnInterval = 0.75f;
                waveRec.waveInterval = 120f;
                waves[currentWaveNumber + 1] = waveRec;
                waveCreated = true;
                if (waveRec.arkzom > 50) scale3 = 1.2;
                if (waveRec.arkzom > 100) scale3 = 1.1;
                if (waveRec.arkzom > 300) scale3 = 1.05;
                if (waveRec.skeleboar > 30) scale2 = 1.2;
                if (waveRec.skeleboar > 75) scale2 = 1.1;
                if (waveRec.skeleboar > 150) scale2 = 1.05;
                if (waveRec.orphenoch > 30) scale1 = 1.3;
                if (waveRec.orphenoch > 75) scale1 = 1.1;
                if (waveRec.orphenoch > 150) scale1 = 1.05;
            }

            GameObject enemy = null;

            if (currentWave.arkzom >= 1) {
                enemy = enemies[0];
                currentWave.arkzom--;
            } else if (currentWave.skeleboar >= 1) {
                enemy = enemies[1];
                currentWave.skeleboar--;
            } else if (currentWave.dopant >= 1) {
                enemy = enemies[2];
                currentWave.dopant--;
            } else if (currentWave.orphenoch >= 1) {
                enemy = enemies[3];
                currentWave.orphenoch--;
            } else {
                canSpawn = false;
                waitFinish = false;
                waveCreated = false;
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
