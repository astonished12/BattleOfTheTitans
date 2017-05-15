using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class JSONParser {

    public string Vector3ToJsonObject(Vector3 vector)
    {
        return string.Format(@"{{""x"":""{0}"",""y"":""{1}"",""z"":""{2}""}}", vector.x, vector.y, vector.z);
    }

    public string TargetIdToJson(string id)
    {
        return string.Format(@"{{""idTarget"":""{0}""}}", id);
    }
    public string MessageToJson(string message)
    {
        return string.Format(@"{{""message"":""{0}""}}", message);
    }
    public string MinionsDataToJsonFollow(string idFollower, string id)
    {
        return string.Format(@"{{""idFollower"":""{0}"",""idTarget"":""{1}""}}", idFollower, id);
    }

    public string AttackAIToJson(string idAttacker, string id)
    {
        return string.Format(@"{{""idAttacker"":""{0}"",""idTarget"":""{1}""}}", idAttacker, id);
    }

    public string[] ElementFromJsonToString(string target)
    {
        string[] newString = Regex.Split(target, "\"");
        return newString;
    }

    public string KeyIdToJson(string id)
    {
        return string.Format(@"{{""key"":""{0}""}}", id);
    }

    public string QKeyToJson(string id, Vector3 vector)
    {
        return string.Format(@"{{""key"":""{0}"",""x"":""{1}"",""y"":""{2}"",""z"":""{3}""}}", id, vector.x, vector.y, vector.z);
    }

    public Vector3 GetVectorPositionFromJson(JSONObject Json)
    {
        return new Vector3(float.Parse(Json["x"].ToString().Replace("\"", "")), float.Parse(Json["y"].ToString().Replace("\"", "")), float.Parse(Json["z"].ToString().Replace("\"", "")));
    }

}
