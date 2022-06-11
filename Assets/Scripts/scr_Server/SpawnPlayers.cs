using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject par_SpawnedPlayers;
    [SerializeField] private List<Transform> spawnPositions;

    private void Start()
    {
        int index = Random.Range(0, spawnPositions.Count);

        GameObject newPlayer = PhotonNetwork.Instantiate(playerPrefab.name,
                                                          spawnPositions[index].position,
                                                          Quaternion.identity);

        newPlayer.transform.parent = par_SpawnedPlayers.transform;
    }
}