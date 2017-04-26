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


    public void SendAttackerId(string id)
    {
        NetworkScript.SocketIO.Emit("attack", new JSONObject(TargetIdToJson(id))); 
    }

    public void SendTowerNumberToFollow(string idTower)
    {
        Debug.Log("Seding tower id to node " + idTower);
        NetworkScript.SocketIO.Emit("followTower", new JSONObject(TargetIdToJson(idTower)));
    }

}
