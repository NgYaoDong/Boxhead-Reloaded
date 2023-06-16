using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI waveCountText;
    [SerializeField] private TextMeshProUGUI waveNumberText;
    [SerializeField] private AudioClip waveClip;

    private int currentWave = 1;
    private int total;

    public void SetCount(int totalWaves)
    {
        total = totalWaves;
        waveNumberText.text = currentWave.ToString();
        waveCountText.text = currentWave.ToString() + "/" + total.ToString();
    }

    public void UpdateWaveCount()
    {
        currentWave++;
        SetCount(total);
    }

    public IEnumerator StartAnimation()
    {
        animator.SetBool("WaveIncoming", true);
        //AudioSource.PlayClipAtPoint(waveClip, Camera.main.transform.position, 0.15f);
        yield return new WaitForSeconds(5.5f);
        animator.SetBool("WaveIncoming", false);
    }
}
