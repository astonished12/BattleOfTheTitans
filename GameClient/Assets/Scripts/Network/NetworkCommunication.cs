using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCommunication : MonoBehaviour {

    JSONParser myJsonParser = new JSONParser();
    public void SendLastPositionToNodeServer(Vector3 currentPositions, Vector3 destinationPosition)
    {
        JSONObject pack = new JSONObject(JSONObject.Type.OBJECT);
        pack.AddField("current", new JSONObject(myJsonParser.Vector3ToJsonObject(currentPositions)));
        pack.AddField("destination", new JSONObject(myJsonParser.Vector3ToJsonObject(destinationPosition)));

        NetworkScript.SocketIO.Emit("move",pack);
    }   

    public void SendPlayerIdToFollow(string id)
    {
        NetworkScript.SocketIO.Emit("follow", new JSONObject(myJsonParser.TargetIdToJson(id)));
    }

    public void SendAttackerId(string id)
    {
        NetworkScript.SocketIO.Emit("attack", new JSONObject(myJsonParser.TargetIdToJson(id))); 
    }

    public void SendTowerNumberToFollow(string idTower)
    {
        Debug.Log("Seding tower id to node " + idTower);
        NetworkScript.SocketIO.Emit("followTower", new JSONObject(myJsonParser.TargetIdToJson(idTower)));
    }


    public void SendMinionDataToFollow(string idFollower,string idMinion)
    {
        NetworkScript.SocketIO.Emit("minionFollowMinion", new JSONObject(myJsonParser.MinionsDataToJsonFollow(idFollower, idMinion)));
    }

    public void SendMinionNumberToFollow(string idMinion)
    {
        NetworkScript.SocketIO.Emit("followMinion", new JSONObject(myJsonParser.TargetIdToJson(idMinion)));
    }

    public void SendMinionDataToAttack(string idAttacker,string idTargetMinion)
    {
        NetworkScript.SocketIO.Emit("minionAttackMinion", new JSONObject(myJsonParser.AttackAIToJson(idAttacker, idTargetMinion)));
    }

    public void SendMinionHasNoEnemyAround(string idMinion)
    {
        NetworkScript.SocketIO.Emit("minionNoTarget", new JSONObject(myJsonParser.TargetIdToJson(idMinion)));
    }

    public void SendMinionsOrPlayerIdToServerForTowerAttacking(string idTower,string idTarget)
    {
        NetworkScript.SocketIO.Emit("towerTarget", new JSONObject(myJsonParser.AttackAIToJson(idTower,idTarget)));
    }

    public void SendMessageChat(string message)
    {
        NetworkScript.SocketIO.Emit("newMessageGameChat", new JSONObject(myJsonParser.MessageToJson(message)));
    }
}
