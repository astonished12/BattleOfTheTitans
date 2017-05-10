using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepAi : MonoBehaviour
{
    public static int numberOrder;
    public Vector3 offset;
    public bool isMovingOn = false;
    public bool isAttacking = false;
    public Transform final;
    public int number;
    public GameObject posibleTarget;
    void Start()
    {
        final = GetComponent<Target>().targetTransform;
    }

      
    public Vector3 ComputeOffset(int number)
    {

        if (number == 1)
        {
            offset = new Vector3(-2f, 0f, 2f);
        }
        if (number == 2)
        {
            offset = new Vector3(2f, 0f, 2f);
        }

        if (number == 3)
        {
            offset = new Vector3(-2f, 0f, 0f);
        }
        if (number == 4)
        {
            offset = new Vector3(2f, 0f, 0f);
        }
        if(number == 5)
        {
            offset = new Vector3(-2f, 0f, -2f);
        }
        if (number == 6)
        {
            offset = new Vector3(2f, 0f, -2f);
        }
        return offset;
    }

}
 

