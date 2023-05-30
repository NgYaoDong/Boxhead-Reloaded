using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameEnd : MonoBehaviour
{
    [SerializeField] private GameObject levelComplete;
    [SerializeField] private GameObject gameOver;
    private void Awake()
    {
        Cursor.visible = true;
    }

    public void CheckDead(bool dead)
    {
        if (dead)
        {
            levelComplete.SetActive(false);
            gameOver.SetActive(true);
        }
        else
        {
            levelComplete.SetActive(true);
            gameOver.SetActive(false);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
