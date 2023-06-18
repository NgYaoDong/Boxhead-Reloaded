using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private List<GameObject> characters = new();
    [SerializeField] private GameObject characterSelPanel;
    [SerializeField] private GameObject canvas;

    public void BackToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1, LoadSceneMode.Single);
    }

    public void SpawnRed()
    {
        characterSelPanel.SetActive(false);
        Spawn(0);
    }

    public void SpawnSilver()
    {
        characterSelPanel.SetActive(false);
        Spawn(1);
    }

    void Spawn(int SpawnInd)
    {
        PlayerPrefs.SetInt("SpawnInd", SpawnInd);
        GameObject player = characters[SpawnInd];
        Weapon[] weapons = player.GetComponent<PlayerWeapon>().weapons;
        foreach (Weapon weapon in weapons)
        {
            if (weapon.name == "Pistol") weapon.isActive = true;
            else weapon.isActive = false;
        }
        if (PlayerPrefs.GetInt("Mode") == 0) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        else if (PlayerPrefs.GetInt("Mode") == 1) SceneManager.LoadScene("Everlasting Abyss", LoadSceneMode.Single);
    }
}
