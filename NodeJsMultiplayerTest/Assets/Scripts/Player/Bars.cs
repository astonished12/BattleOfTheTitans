﻿using UnityEngine;
using System.Collections;

public class Bars : MonoBehaviour
{
    public Camera cameraPlayer;
    private Quaternion rotation;
    private Vector3 position;
    private Alive infoPlayer;
    void Awake()
    {
        rotation = transform.rotation;
        position = transform.parent.position - transform.position;
    }

    void Update()
    {
        transform.rotation = rotation;
        transform.position = transform.parent.position - position;
    }

   
}