using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InGame : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject levelComplete;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject bloodOverlay;
    [SerializeField] private GameObject slowOverlay;
    [SerializeField] private TextMeshProUGUI reloadedWeapon;
    [SerializeField] private Weapon[] weapons;
    public static bool isPaused;

    private void Awake()
    {
        ResumeGame();
        transform.position = Camera.main.transform.position;
        levelComplete.SetActive(false);
        gameOver.SetActive(false);
    }

    public void ReloadText(Weapon reloadWeapon)
    {
        reloadedWeapon.text = "Picked Up " + reloadWeapon.name;
        reloadedWeapon.GetComponent<Animator>().SetTrigger("Reload");
    }

    public void Blood()
    {
        bloodOverlay.GetComponent<Animator>().SetTrigger("Blood");
    }

    public void Slow()
    {
        slowOverlay.GetComponent<Animator>().SetTrigger("Slow");
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
        GunsOff();
        if (PlayerPrefs.GetInt("Mode") == 0) SceneManager.LoadScene("Fresh Beginnings", LoadSceneMode.Single);
        else if (PlayerPrefs.GetInt("Mode") == 1) SceneManager.LoadScene("Everlasting Abyss", LoadSceneMode.Single);
    }

    public void MainMenu()
    {
        GunsOff();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    private void GunsOff()
    {
        foreach (Weapon weapon in weapons) if (weapon.name != "Pistol") weapon.isActive = false;
    }
}
