using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour {


    public KeyCode key;
    public int damage;
    public bool inAction;
    public Texture2D pictureSkill;
    public GameObject objectToSpawn;
    float distance = 10f;
    public Vector3 targetPositions;
    

    // Use this for initialization
    void Start () { 
	    	
	}

    // Update is called once per frame
    void Update()
    {
        if (key.ToString() == "Q" && inAction && GetComponent<Mana>().curMana>objectToSpawn.GetComponent<SkillInfo>().cost) {
            Debug.Log("Special attack ON " + key.ToString());
            Debug.Log("Instantiez Q la " + targetPositions);
           
            if (GetDistanceBetweenPositions(transform.position, targetPositions) > distance)
            {
                Vector2 start = new Vector2(transform.position.x, transform.position.z);
                Vector2 end = new Vector2(targetPositions.x, targetPositions.z);
                targetPositions = MakeQSpawnPointTarget(distance, transform.position, GetNormalizeStartToEnd(start, end));
            }

            if (targetPositions.y > 0.3)
            {
                targetPositions.y = 0.2f;
            }
            GameObject go = Instantiate(objectToSpawn, targetPositions+new Vector3(0f,0.2f,0f), Quaternion.identity) as GameObject;
            go.GetComponent<SkillInfo>().damage = damage;
            GetComponent<Mana>().DecreaseMana(go.GetComponent<SkillInfo>().cost);
            Destroy(go, 2);
            if (gameObject == GameObject.FindGameObjectWithTag("Player"))
            {
                ActionBar.skillSlots[0].coolDownActive = true;
                ActionBar.skillSlots[0].coolDonwTime = go.GetComponent<SkillInfo>().cast;
            }
            inAction = false;
            
        }

        if (key.ToString() == "W" && inAction  && GetComponent<Mana>().curMana > objectToSpawn.GetComponent<SkillInfo>().cost)
        {
            Debug.Log("Special attack ON " + key.ToString());
            GameObject go = Instantiate(objectToSpawn, transform.position, Quaternion.identity) as GameObject;
            GetComponent<Mana>().DecreaseMana(go.GetComponent<SkillInfo>().cost);
            //go.GetComponent<SkillInfo>().damage = damage;
            GetComponent<Alive>().OnHealUp(damage);
            Destroy(go, 2);
            if (gameObject == GameObject.FindGameObjectWithTag("Player"))
            {
                ActionBar.skillSlots[1].coolDownActive = true;
                ActionBar.skillSlots[1].coolDonwTime = go.GetComponent<SkillInfo>().cast;
            }
            inAction = false;

        }

        if (key.ToString() == "E" && inAction  && GetComponent<Mana>().curMana > objectToSpawn.GetComponent<SkillInfo>().cost)
        {
            Debug.Log("Special attack ON " + key.ToString());
            GameObject go = Instantiate(objectToSpawn,transform.position, Quaternion.identity) as GameObject;
            go.GetComponent<SkillInfo>().damage = damage;
            GetComponent<Mana>().DecreaseMana(go.GetComponent<SkillInfo>().cost);
            Destroy(go, 5);
            if (gameObject == GameObject.FindGameObjectWithTag("Player"))
            {
                ActionBar.skillSlots[2].coolDownActive = true;
                ActionBar.skillSlots[2].coolDonwTime = go.GetComponent<SkillInfo>().cast;
            }
            inAction = false;

        }

        if (key.ToString() == "R" && inAction && GetComponent<Mana>().curMana > objectToSpawn.GetComponent<SkillInfo>().cost)
        {
            Debug.Log("Special attack ON " + key.ToString());
            GameObject go = Instantiate(objectToSpawn, transform.position, Quaternion.identity) as GameObject;
            go.GetComponent<SkillInfo>().damage = damage;
            GetComponent<Mana>().DecreaseMana(go.GetComponent<SkillInfo>().cost);
            Destroy(go, 4);
            if (gameObject == GameObject.FindGameObjectWithTag("Player"))
            {
                ActionBar.skillSlots[3].coolDownActive = true;
                ActionBar.skillSlots[3].coolDonwTime = go.GetComponent<SkillInfo>().cast;
            }
            inAction = false;

        }


    }

    private float GetDistanceBetweenPositions(Vector3 start, Vector3 end)
    {
        return Mathf.Sqrt(Mathf.Pow((end.x - start.x), 2) + Mathf.Pow(end.y - start.y, 2));
    }

    Vector2 GetNormalizeStartToEnd(Vector2 start,Vector2 end)
    {
        Vector2 vectorToNormalized = end - start;
        float lengthOfNormalized = GetLengthVector(vectorToNormalized);

        return new Vector2(vectorToNormalized.x / lengthOfNormalized, vectorToNormalized.y / lengthOfNormalized);
    }

    Vector3 MakeQSpawnPointTarget(float distance,Vector3 currentPosition,Vector2 norma)
    {
        Vector3 spawn = new Vector3();
        spawn.x = currentPosition.x + distance * norma.x;
        spawn.y = currentPosition.y;
        spawn.z = currentPosition.z + distance * norma.y;

        return spawn;
    }
    float GetLengthVector(Vector2 vec)
    {
        return Mathf.Sqrt(vec.x * vec.x + vec.y * vec.y);
    }
}
