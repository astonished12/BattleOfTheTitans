using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPlayer : MonoBehaviour {

    public GameObject listOfCharactersRemote;
    public GameObject listOfCharacter;
    public GameObject bulletPro;
    public Dictionary<string, GameObject> OtherPlayersGameObjects = new Dictionary<string, GameObject>();

    public void SpawnPlayer(string id,int numberCharacter, Vector3 positions)
    {
        if (!OtherPlayersGameObjects.ContainsKey(id))
        {
            GameObject newGameObjectPlayer = Instantiate(listOfCharactersRemote.transform.GetChild(numberCharacter).gameObject, positions, Quaternion.identity);
            //ADDED REFERENCE INSTANTIATED PLAYER INDEX OF CHARACTER LIST FROM SELECTION MENU
            newGameObjectPlayer.GetComponent<FollowToClick>().myPlayer = listOfCharacter.transform.GetChild(NetworkRegisterLogin.noCharacter).gameObject;
            newGameObjectPlayer.GetComponent<NetworkEntity>().Id = id;
            OtherPlayersGameObjects.Add(id, newGameObjectPlayer);
        }
    }

    internal void PlayerLeft(string socket_id)
    {
        Destroy(OtherPlayersGameObjects[socket_id]);
        OtherPlayersGameObjects.Remove(socket_id);
    }

    internal void AddMyPlayer(string id,GameObject myObject){
        OtherPlayersGameObjects.Add(id, myObject);
    }
    public void SpawnBullet(string id, Vector3 positions,Transform target)
    {
        //TO DO ADD DAMAGE FROM PLAYER CLASS ATTRIBUTES
        GameObject newGameObjectPlayer = Instantiate(bulletPro, positions, Quaternion.identity);
        newGameObjectPlayer.GetComponent<Bullet>().targetTransform = target;
    }


}
