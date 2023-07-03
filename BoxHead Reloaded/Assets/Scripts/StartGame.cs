using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [SerializeField] private List<GameObject> characters = new();
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetFloat("Music", 1);
        }
        if (!PlayerPrefs.HasKey("SFX"))
        {
            PlayerPrefs.SetFloat("SFX", 1);
        }
        musicSlider.value = PlayerPrefs.GetFloat("Music");
        SFXSlider.value = PlayerPrefs.GetFloat("SFX");
    }

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
