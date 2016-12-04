using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeAttack : MonoBehaviour
{
    public float distance;
    public float maxHitDistance = 2.5f;
    public List<Ingredient> ingredients1 { get; set; }
    public List<Ingredient> ingredients2 { get; set; }
    public List<Ingredient> activeIngredients { get; set; }
    private GameObject weapon;
    public Material material1;
    public Material material2;

    void Start()
    {
        weapon = GameObject.Find("Weapon");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeIngredients = ingredients1;
            weapon.GetComponentInChildren<Renderer>().material = material1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activeIngredients = ingredients2;
            weapon.GetComponentInChildren<Renderer>().material = material2;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (activeIngredients != null)
            {
                foreach (Ingredient i in activeIngredients)
                {
                    Debug.Log(i.getName());
                }
            }

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
            {
                distance = hit.distance;
                //Debug.Log (distance);
                if (distance < maxHitDistance)
                {
                    //hit.transform.SendMessage ("HitByPlayer", ingredients, SendMessageOptions.DontRequireReceiver);
                    hit.transform.gameObject.GetComponent<EnemyController>().HitByPlayer(activeIngredients);
                }
            }
        }
    }
}
