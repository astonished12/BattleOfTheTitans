using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCommunication : MonoBehaviour {

    public SocketIOComponent SocketIO;

    public void sendLastPositionToNodeServer(Vector3 endPosition)
    {
        //SocketIO.Emit("move",new JSONObject(Vector3ToJsonObject(endPosition)));
    }

    private string Vector3ToJsonObject(Vector3 vector)
    {
        return string.Format(@"{{""x"":""{0}"",""y"":""{1}""}}", vector.x, vector.z);
    }
}
