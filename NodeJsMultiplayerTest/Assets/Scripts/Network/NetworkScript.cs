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
    public Dictionary<string, GameObject> towersData = new Dictionary<string, GameObject>();
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
        SocketIO.On("attackPlayer", OnAttack);
        SocketIO.On("spawnMinions",OnSpawnMinions);
        SocketIO.On("keyPressed", OnKeyPressed);
        SocketIO.On("minionFollowMinion", OnMinionFollowMinion);
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

        //IDENTIFY TOWERS      
        objects = GameObject.FindGameObjectsWithTag("clickable");       
        var objectCount = objects.Length;
        int k = 0;  
        foreach (var obj in objects)
        {            
            obj.GetComponent<FollowTower>().myPlayer = player;
            string idTower = idTowers[k].ToString().Replace("\"", "");
            obj.GetComponent<NetworkEntity>().Id = idTower;
            towersData.Add(idTower, obj);
            k++;        
        }    
          
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
        //client player
        var target = towersData[target_id];

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
        else if(towersData.ContainsKey(target_id))
        {
            Debug.Log("Attack tureta");          
            targetOfAttacker = towersData[target_id];
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

       
        if(spawner.minionsData.ContainsKey(follower_id) && spawner.minionsData.ContainsKey(follower_id))
        {
            var follower = spawner.minionsData[follower_id];
            var target = spawner.minionsData[target_id];

            follower.GetComponent<CreepAi>().isMovingOn = true;
            
            var newTrans = new GameObject().transform;
            newTrans.position = makePositiveVector((follower.gameObject.transform.position - target.gameObject.transform.position)/2);


            newTrans.position += target.GetComponent<CreepAi>().ComputeOffset(target.GetComponent<CreepAi>().number);
            target.GetComponent<Target>().targetTransform = newTrans;

            newTrans.position -= target.GetComponent<CreepAi>().ComputeOffset(target.GetComponent<CreepAi>().number);
            newTrans.position += follower.GetComponent<CreepAi>().ComputeOffset(follower.GetComponent<CreepAi>().number);
            follower.GetComponent<Target>().targetTransform = newTrans;


            
        }
        else
        {
            Debug.Log("Problem with minion id follwings on minion follow minion");
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
            }
        }
    }
}

