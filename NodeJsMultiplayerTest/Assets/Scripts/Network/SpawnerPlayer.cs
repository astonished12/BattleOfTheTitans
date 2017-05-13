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
    public GameObject nexusA;
    public GameObject nexusARemote;
    public GameObject nexusB;
    public GameObject nexusBRemote;
    public GameObject towerA;
    public GameObject towerARemote;
    public GameObject towerB;
    public GameObject towerBRemote;
    public GameObject minionsSpawnPointsA;
    public GameObject minionsSpawnPointsB;
    public Dictionary<string, GameObject> OtherPlayersGameObjects = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> minionsData = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> towersData = new Dictionary<string, GameObject>();

    void Start()
    {
        InvokeRepeating("UpdateMinions", 20, 20);
    }   

    void UpdateMinions()
    {
        var itemsToRemove = minionsData.Where(f => f.Value == null).ToArray();
        foreach (var item in itemsToRemove)
            minionsData.Remove(item.Key);      
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

    internal void SpawnTowers(JSONObject idTowers,bool checkOwner)
    {
        Debug.Log("Spawn TOWERS");
        GameObject positionsA = GameObject.Find("APositions");
        GameObject positionsB = GameObject.Find("BPositions");
        if (checkOwner)
        {
            string idTower = idTowers[0].ToString().Replace("\"", "");
            GameObject nexusAgameObject = Instantiate(nexusA, positionsA.transform.GetChild(0).transform.position, Quaternion.identity);
            nexusAgameObject.GetComponent<NetworkEntity>().Id = idTower;
            nexusAgameObject.GetComponent<NetworkEntity>().isTower = true;
            towersData.Add(idTower, nexusAgameObject);

            idTower = idTowers[1].ToString().Replace("\"", "");
            GameObject firstTowerAGameObject = Instantiate(towerA, positionsA.transform.GetChild(1).transform.position, Quaternion.identity);
            firstTowerAGameObject.GetComponent<NetworkEntity>().Id = idTower;
            firstTowerAGameObject.GetComponent<NetworkEntity>().isTower = true;
            towersData.Add(idTower, firstTowerAGameObject);

            idTower = idTowers[2].ToString().Replace("\"", "");
            GameObject secondTowerAGameObject = Instantiate(towerA, positionsA.transform.GetChild(2).transform.position, Quaternion.identity);
            secondTowerAGameObject.GetComponent<NetworkEntity>().Id = idTower;
            secondTowerAGameObject.GetComponent<NetworkEntity>().isTower = true;
            towersData.Add(idTower, secondTowerAGameObject);

            idTower = idTowers[3].ToString().Replace("\"", "");
            GameObject nexusBgameObjectRemote = Instantiate(nexusBRemote, positionsB.transform.GetChild(0).transform.position, Quaternion.identity);
            nexusBgameObjectRemote.GetComponent<NetworkEntity>().Id = idTower;
            nexusBgameObjectRemote.GetComponent<NetworkEntity>().isTower = true;
            nexusBgameObjectRemote.GetComponent<FollowTower>().myPlayer = listOfCharacter.transform.GetChild(NetworkRegisterLogin.noCharacter).gameObject;
            towersData.Add(idTower, nexusBgameObjectRemote);

            idTower = idTowers[4].ToString().Replace("\"", "");
            GameObject firstTowerBGameObjectRemote = Instantiate(towerBRemote, positionsB.transform.GetChild(1).transform.position, Quaternion.identity);
            firstTowerBGameObjectRemote.GetComponent<NetworkEntity>().Id = idTower;
            firstTowerBGameObjectRemote.GetComponent<NetworkEntity>().isTower = true;
            firstTowerBGameObjectRemote.GetComponent<FollowTower>().myPlayer = listOfCharacter.transform.GetChild(NetworkRegisterLogin.noCharacter).gameObject;
            towersData.Add(idTower, firstTowerBGameObjectRemote);

            idTower = idTowers[5].ToString().Replace("\"", "");
            GameObject secondTowerBGameObjectRemote = Instantiate(towerBRemote, positionsB.transform.GetChild(2).transform.position, Quaternion.identity);
            secondTowerBGameObjectRemote.GetComponent<NetworkEntity>().Id = idTower;
            secondTowerBGameObjectRemote.GetComponent<NetworkEntity>().isTower = true;
            secondTowerBGameObjectRemote.GetComponent<FollowTower>().myPlayer = listOfCharacter.transform.GetChild(NetworkRegisterLogin.noCharacter).gameObject;
            towersData.Add(idTower, secondTowerBGameObjectRemote);
        }        
        else
        {
            string idTower = idTowers[0].ToString().Replace("\"", "");
            GameObject nexusAgameObjectRemote = Instantiate(nexusARemote, positionsA.transform.GetChild(0).transform.position, Quaternion.identity);
            nexusAgameObjectRemote.GetComponent<NetworkEntity>().Id = idTower;
            nexusAgameObjectRemote.GetComponent<NetworkEntity>().isTower = true;
            nexusAgameObjectRemote.GetComponent<FollowTower>().myPlayer = listOfCharacter.transform.GetChild(NetworkRegisterLogin.noCharacter).gameObject;
            towersData.Add(idTower, nexusAgameObjectRemote);

            idTower = idTowers[1].ToString().Replace("\"", "");
            GameObject firstTowerAGameObjectRemote = Instantiate(towerARemote, positionsA.transform.GetChild(1).transform.position, Quaternion.identity);
            firstTowerAGameObjectRemote.GetComponent<NetworkEntity>().Id = idTower;
            firstTowerAGameObjectRemote.GetComponent<NetworkEntity>().isTower = true;
            firstTowerAGameObjectRemote.GetComponent<FollowTower>().myPlayer = listOfCharacter.transform.GetChild(NetworkRegisterLogin.noCharacter).gameObject;
            towersData.Add(idTower, firstTowerAGameObjectRemote);

            idTower = idTowers[2].ToString().Replace("\"", "");
            GameObject secondTowerAGameObjectRemote = Instantiate(towerARemote, positionsA.transform.GetChild(2).transform.position, Quaternion.identity);
            secondTowerAGameObjectRemote.GetComponent<NetworkEntity>().Id = idTower;
            secondTowerAGameObjectRemote.GetComponent<NetworkEntity>().isTower = true;
            secondTowerAGameObjectRemote.GetComponent<FollowTower>().myPlayer = listOfCharacter.transform.GetChild(NetworkRegisterLogin.noCharacter).gameObject;
            towersData.Add(idTower, secondTowerAGameObjectRemote);


            idTower = idTowers[3].ToString().Replace("\"", "");
            GameObject nexusBgameObject = Instantiate(nexusB, positionsB.transform.GetChild(0).transform.position, Quaternion.identity);
            nexusBgameObject.GetComponent<NetworkEntity>().Id = idTower;
            nexusBgameObject.GetComponent<NetworkEntity>().isTower = true;
            towersData.Add(idTower, nexusBgameObject);


            idTower = idTowers[4].ToString().Replace("\"", "");
            GameObject firstTowerBGameObject = Instantiate(towerB, positionsB.transform.GetChild(1).transform.position, Quaternion.identity);
            firstTowerBGameObject.GetComponent<NetworkEntity>().Id = idTower;
            firstTowerBGameObject.GetComponent<NetworkEntity>().isTower = true;
            towersData.Add(idTower, firstTowerBGameObject);

            idTower = idTowers[5].ToString().Replace("\"", "");
            GameObject secondTowerBGameObject = Instantiate(towerB, positionsB.transform.GetChild(2).transform.position, Quaternion.identity);
            secondTowerBGameObject.GetComponent<NetworkEntity>().Id = idTower;
            secondTowerBGameObject.GetComponent<NetworkEntity>().isTower = true;
            towersData.Add(idTower, secondTowerBGameObject);

        }
    }

   

    public IEnumerator SpawnMinions(List<string> idMinions, bool checkOwner)
    {
        Debug.Log("SPAWN MINIONS");
        GameObject spawnPointA = GameObject.Find("SpawnPointMinionsA");
        GameObject spawnPointB = GameObject.Find("SpawnPointMinionsB");
       
        for(int k=0;k<idMinions.Count;k++)
        {

            if (checkOwner)
            {
                GameObject maxinon1 = Instantiate(maxinon, spawnPointA.transform.position, Quaternion.identity);
                maxinon1.GetComponent<FollowerMinion>().SetWayPoints(minionsSpawnPointsA);
                maxinon1.GetComponent<Target>().targetTransform = spawnPointB.transform;
                maxinon1.GetComponent<NetworkEntity>().Id = idMinions[k];
                maxinon1.GetComponent<CreepAi>().number = k+1;
                minionsData.Add(idMinions[k], maxinon1);
                ++k;

                GameObject maxinon2 = Instantiate(maxinonRemote, spawnPointB.transform.position, Quaternion.identity);
                maxinon2.GetComponent<FollowerMinion>().SetWayPoints(minionsSpawnPointsB);
                maxinon2.GetComponent<Target>().targetTransform = spawnPointA.transform;
                maxinon2.GetComponent<NetworkEntity>().Id = idMinions[k];
                maxinon2.GetComponent<FollowMinions>().myPlayer = listOfCharacter.transform.GetChild(NetworkRegisterLogin.noCharacter).gameObject;
                maxinon2.GetComponent<CreepAi>().number = k + 1;

                minionsData.Add(idMinions[k], maxinon2);

                yield return new WaitForSeconds(1.0f);                
            }
            if (!checkOwner)
            {
                GameObject maxinon1 = Instantiate(maxinonRemote, spawnPointA.transform.position, Quaternion.identity);
                maxinon1.GetComponent<FollowerMinion>().SetWayPoints(minionsSpawnPointsA);
                maxinon1.GetComponent<Target>().targetTransform = spawnPointB.transform;
                maxinon1.GetComponent<NetworkEntity>().Id = idMinions[k];
                maxinon1.GetComponent<FollowMinions>().myPlayer = listOfCharacter.transform.GetChild(NetworkRegisterLogin.noCharacter).gameObject;
                maxinon1.GetComponent<CreepAi>().number = k + 1;

                minionsData.Add(idMinions[k], maxinon1);
                ++k;

                GameObject maxinon2 = Instantiate(maxinon, spawnPointB.transform.position, Quaternion.identity);
                maxinon2.GetComponent<FollowerMinion>().SetWayPoints(minionsSpawnPointsB);
                maxinon2.GetComponent<Target>().targetTransform = spawnPointA.transform;
                maxinon2.GetComponent<NetworkEntity>().Id = idMinions[k];
                maxinon2.GetComponent<CreepAi>().number = k + 1;

                minionsData.Add(idMinions[k], maxinon2);               

                yield return new WaitForSeconds(1.0f);
            }
            
        }

        CreepAi.numberOrder = 0;

        yield return null;


    }


}
