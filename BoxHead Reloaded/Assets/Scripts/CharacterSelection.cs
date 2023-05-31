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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }
}
