using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPlayer : MonoBehaviour {

    public GameObject EnemyPrefab;
    public GameObject player;
    public Dictionary<string, GameObject> OtherPlayersGameObjects = new Dictionary<string, GameObject>();

    public void SpawnPlayer(string id, Vector3 positions)
    {
        GameObject newGameObjectPlayer = Instantiate(EnemyPrefab, positions, Quaternion.identity);
        OtherPlayersGameObjects.Add(id, newGameObjectPlayer);
        
    }

    internal void PlayerLeft(string socket_id)
    {
        Destroy(OtherPlayersGameObjects[socket_id]);
        OtherPlayersGameObjects.Remove(socket_id);
    }
}
