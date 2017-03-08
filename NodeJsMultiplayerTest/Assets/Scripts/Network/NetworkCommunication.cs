using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCommunication : MonoBehaviour {
      

    public void SendLastPositionToNodeServer(Vector3 endPosition)
    {
        NetworkScript.SocketIO.Emit("move",new JSONObject(Vector3ToJsonObject(endPosition)));
    }

    private string Vector3ToJsonObject(Vector3 vector)
    {
        return string.Format(@"{{""x"":""{0}"",""y"":""{1}"",""z"":""{2}""}}", vector.x, vector.y,vector.z);
    }
}
