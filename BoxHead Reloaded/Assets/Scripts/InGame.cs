using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGame : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject levelComplete;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject bloodOverlay;
    [SerializeField] private GameObject slowOverlay;
    [SerializeField] private GameObject smokeOverlay;
    [SerializeField] private TextMeshProUGUI reloadedWeapon;
    [SerializeField] private Weapon[] weapons;
    public static bool isPaused;

    [Header("Weapon UI")]
    [SerializeField] private TextMeshProUGUI[] weaponList;

    [Header("Ability UI")]
    private int ability;
    [SerializeField] private Image[] abilityCD;
    private bool isCD = false;
    private float CDTime = 0f;
    private float CDTimer = 0f;
    private bool isAbility = false;
    private float abilityTime = 0f;
    private float abilityTimer = 0f;

    [Header("Sound Settings")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    private void Awake()
    {
        ResumeGame();
        transform.position = Camera.main.transform.position;
        levelComplete.SetActive(false);
        gameOver.SetActive(false);
    }

    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("Music");
        SFXSlider.value = PlayerPrefs.GetFloat("SFX");
        ability = PlayerPrefs.GetInt("SpawnInd");
        abilityCD[ability].transform.parent.gameObject.SetActive(true);
        abilityCD[ability].fillAmount = 0f;
    }

    private void Update()
    {
        if (!isPaused)
        {
            weaponList[1].text = weapons[1].currAmmo.ToString();
            weaponList[2].text = weapons[2].currAmmo.ToString();
            weaponList[3].text = weapons[3].currAmmo.ToString();
            weaponList[4].text = weapons[4].currAmmo.ToString();
            weaponList[5].text = weapons[5].currAmmo.ToString();
            weaponList[6].text = weapons[6].currAmmo.ToString();
            if (isCD) ApplyCD();
            if (isAbility) ApplyAbility();
        }
    }

    private void ApplyCD()
    {
        CDTimer -= Time.deltaTime;

        if (CDTimer <= 0f) 
        {
            isCD = false;
            abilityCD[ability].fillAmount = 0f;
        }
        else
        {
            abilityCD[ability].fillAmount = CDTimer / CDTime;
        }
    }

    private void ApplyAbility()
    {
        abilityTimer += Time.deltaTime;

        if (abilityTimer >= abilityTime) 
        {
            isAbility = false;
            abilityCD[ability].fillAmount = 1f;
            abilityTimer = 0f;
        }
        else
        {
            abilityCD[ability].fillAmount = abilityTimer / abilityTime;
        }
    }

    public void UseAbility(float CD)
    {
        if (isCD) return;
        isCD = true;
        CDTime = CD;
        CDTimer = CD;
    }

    public void InAbility(float abilityDuration)
    {
        if (isAbility) return;
        isAbility = true;
        abilityTime = abilityDuration;
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

    public void Smoke()
    {
        smokeOverlay.GetComponent<Animator>().SetTrigger("Smoke");
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

    public void SetMusic(float sliderValue)
    {
        mixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("Music", sliderValue);
    }

    public void SetSFX(float sliderValue)
    {
        mixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SFX", sliderValue);
    }
}
