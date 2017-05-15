using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System.Text.RegularExpressions;
using System;

public class NetworkScript : MonoBehaviour
{

    public static SocketIOComponent SocketIO;
    public SpawnerPlayer spawner;
    private GameObject player;
    public GameObject listOfChracter;
    public Dictionary<string, GameObject> minionsData = new Dictionary<string, GameObject>();

    private bool ownerFlag;
    private GameObject[] objects;
    private GameObject targetOfAttacker;
    private GameObject attacker;
    //public GameObject mainChracter;

    private void Awake()
    {
        player = listOfChracter.transform.GetChild(NetworkRegisterLogin.noCharacter).gameObject;   
        SocketIO = GameObject.Find("SocketRegisterLogin").GetComponent<SocketIOComponent>();
    }
    // Use this for initialization
    void Start()
    {        
        SocketIO.On("identify", OnIdentify);
        //SocketIO.On("anotherplayerconnected", OtherPlayer);
        SocketIO.On("playerLeft", OnPlayerLeft);
        SocketIO.On("playerMove", OnMove);
        SocketIO.On("followPlayer", OnFollow);
        SocketIO.On("followTower", OnFollowTower);
        SocketIO.On("followMinion", OnFollowMinion);
        SocketIO.On("minionHasNoTarget", OnMinionHasNooTarget);
        SocketIO.On("attackPlayer", OnAttack);
        SocketIO.On("spawnMinions",OnSpawnMinions);
        SocketIO.On("keyPressed", OnKeyPressed);
        SocketIO.On("minionFollowMinion", OnMinionFollowMinion);
        SocketIO.On("minionAttackMinion", OnMinionAttackMinion);
        SocketIO.On("towerAttack", OnTowerAttack);
    }

    

    void OnIdentify(SocketIOEvent Obj)
    {
        player.transform.position = GetVectorPositionFromJson(Obj.data);
        ownerFlag = Convert.ToBoolean(ElementFromJsonToString(Obj.data.GetField("owner").ToString())[1]);
        //Debug.Log(" Hostul are owner " + Obj.data["owner"] +ownerFlag);
        var players = Obj.data.GetField("allPlayersAtCurrentTime");
        var idTowers = Obj.data.GetField("towersId");
        Debug.Log(idTowers);
        var socket_id = ElementFromJsonToString(Obj.data.GetField("socket_id").ToString())[1];
        player.GetComponent<NetworkEntity>().Id = socket_id;
        player.GetComponent<NetworkEntity>().ownerFlag = ownerFlag;

       

        spawner.SpawnTowers(idTowers, ownerFlag);
          
        spawner.AddMyPlayer(socket_id, player);

        for (int i = 0; i < players.list.Count; i++)
        {
            string playerKey = (string)players.keys[i];
            if (playerKey != socket_id)
             {
                JSONObject playerData = (JSONObject)players.list[i];
                int noRemoteCharacters = int.Parse(ElementFromJsonToString(playerData.GetField("characterNumber").ToString())[1]);
                bool ownerFlagAux = Convert.ToBoolean(ElementFromJsonToString(playerData.GetField("isOwner").ToString())[1]);
                //Debug.Log(" la remote avem owner " + playerData["isOwner"]+ " "+ ownerFlagAux);
                spawner.SpawnPlayer(playerKey, noRemoteCharacters,GetVectorPositionFromJson(playerData), ownerFlagAux);
            }
        }
        
    }

    string[] ElementFromJsonToString(string target)
    {
        string[] newString = Regex.Split(target, "\"");
        return newString;
    }
 
    Vector3 GetVectorPositionFromJson(JSONObject Json)
    {
        return new Vector3(float.Parse(Json["x"].ToString().Replace("\"","")), float.Parse(Json["y"].ToString().Replace("\"","")), float.Parse(Json["z"].ToString().Replace("\"", "")));
    }

   

    private void OnPlayerLeft(SocketIOEvent obj)
    {
        string socket_id = ElementFromJsonToString(obj.data.GetField("socket_id").ToString())[1];
        spawner.PlayerLeft(socket_id);       
    }

    private void OnMove(SocketIOEvent obj)
    {
        string socket_id = ElementFromJsonToString(obj.data["socket_id"].ToString())[1];      
        Vector3 targetPostion = GetVectorPositionFromJson(obj.data);
        var otherPlayer = spawner.OtherPlayersGameObjects[socket_id];
        var walker = otherPlayer.GetComponent<NavagiateToPosition>();
        walker.SetTargetPosition(targetPostion);
    }

    private void OnFollow(SocketIOEvent obj)
    {
        string socket_id = ElementFromJsonToString(obj.data["socket_id"].ToString())[1];
        string target_id = ElementFromJsonToString(obj.data["target_id"].ToString())[1];

        //remote
        var playerWhoDoRequest = spawner.OtherPlayersGameObjects[socket_id];
        //client player
        var target = spawner.OtherPlayersGameObjects[target_id];        

        var followerOfPlaeryRequested = playerWhoDoRequest.GetComponent<Target>();
        followerOfPlaeryRequested.SetTargetTransform(target.transform);
    }
    private void OnFollowTower(SocketIOEvent obj)
    {
        string socket_id = ElementFromJsonToString(obj.data["socket_id"].ToString())[1];
        string target_id = ElementFromJsonToString(obj.data["target_id"].ToString())[1];
        //remote
        var playerWhoDoRequest = spawner.OtherPlayersGameObjects[socket_id];
        //tower
        var target = spawner.towersData[target_id];

        var followerOfPlaeryRequested = playerWhoDoRequest.GetComponent<Target>();
        followerOfPlaeryRequested.SetTargetTransform(target.transform);        
    }

    private void OnFollowMinion(SocketIOEvent Obj)
    {
        string socket_id = ElementFromJsonToString(Obj.data["socket_id"].ToString())[1];
        string target_id = ElementFromJsonToString(Obj.data["target_id"].ToString())[1];
        //remote
        var playerWhoDoRequest = spawner.OtherPlayersGameObjects[socket_id];
        //minion
        var target = spawner.minionsData[target_id];

        var followerOfPlaeryRequested = playerWhoDoRequest.GetComponent<Target>();
        followerOfPlaeryRequested.SetTargetTransform(target.transform);
    }
    public void OnAttack(SocketIOEvent obj)
    {
        string target_id = ElementFromJsonToString(obj.data["target_id"].ToString())[1];
        string socket_id = ElementFromJsonToString(obj.data["socket_id"].ToString())[1];
        attacker = spawner.OtherPlayersGameObjects[socket_id];
        Debug.Log(socket_id + " " + target_id);
        
        if (spawner.OtherPlayersGameObjects.ContainsKey(target_id))
        {
            Debug.Log("Attack inamic");
            targetOfAttacker = spawner.OtherPlayersGameObjects[target_id];       
            targetOfAttacker.transform.rotation = Quaternion.LookRotation(attacker.transform.position - targetOfAttacker.transform.position);
            attacker.transform.rotation = Quaternion.LookRotation(targetOfAttacker.transform.position - attacker.transform.position);
        }
        else if(spawner.towersData.ContainsKey(target_id))
        {
            Debug.Log("Attack tureta");          
            targetOfAttacker = spawner.towersData[target_id];
            attacker.transform.rotation = Quaternion.LookRotation(targetOfAttacker.transform.position - attacker.transform.position);
        }
        else if (spawner.minionsData.ContainsKey(target_id))
        {
            Debug.Log("Attack maxinon");
            targetOfAttacker = spawner.minionsData[target_id];
            attacker.transform.rotation = Quaternion.LookRotation(targetOfAttacker.transform.position - attacker.transform.position);
        }
       

        attacker.GetComponent<Animator>().SetFloat("multiplier", 2);
        attacker.GetComponent<Animator>().SetTrigger("attack");
        
        spawner.SpawnBullet(attacker, attacker.transform.position, targetOfAttacker.transform);

        
    }

    private void OnSpawnMinions(SocketIOEvent Obj)
    {
        var idTowers = Obj.data.GetField("minionsId");
        List<string> myIdTowers = new List<string>();
        for (int i=0;i< idTowers.Count;i++)
        {
            myIdTowers.Add(idTowers[i].ToString().Replace("\"", ""));

        }

        StartCoroutine(spawner.SpawnMinions(myIdTowers,ownerFlag));
    }

    private void OnMinionFollowMinion(SocketIOEvent Obj)
    {
        string follower_id = ElementFromJsonToString(Obj.data["id_follower"].ToString())[1];
        string target_id = ElementFromJsonToString(Obj.data["target_id"].ToString())[1];

       
        if(spawner.minionsData.ContainsKey(follower_id) && spawner.minionsData.ContainsKey(target_id))
        {
            var follower = spawner.minionsData[follower_id];
            var target = spawner.minionsData[target_id];

            target.GetComponent<CreepAi>().posibleTarget = follower;
            target.GetComponent<FollowerMinion>().mustStop = 1;

            follower.GetComponent<CreepAi>().posibleTarget = target;
            follower.GetComponent<FollowerMinion>().mustStop = 1;

        }
        else if(spawner.minionsData.ContainsKey(follower_id) && spawner.OtherPlayersGameObjects.ContainsKey(target_id))
        {

            Debug.Log("Minionul "+follower_id+" urmareste jucatorul "+target_id);
            var follower = spawner.minionsData[follower_id];
            var target = spawner.OtherPlayersGameObjects[target_id];


            follower.GetComponent<CreepAi>().posibleTarget = target;
            follower.GetComponent<FollowerMinion>().mustStop = 1;
        }
        else if(spawner.minionsData.ContainsKey(follower_id) && spawner.towersData.ContainsKey(target_id)){
            var follower = spawner.minionsData[follower_id];
            var target = spawner.towersData[target_id];



            follower.GetComponent<CreepAi>().posibleTarget = target;
            follower.GetComponent<FollowerMinion>().mustStop = 1;
        }
    }

    private void OnMinionHasNooTarget(SocketIOEvent Obj)
    {
        string target_id = ElementFromJsonToString(Obj.data["target_id"].ToString())[1];

        //Debug.Log("Minionul "+target_id + " nu are inamici in jur");
        if (spawner.minionsData.ContainsKey(target_id))
        {
            var target = spawner.minionsData[target_id];
            if (target)
            {
                target.GetComponent<CreepAi>().posibleTarget = null;
                target.GetComponent<FollowerMinion>().mustStop = 0;
            }
        }
    }

    private void OnMinionAttackMinion(SocketIOEvent Obj)
    {
        string attacker_id_minion = ElementFromJsonToString(Obj.data["id_attacker"].ToString())[1];
        string target_id_minion = ElementFromJsonToString(Obj.data["target_id"].ToString())[1];

        if (spawner.minionsData.ContainsKey(attacker_id_minion) && spawner.minionsData.ContainsKey(target_id_minion))
        {
            var atacker_minion = spawner.minionsData[attacker_id_minion];
            var target_minion = spawner.minionsData[target_id_minion];
           if(target_minion.GetComponent<Alive>().isAlive && atacker_minion.GetComponent<Alive>().isAlive)
            {
                spawner.SpawnBullet(atacker_minion, atacker_minion.transform.position, target_minion.transform);
            }
        }
        else if(spawner.minionsData.ContainsKey(attacker_id_minion) && spawner.OtherPlayersGameObjects.ContainsKey(target_id_minion))
        {
            Debug.Log("Minionul " + attacker_id_minion + " urmareste jucatorul " + target_id_minion);

            var atacker_minion = spawner.minionsData[attacker_id_minion];
            var target_minion = spawner.OtherPlayersGameObjects[target_id_minion];
            if(target_minion.GetComponent<Alive>().isAlive)
                spawner.SpawnBullet(atacker_minion, atacker_minion.transform.position, target_minion.transform);
        }
        else if (spawner.minionsData.ContainsKey(attacker_id_minion) && spawner.towersData.ContainsKey(target_id_minion))
        {
            var atacker_minion = spawner.minionsData[attacker_id_minion];
            var tower_target = spawner.towersData[target_id_minion];
            if (tower_target.GetComponent<Alive>().isAlive)
                spawner.SpawnBullet(atacker_minion, atacker_minion.transform.position, tower_target.transform);
        }
    }

    private void OnTowerAttack(SocketIOEvent Obj)
    {
        string attacker_id = ElementFromJsonToString(Obj.data["id_attacker"].ToString())[1];
        string target_id = ElementFromJsonToString(Obj.data["target_id"].ToString())[1];

        Debug.Log("Tureta " + attacker_id + "attacka pe " + target_id);
        if (spawner.towersData.ContainsKey(attacker_id) && (spawner.minionsData.ContainsKey(target_id) || spawner.OtherPlayersGameObjects.ContainsKey(target_id)))
        {
            GameObject attaker = spawner.towersData[attacker_id];
            GameObject target = null;
            if(spawner.minionsData.ContainsKey(target_id))
                 target = spawner.minionsData[target_id];
            else
                 target = spawner.OtherPlayersGameObjects[target_id];

            if (attaker.GetComponent<TowerAtack>())
            {
                attaker.GetComponent<TowerAtack>().lastTimeAttack = Time.time;
            }
            if(target && target.GetComponent<Alive>().isAlive)
              spawner.SpawnBullet(attaker,attaker.transform.position+new Vector3(0f,2f,0f) ,target.transform);
        }

    }
    private Vector3 makePositiveVector(Vector3 vectorToTransform)
    {
        if (vectorToTransform.x < 0)
            vectorToTransform.x *= (-1);
         if (vectorToTransform.y < 0)
            vectorToTransform.y*= (-1);

        return vectorToTransform;
    }

    private void OnKeyPressed(SocketIOEvent Obj)
    {
        string user_Id = ElementFromJsonToString(Obj.data["user_id"].ToString())[1];
        string key = ElementFromJsonToString(Obj.data["key"].ToString())[1];

        Debug.Log("Jucatorul " + user_Id + " foloeste skilul " + key);
        if (spawner.OtherPlayersGameObjects.ContainsKey(user_Id))
        {
            GameObject playerWhoPressedKey = spawner.OtherPlayersGameObjects[user_Id];
            SpecialAttack[] specialAttacks = playerWhoPressedKey.GetComponents<SpecialAttack>();
            foreach(SpecialAttack skill in specialAttacks)
            {
                if(skill.key.ToString() == key)
                {
                   skill.inAction = true;                   
                }
                if(skill.key.ToString() == key && skill.key == KeyCode.Q)
                {
                    Vector3 test = GetVectorPositionFromJson(Obj.data);
                    Debug.Log(test);
                    skill.targetPositions = test;
                 }
            }
        }
    }

}

