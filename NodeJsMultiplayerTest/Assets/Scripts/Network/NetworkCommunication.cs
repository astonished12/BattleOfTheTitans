using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCommunication : MonoBehaviour {
      

    public void SendLastPositionToNodeServer(Vector3 endPosition)
    {
        NetworkScript.SocketIO.Emit("move",new JSONObject(Vector3ToJsonObject(endPosition)));
    }
    

    public void SendPlayerIdToFollow(string id)
    {
        NetworkScript.SocketIO.Emit("follow", new JSONObject(TargetIdToJson(id)));
    }

    private string Vector3ToJsonObject(Vector3 vector)
    {
        return string.Format(@"{{""x"":""{0}"",""y"":""{1}"",""z"":""{2}""}}", vector.x, vector.y,vector.z);
    }

    private string TargetIdToJson(string id)
    {
        return string.Format(@"{{""idTarget"":""{0}""}}", id);
    }
    private string MessageToJson(string message)
    {
        return string.Format(@"{{""message"":""{0}""}}", message);
    }
    private string MinionsDataToJsonFollow(string idFollower,string id)
    {
        return string.Format(@"{{""idFollower"":""{0}"",""idTarget"":""{1}""}}", idFollower, id);
    }

    private string AttackAIToJson(string idAttacker, string id)
    {
        return string.Format(@"{{""idAttacker"":""{0}"",""idTarget"":""{1}""}}", idAttacker, id);
    }
    public void SendAttackerId(string id)
    {
        NetworkScript.SocketIO.Emit("attack", new JSONObject(TargetIdToJson(id))); 
    }

    public void SendTowerNumberToFollow(string idTower)
    {
        Debug.Log("Seding tower id to node " + idTower);
        NetworkScript.SocketIO.Emit("followTower", new JSONObject(TargetIdToJson(idTower)));
    }


    public void SendMinionDataToFollow(string idFollower,string idMinion)
    {
        NetworkScript.SocketIO.Emit("minionFollowMinion", new JSONObject(MinionsDataToJsonFollow(idFollower, idMinion)));
    }

    public void SendMinionNumberToFollow(string idMinion)
    {
        NetworkScript.SocketIO.Emit("followMinion", new JSONObject(TargetIdToJson(idMinion)));
    }

    public void SendMinionDataToAttack(string idAttacker,string idTargetMinion)
    {
        NetworkScript.SocketIO.Emit("minionAttackMinion", new JSONObject(AttackAIToJson(idAttacker, idTargetMinion)));
    }

    public void SendMinionHasNoEnemyAround(string idMinion)
    {
        NetworkScript.SocketIO.Emit("minionNoTarget", new JSONObject(TargetIdToJson(idMinion)));
    }

    public void SendMinionsOrPlayerIdToServerForTowerAttacking(string idTower,string idTarget)
    {
        NetworkScript.SocketIO.Emit("towerTarget", new JSONObject(AttackAIToJson(idTower,idTarget)));
    }

    public void SendMessageChat(string message)
    {
        NetworkScript.SocketIO.Emit("newMessageGameChat", new JSONObject(MessageToJson(message)));
    }
}
