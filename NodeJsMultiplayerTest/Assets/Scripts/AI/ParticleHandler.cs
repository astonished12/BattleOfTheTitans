using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour {
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    GameObject target;
    int numCollisionEvents = 0;
    bool onceActived = false;
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }
    
    void Update()
    {

        if (numCollisionEvents > 0 && target.GetComponent<Alive>().isAlive && gameObject.GetComponent<SkillInfo>().isActive == true && !onceActived)
        {
            target.GetComponent<Alive>().OnHit(target, gameObject.GetComponent<SkillInfo>().damage);
            gameObject.GetComponent<SkillInfo>().isActive = false;
            numCollisionEvents = 0;
            onceActived = true;
        }
    }

    void OnParticleCollision(GameObject collider)
    {

        target = collider;    
        numCollisionEvents = part.GetCollisionEvents(collider, collisionEvents);       

    }
    

}
