using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InGame : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject levelComplete;
    [SerializeField] private GameObject gameOver;
    public static bool isPaused;
    private void Awake()
    {
        ResumeGame();
        transform.position = Camera.main.transform.position;
        levelComplete.SetActive(false);
        gameOver.SetActive(false);
    }

    public void PauseGame()
    {
        Cursor.visible = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void CheckDead(bool isDead)
    {
        Cursor.visible = true;
        if (isDead) gameOver.SetActive(true);
        else levelComplete.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
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
