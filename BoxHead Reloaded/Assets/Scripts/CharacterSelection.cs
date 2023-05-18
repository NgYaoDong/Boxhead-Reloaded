using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private List<GameObject> characters = new List<GameObject>();
    [SerializeField] private GameObject characterSelPanel;
    [SerializeField] private GameObject canvas;

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
        GameObject player = Instantiate(characters[SpawnInd], SpawnPoint.instance.transform.position, quaternion.identity);
        //Spawn(player);
    }
}
