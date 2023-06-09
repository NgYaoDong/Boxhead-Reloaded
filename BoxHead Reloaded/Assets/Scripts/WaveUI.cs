using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI waveCountText;
    [SerializeField] private TextMeshProUGUI waveNumberText;
    [SerializeField] private TextMeshProUGUI waveAreaText;
    [SerializeField] private AudioClip waveClip;

    private int currentWave = 1;
    private int total;
    private bool playing = false;

    public void SetCount(int totalWaves)
    {
        SetArea();
        total = totalWaves;
        waveNumberText.text = currentWave.ToString();
        if (total == 99) {
            waveCountText.text = currentWave.ToString() + "/" + "\u221E";
        } else {
            waveCountText.text = currentWave.ToString() + "/" + total.ToString();
        }
    }

    public void SetArea()
    {
        waveAreaText.text = SceneManager.GetActiveScene().name;
    }

    public void UpdateWaveCount()
    {
        currentWave++;
        SetCount(total);
    }

    public IEnumerator StartAnimation()
    {
        animator.SetBool("WaveIncoming", true);
        if (!playing) {
            playing = true;
            AudioSource.PlayClipAtPoint(waveClip, Camera.main.transform.position + new Vector3(0, 0, 10), 0.05f * PlayerPrefs.GetFloat("SFX"));
        }
        yield return new WaitForSeconds(5.5f);
        playing = false;
        animator.SetBool("WaveIncoming", false);
    }
}
