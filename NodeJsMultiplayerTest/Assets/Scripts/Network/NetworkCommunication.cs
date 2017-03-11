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
        NetworkScript.SocketIO.Emit("follow", new JSONObject(PlayerIdToJson(id)));
    }

    private string Vector3ToJsonObject(Vector3 vector)
    {
        return string.Format(@"{{""x"":""{0}"",""y"":""{1}"",""z"":""{2}""}}", vector.x, vector.y,vector.z);
    }

    private string PlayerIdToJson(string id)
    {
        return string.Format(@"{{""idTarget"":""{0}""}}", id);
    }


}
