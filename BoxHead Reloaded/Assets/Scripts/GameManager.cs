using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> characters = new();
    [SerializeField] private Vector2 minSpawn, maxSpawn;
    [SerializeField] private GameObject inGame;
    [SerializeField] private GameObject empty;
    private Weapon[] weapons;
    private bool canPause = true;
    void Start()
    {
        if (BGM.instance) BGM.instance.GetComponent<AudioSource>().Stop();
        float x = Random.Range(minSpawn.x, maxSpawn.x);
        float y = Random.Range(minSpawn.y, maxSpawn.y);
        GameObject player = Instantiate(characters[PlayerPrefs.GetInt("SpawnInd")], new Vector2(x, y), Quaternion.identity);
        weapons = player.GetComponent<PlayerWeapon>().weapons;
        if (PlayerPrefs.GetInt("Mode") == 1) InfiniteMode();
        else
        {
            CampaignMode();
            CheckActive();
        }
    }

    private void InfiniteMode()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.isActive = true;
            if (weapon.name != "Pistol") weapon.currAmmo = 0;
        }
        weapons[1].maxAmmo = 300;
        weapons[2].maxAmmo = 50;
        weapons[3].maxAmmo = 40;
        weapons[4].maxAmmo = 120;
        weapons[5].maxAmmo = 30;
        weapons[6].maxAmmo = 600;
    }

    private void CampaignMode()
    {
        weapons[1].maxAmmo = 150;
        weapons[2].maxAmmo = 25;
        weapons[3].maxAmmo = 20;
        weapons[4].maxAmmo = 60;
        weapons[5].maxAmmo = 15;
        weapons[6].maxAmmo = 300;
        foreach (Weapon weapon in weapons)
        {
            if (weapon.isActive) weapon.AddAmmo();
            else weapon.currAmmo = 0;
        }
    }

    private void CheckActive()
    {
        if (weapons[5].isActive) weapons[6].isActive = true;
        else if (weapons[3].isActive)
        {
            weapons[4].isActive = true;
            weapons[5].isActive = true;
        }
        else if (weapons[1].isActive)
        {
            weapons[2].isActive = true;
            weapons[3].isActive = true;
        }
        else weapons[1].isActive = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPause) 
        {
            if (InGame.isPaused) inGame.GetComponent<InGame>().ResumeGame();
            else inGame.GetComponent<InGame>().PauseGame();
        }
    }

    public void Reloading(Weapon reloadWeapon)
    {
        inGame.GetComponent<InGame>().ReloadText(reloadWeapon);
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
