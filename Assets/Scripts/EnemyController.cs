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
    private int? lastWaypointIndex = null;
    private int wayPointsToVisit;
    private int wayPointsVisited = 0;

    private Vector3 tempDest;
    private bool avoiding = false;
    public float maxHitDistance = 10f;
    public float hitRadius = 0.3f;

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

        RaycastHit[] allHits;
        allHits = Physics.SphereCastAll(transform.position + transform.forward + transform.up, hitRadius, transform.forward, maxHitDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        foreach (RaycastHit hit in allHits)
        {
            if (hit.transform.gameObject.tag == "Player" && avoiding == false)
            {
                tempDest = agent.destination;
                avoiding = true;

                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(GameObject.Find("AvoidRight").transform.position, path);
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    agent.destination = GameObject.Find("AvoidRight").transform.position;
                    //Debug.LogError("Avoid Right!");
                }
                else
                {
                    path = new NavMeshPath();
                    agent.CalculatePath(GameObject.Find("AvoidLeft").transform.position, path);
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        agent.destination = GameObject.Find("AvoidLeft").transform.position;
                        //Debug.LogError("Avoid Left!");
                    }
                    else
                    {
                        //Debug.LogError("Can't Avoid!");
                        avoiding = false;
                    }

                }
            }
            if (avoiding == true && agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.destination = tempDest;
                avoiding = false;
                Debug.LogError("Avoided!");
            }
        }
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


    public void HitByPlayer(List<Ingredient> ingredients, Bread activebread)
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
        int baseDamage = activebread.getBaseDamage();
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
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                r.material = hitMaterial;
            }
            //GetComponentInChildren<Renderer>().material = hitMaterial;
            yield return null;
        }
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material = defaultMaterial;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Waypoint")
        {
            avoiding = false;
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
