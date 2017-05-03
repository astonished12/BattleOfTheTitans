using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour {

    void OnParticleCollision(GameObject test)
    {
        if(test!=gameObject)
             Debug.Log("Got hit " + gameObject + " de la " + test.name);
    }

}
