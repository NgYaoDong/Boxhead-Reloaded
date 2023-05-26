using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = true;
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
