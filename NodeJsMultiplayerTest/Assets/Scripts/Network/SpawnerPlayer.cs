using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPlayer : MonoBehaviour {

    public GameObject listOfCharactersRemote;
    public GameObject listOfCharacter;
    public GameObject bulletPro;
    public GameObject maxinon;
    public Dictionary<string, GameObject> OtherPlayersGameObjects = new Dictionary<string, GameObject>();

    public void SpawnPlayer(string id,int numberCharacter, Vector3 positions,bool ownerOfRoom)
    {
        if (!OtherPlayersGameObjects.ContainsKey(id))
        {
            GameObject newGameObjectPlayer = Instantiate(listOfCharactersRemote.transform.GetChild(numberCharacter).gameObject, positions, Quaternion.identity);
            //ADDED REFERENCE INSTANTIATED PLAYER INDEX OF CHARACTER LIST FROM SELECTION MENU
            newGameObjectPlayer.GetComponent<FollowToClick>().myPlayer = listOfCharacter.transform.GetChild(NetworkRegisterLogin.noCharacter).gameObject;
            newGameObjectPlayer.GetComponent<NetworkEntity>().Id = id;
            Debug.Log("Metoda spawn player are flagul " + ownerOfRoom);
            newGameObjectPlayer.GetComponent<NetworkEntity>().ownerFlag = ownerOfRoom;
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

    public IEnumerator SpawnMinions(List<string> idTowers)
    {
        Debug.Log("SPAWN MINIONS");
        GameObject spawnPointA = GameObject.Find("SpawnPointMinionsA");
        GameObject spawnPointB = GameObject.Find("SpawnPointMinionsB");

        foreach (string towerId in idTowers)
        {
            GameObject maxinon1 = Instantiate(maxinon, spawnPointA.transform.position, Quaternion.identity);
            maxinon1.GetComponent<Target>().targetTransform = spawnPointB.transform;
            //TO DO : SPAWN MINIONS + REMOTE ...
            maxinon1.GetComponent<NetworkEntity>().Id = towerId;

            GameObject maxinon2 = Instantiate(maxinon, spawnPointB.transform.position, Quaternion.identity);
            maxinon2.GetComponent<Target>().targetTransform = spawnPointA.transform;
            yield return new WaitForSeconds(1.0f);
        }
        yield return null;


    }


}
