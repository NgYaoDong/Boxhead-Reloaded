using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [Header("Characters")]
    [SerializeField] private List<GameObject> characters = new();
    [SerializeField] private List<GameObject> characterSelection = new();

    [Header("Audio Settings")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    private int selectedCharacter = 0;

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

    public void SpawnFang()
    {
        Spawn(2);
    }

    public void Next()
    {
        characterSelection[selectedCharacter].SetActive(false);
        selectedCharacter++;
        if (selectedCharacter == characterSelection.Count)
            selectedCharacter = 0;
        characterSelection[selectedCharacter].SetActive(true);
    }
    
    public void Previous()
    {
        characterSelection[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter == -1)
            selectedCharacter = characterSelection.Count - 1;
        characterSelection[selectedCharacter].SetActive(true);
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
        WeaponSet(weapons, PlayerPrefs.GetInt("Mode"));
    }

    public static void WeaponSet(Weapon[] weapons, int mode)
    {
        if (mode == 0)
        {
            weapons[0].fireRate = 5f; weapons[0].pellets = 1;
            weapons[1].fireRate = 15f; weapons[1].pellets = 1; weapons[1].maxAmmo = 150;
            weapons[2].fireRate = 1.5f; weapons[2].pellets = 5; weapons[2].maxAmmo = 25;
            weapons[3].fireRate = 1f; weapons[3].pellets = 1; weapons[3].maxAmmo = 20;
            weapons[4].fireRate = 7.5f; weapons[4].pellets = 1; weapons[4].maxAmmo = 60;
            weapons[5].fireRate = 0.75f; weapons[5].pellets = 1; weapons[5].maxAmmo = 15;
            weapons[6].fireRate = 30f; weapons[6].pellets = 1; weapons[6].maxAmmo = 300;
            SceneManager.LoadScene("Fresh Beginnings", LoadSceneMode.Single);
        }
        else if (mode == 1)
        {
            weapons[0].fireRate = 5f; weapons[0].pellets = 1;
            weapons[1].fireRate = 15f; weapons[1].pellets = 1; weapons[1].maxAmmo = 300;
            weapons[2].fireRate = 1.5f; weapons[2].pellets = 5; weapons[2].maxAmmo = 50;
            weapons[3].fireRate = 1f; weapons[3].pellets = 1; weapons[3].maxAmmo = 40;
            weapons[4].fireRate = 7.5f; weapons[4].pellets = 1; weapons[4].maxAmmo = 120;
            weapons[5].fireRate = 0.75f; weapons[5].pellets = 1; weapons[5].maxAmmo = 30;
            weapons[6].fireRate = 30f; weapons[6].pellets = 1; weapons[6].maxAmmo = 600;
            SceneManager.LoadScene("Everlasting Abyss", LoadSceneMode.Single);
        }
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
