using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepAi : MonoBehaviour {
    public static int numberOrder;
    Vector3 offset;

    public Vector3 ComputeOffset()
    {
        if(CreepAi.numberOrder == 1)
        {
            offset = new Vector3(-3f, 0f, 0f);
        }
        if (CreepAi.numberOrder == 2)
        {
            offset = new Vector3(-3f, 0f, -5f);
        }

        if (CreepAi.numberOrder == 3)
        {
            offset = new Vector3(-3f, 0f, 5f);
        }
        return offset;
    }


}
