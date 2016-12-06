using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeAttack : MonoBehaviour
{
    public float distance;
    public float maxHitDistance = 2.5f;
    public float hitRadius = 1.0f;
    public List<Ingredient> ingredients1 { get; set; }
    public List<Ingredient> ingredients2 { get; set; }
    public List<Ingredient> activeIngredients { get; set; }
    private GameObject weapon;
    public Material material1;
    public Material material2;
    public Bread bread1;
    public int? breadHealth1;
    public Bread bread2;
    public int? breadHealth2;
    public Bread activebread;

    void Start()
    {
        weapon = GameObject.Find("Weapon");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && breadHealth1 > 0)
        {
            activeIngredients = ingredients1;
            activebread = bread1;
            weapon.GetComponentInChildren<Renderer>().material = material1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && breadHealth2 > 0)
        {
            activeIngredients = ingredients2;
            activebread = bread2;
            weapon.GetComponentInChildren<Renderer>().material = material2;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (activeIngredients != null)
            {
                foreach (Ingredient i in activeIngredients)
                {
                    //Debug.Log(i.getName());
                }
            }

            if (breadHealth1 != null && breadHealth1 <= 0 && breadHealth2 != null && breadHealth2 <= 0)
            {
                Debug.Log("both dead");
                weapon.GetComponentInChildren<Renderer>().enabled = false;
                return;
            }
            else { weapon.GetComponentInChildren<Renderer>().enabled = true; }


            if (activebread != null && activebread == bread1)
            {
                breadHealth1--;
                if (breadHealth1 <= 0)
                {
                    Debug.Log("bread 1 dead");
                    if (breadHealth2 > 0)
                    {
                        activeIngredients = ingredients2;
                        activebread = bread2;
                        weapon.GetComponentInChildren<Renderer>().material = material2;
                    }

                }
            }
            if (activebread != null && activebread == bread2)
            {
                breadHealth2--;
                if (breadHealth2 <= 0)
                {
                    Debug.Log("bread 2 dead");
                    if (breadHealth1 > 0)
                    {
                        activeIngredients = ingredients1;
                        activebread = bread1;
                        weapon.GetComponentInChildren<Renderer>().material = material1;
                    }
                }
            }

            RaycastHit[] allHits;
            allHits = Physics.SphereCastAll(transform.position, hitRadius, transform.forward, maxHitDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
            foreach (RaycastHit hit in allHits)
            {
                if (hit.transform.gameObject.tag == "Enemy")
                {
                    hit.transform.gameObject.GetComponent<EnemyController>().HitByPlayer(activeIngredients, activebread);
                    break;
                }
            }
        }
    }
}
