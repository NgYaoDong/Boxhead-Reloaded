using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using Unity.Mathematics;

public class CharacterSelection : NetworkBehaviour
{
    [SerializeField] private List<GameObject> characters = new List<GameObject>();
    [SerializeField] private GameObject characterSelPanel;
    [SerializeField] private GameObject canvas;
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!base.IsOwner)
            canvas.SetActive(false);
    }

    public void SpawnRed()
    {
        characterSelPanel.SetActive(false);
        Spawn(0, LocalConnection);
    }

    public void SpawnSilver() 
    {
        characterSelPanel.SetActive(false);
        Spawn(1, LocalConnection);
    }

    [ServerRpc(RequireOwnership = false)]
    void Spawn(int SpawnInd, NetworkConnection conn)
    {
        GameObject player = Instantiate(characters[SpawnInd], SpawnPoint.instance.transform.position, quaternion.identity);
        Spawn(player, conn);
    }
}
