using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    [SerializeField] private List<GameObject> characters = new List<GameObject>();
    [SerializeField] private float minX, maxX, minY, maxY;
    void Start()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        GameObject player = Instantiate(characters[PlayerPrefs.GetInt("SpawnInd")], new Vector2(x, y), Quaternion.identity);
    }
}
