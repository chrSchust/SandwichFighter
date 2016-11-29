using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    public Material hitMaterial;
    public float health;
    public float speed;
    public int type { get; set; }
    public int baseDamage = 10;
    private int? lastWaypointIndex = null;
    private int wayPointsToVisit;
    private int wayPointsVisited = 0;

    private NavMeshAgent agent;

    private Animator animator;
    private Material defaultMaterial;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        health = Enemy.HealthList[type];
        speed = Enemy.SpeedList[type];
        wayPointsToVisit = Enemy.WaypointList[type];

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

        agent = GetComponent<NavMeshAgent>();
        agent.avoidancePriority = UnityEngine.Random.Range(45, 55);
        agent.speed = speed;

        moveToWaypoint();
    }



    void FixedUpdate()
    {
        animator.SetFloat("Speed", 1f);
        // destination = GameObject.Find("Destination");

        // Bad performance in the future. Try to find other solution later
        // agent = GetComponent<NavMeshAgent>();
        // agent.destination = destination.transform.position;
    }

    void moveToWaypoint()
    {
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        int newWaypointIndex = UnityEngine.Random.Range(0, waypoints.Length);
        if (lastWaypointIndex != null && lastWaypointIndex == newWaypointIndex)
        {
            moveToWaypoint();
            return;
        }
        lastWaypointIndex = newWaypointIndex;
        GameObject waypoint = waypoints[newWaypointIndex];
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = waypoint.transform.position;
    }

    void moveToDestination()
    {
        GameObject destination = GameObject.Find("Destination");
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = destination.transform.position;
    }


    public void HitByPlayer(List<Ingredient> ingredients)
    {
        StartCoroutine(displayDamage());

        int speedBonus = 0;

        foreach (Ingredient ingredient in ingredients)
        {
            //Debug.Log(ingredient.getName());
            //Debug.Log(ingredient.getSpeedBonus(type));
            speedBonus = speedBonus + ingredient.getSpeedBonus(type);
        }
        speed = speed + speedBonus;
        agent.speed = speed;


        int bonusDamage = 0;

        foreach (Ingredient ingredient in ingredients)
        {
            //Debug.Log(ingredient.getName());
            //Debug.Log(ingredient.getDamageBonus(type));
            bonusDamage = bonusDamage + ingredient.getDamageBonus(type);
        }

        int damage = baseDamage + bonusDamage;
        health = health - damage;
        if (health <= 0)
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

        while (time <= duration)
        {
            time += Time.deltaTime;
            GetComponentInChildren<Renderer>().material = hitMaterial;
            yield return null;
        }
        GetComponentInChildren<Renderer>().material = defaultMaterial;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Waypoint")
        {
            GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
            if (lastWaypointIndex == Array.IndexOf(waypoints, other.gameObject))
            {
                wayPointsVisited++;
                if (wayPointsVisited >= wayPointsToVisit)
                {
                    moveToDestination();
                }
                else moveToWaypoint();
            }
        }
    }
}
