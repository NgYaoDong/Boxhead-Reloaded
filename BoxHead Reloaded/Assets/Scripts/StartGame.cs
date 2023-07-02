using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private List<GameObject> characters = new();

    public void PlayGame()
    {
        PlayerPrefs.SetInt("Mode", 0);
    }

    public void PlayInfinite()
    {
        PlayerPrefs.SetInt("Mode", 1);
    }

    public void SpawnRed()
    {
        Spawn(0);
    }
    
    public void SpawnSilver()
    {
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

    public void Feedback()
    {
        Application.OpenURL("https://forms.gle/DEUkzzpUgy15bGSG9");
    }
}
