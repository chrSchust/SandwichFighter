using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {
    private GameObject destination;
    public Material hitMaterial;
    public float health;
    public int type { get; set; }
    public int baseDamage = 10;

	private Animator animator;
	private Material defaultMaterial;
	private NavMeshAgent agent;
	// Use this for initialization
    void Start () {
		animator = GetComponent<Animator> ();
		health = Enemy.HealthList[type];

        /* switch (type)
        {
            case Enemy.NORMAL:
                //defaultMaterial = normalMaterial;
				defaultMaterial = GetComponentInChildren<Renderer>().material;;
                break;
            case Enemy.VEGAN:
				defaultMaterial = GetComponentInChildren<Renderer>().material;
                break;
            default:
                break;
        }*/
        // this.GetComponent<Renderer>().material = defaultMaterial;
		defaultMaterial = GetComponentInChildren<Renderer>().material;
		defaultMaterial = hitMaterial;
		Debug.Log (defaultMaterial);

        destination = GameObject.Find("Destination");
        agent = GetComponent<NavMeshAgent>();
        agent.destination = destination.transform.position;
    }

    void FixedUpdate()
    {
		animator.SetFloat ("Speed", 1f);
		destination = GameObject.Find("Destination");

		// Bad performance in the future. Try to find other solution later
		agent = GetComponent<NavMeshAgent>();
		agent.destination = destination.transform.position;
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
			GetComponentInChildren<Renderer>().material = hitMaterial;
            yield return null;
        }
		GetComponentInChildren<Renderer>().material = defaultMaterial;
    }
}
