using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerPlayer : MonoBehaviour {

    public GameObject listOfCharactersRemote;
    public GameObject listOfCharacter;
    public GameObject bulletPro;
    public GameObject maxinon;
    public GameObject maxinonRemote;
    public Dictionary<string, GameObject> OtherPlayersGameObjects = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> minionsData = new Dictionary<string, GameObject>();

    void Start()
    {
        InvokeRepeating("UpdateMinions", 20, 20);
        InvokeRepeating("ResetAttackFlagMinions", 20,2);
    }   

    void UpdateMinions()
    {
        var itemsToRemove = minionsData.Where(f => f.Value == null).ToArray();
        foreach (var item in itemsToRemove)
            minionsData.Remove(item.Key);      
    }

    void ResetAttackFlagMinions()
    {
        foreach (string key in minionsData.Keys)
        {
            if(minionsData[key])
                minionsData[key].GetComponent<CreepAi>().isAttacking = false;
        }
    }
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
    public void SpawnBullet(GameObject owner, Vector3 positions,Transform target)
    {
        //TO DO ADD DAMAGE FROM PLAYER CLASS ATTRIBUTES
        GameObject newGameObjectPlayer = Instantiate(bulletPro, positions, Quaternion.identity);       
        newGameObjectPlayer.GetComponent<Bullet>().targetTransform = target;
        newGameObjectPlayer.GetComponent<Bullet>().ownerBullet = owner;
    }

    public IEnumerator SpawnMinions(List<string> idTowers, bool checkOwner)
    {
        Debug.Log("SPAWN MINIONS");
        GameObject spawnPointA = GameObject.Find("SpawnPointMinionsA");
        GameObject spawnPointB = GameObject.Find("SpawnPointMinionsB");
       
        for(int k=0;k<idTowers.Count;k++)
        {

            if (checkOwner)
            {
                GameObject maxinon1 = Instantiate(maxinon, spawnPointA.transform.position, Quaternion.identity);
                maxinon1.GetComponent<Target>().targetTransform = spawnPointB.transform;
                maxinon1.GetComponent<NetworkEntity>().Id = idTowers[k];
                maxinon1.GetComponent<CreepAi>().number = k+1;
                minionsData.Add(idTowers[k], maxinon1);
                ++k;

                GameObject maxinon2 = Instantiate(maxinonRemote, spawnPointB.transform.position, Quaternion.identity);
                maxinon2.GetComponent<Target>().targetTransform = spawnPointA.transform;
                maxinon2.GetComponent<NetworkEntity>().Id = idTowers[k];
                maxinon2.GetComponent<FollowMinions>().myPlayer = listOfCharacter.transform.GetChild(NetworkRegisterLogin.noCharacter).gameObject;
                maxinon2.GetComponent<CreepAi>().number = k + 1;

                minionsData.Add(idTowers[k], maxinon2);

                yield return new WaitForSeconds(1.0f);                
            }
            if (!checkOwner)
            {
                GameObject maxinon1 = Instantiate(maxinonRemote, spawnPointA.transform.position, Quaternion.identity);
                maxinon1.GetComponent<Target>().targetTransform = spawnPointB.transform;
                maxinon1.GetComponent<NetworkEntity>().Id = idTowers[k];
                maxinon1.GetComponent<FollowMinions>().myPlayer = listOfCharacter.transform.GetChild(NetworkRegisterLogin.noCharacter).gameObject;
                maxinon1.GetComponent<CreepAi>().number = k + 1;

                minionsData.Add(idTowers[k], maxinon1);
                ++k;

                GameObject maxinon2 = Instantiate(maxinon, spawnPointB.transform.position, Quaternion.identity);
                maxinon2.GetComponent<Target>().targetTransform = spawnPointA.transform;
                maxinon2.GetComponent<NetworkEntity>().Id = idTowers[k];
                maxinon2.GetComponent<CreepAi>().number = k + 1;

                minionsData.Add(idTowers[k], maxinon2);               

                yield return new WaitForSeconds(1.0f);
            }
            
        }

        CreepAi.numberOrder = 0;

        yield return null;


    }


}
