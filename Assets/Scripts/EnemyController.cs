using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {
    private GameObject destination;
    public Material hitMaterial;
    public Material defaultMaterial;
    public Material normalMaterial;
    public Material veganMaterial;
    public float health;
    public int type { get; set; }
    public int baseDamage = 10;

    // Use this for initialization
    void Start () {
        health = Enemy.HealthList[type];

        switch (type)
        {
            case Enemy.NORMAL:
                defaultMaterial = normalMaterial;
                break;
            case Enemy.VEGAN:
                defaultMaterial = veganMaterial;
                break;
            default:
                break;
        }
        this.GetComponent<Renderer>().material = defaultMaterial;

        destination = GameObject.Find("Destination");
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = destination.transform.position;
    }

    void FixedUpdate()
    {

    }

    public void HitByPlayer(List<int> ingredients)
    {
        StartCoroutine(displayDamage());

        int bonusDamage = 0;
        foreach (int ingredient in ingredients)
        {
            try
            {
                bonusDamage = bonusDamage + Enemy.IngredientBonus[type][ingredient];
            }
            catch
            {
                Debug.Log("No bonus Damage defined for" + type + "and" + ingredient);
            }
        }
        int damage = baseDamage + bonusDamage;
        health = health - damage;
        if(health <= 0)
        {
            Destroy(this.gameObject);
            GameObject GameFlow = GameObject.Find("GameFlow");
            GameFlow.GetComponent<GameFlowController>().checkWon();
        }
    }

    IEnumerator displayDamage()
    {
        float time = 0f;
        float duration = 0.3f;

        while(time <= duration)
        {
            time += Time.deltaTime;
            this.GetComponent<Renderer>().material = hitMaterial;
            yield return null;
        }
        this.GetComponent<Renderer>().material = defaultMaterial;
    }
}
