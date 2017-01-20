using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public AudioClip hit1;
    public AudioClip hit2;

    public Material hitMaterial;
    public float health;
    public float speed;
    public int type { get; set; }
	public Renderer rendererSkin;
	public Renderer rendererShirt;
	public Renderer rendererBeard;
	public Renderer rendererTrousersSkirt;
	public Renderer rendererShoeLeft;
	public Renderer rendererShoeRight;
	public Renderer rendererHairCap;
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
	private Material defaultMaterialSkin;
	private Material defaultMaterialBeard;
	private Material defaultMaterialShirt;
	private Material defaultMaterialTrousersSkirt;
	private Material defaultMaterialShoeLeft;
	private Material defaultMaterialShoeRight;
	private Material defaultMaterialHairCap;

    public float minSpeed = 0.5f;

    public GameObject DamageTextPrefab;

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
        // defaultMaterial = GetComponentInChildren<Renderer>().material;
		initDefaultMaterials();
		defaultMaterial = rendererShirt.material;
		Debug.Log (rendererBeard);

        agent = GetComponent<NavMeshAgent>();
        agent.avoidancePriority = UnityEngine.Random.Range(35, 65);
        agent.speed = speed;

        moveToWaypoint();
    }

	private void initDefaultMaterials() {
		if (rendererSkin != null)
			defaultMaterialSkin = rendererSkin.material;
		if (rendererShirt != null)
			defaultMaterialShirt = rendererShirt.material;
		if (rendererBeard != null)
			defaultMaterialBeard = rendererBeard.material;
		if (rendererTrousersSkirt != null)
			defaultMaterialTrousersSkirt = rendererTrousersSkirt.material;
		if (rendererShoeLeft != null)
			defaultMaterialShoeLeft = rendererShoeLeft.material;
		if (rendererShoeRight != null)
			defaultMaterialShoeRight = rendererShoeRight.material;
		if (rendererHairCap != null)
			defaultMaterialHairCap = rendererHairCap.material;
	}



    void FixedUpdate()
    {
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
        }
        if (avoiding == true && agent.remainingDistance <= 3)
        {
            agent.destination = tempDest;
            avoiding = false;
            //Debug.LogError("Avoided!");
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
        //AudioSource[] audioSources = GetComponents<AudioSource>();
        //audioSources[UnityEngine.Random.Range(0, audioSources.Length)].Play();
        if(UnityEngine.Random.Range(0, 2) == 0)
        {
            AudioSource.PlayClipAtPoint(hit1, transform.position);
        }
        else AudioSource.PlayClipAtPoint(hit2, transform.position);

        StartCoroutine(displayDamage());

        int speedBonus = 0;

        foreach (Ingredient ingredient in ingredients)
        {
            //Debug.Log(ingredient.getName());
            //Debug.Log(ingredient.getSpeedBonus(type));
            speedBonus = speedBonus + ingredient.getSpeedBonus(type);
        }
        if((speed + speedBonus) > minSpeed)
        {
            speed = speed + speedBonus;
        }
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
		Debug.Log (damage);
        InitDamageText(damage.ToString());
        health = health - damage;

        transform.Find("EnemyCanvas").Find("HealthBarForeground").gameObject.GetComponent<Image>().fillAmount = health / Enemy.HealthList[type];

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
            /*foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                r.material = hitMaterial;
            }*/
			ShowHitMaterial ();
            //GetComponentInChildren<Renderer>().material = hitMaterial;
            yield return null;
        }
		RestoreDefaultMaterial ();
		/* foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material = defaultMaterial;
        }*/
    }

	private void RestoreDefaultMaterial() {
		if (rendererSkin != null)
			rendererSkin.material = defaultMaterialSkin;
		if (rendererShirt != null)
			rendererShirt.material = defaultMaterialShirt;
		if (rendererBeard != null)
			rendererBeard.material = defaultMaterialBeard;
		if (rendererTrousersSkirt != null)
			rendererTrousersSkirt.material = defaultMaterialTrousersSkirt;
		if (rendererShoeLeft != null)
			rendererShoeLeft.material = defaultMaterialShoeLeft;
		if (rendererShoeRight != null)
			rendererShoeRight.material = defaultMaterialShoeRight;
		if (rendererHairCap != null)
			rendererHairCap.material = defaultMaterialHairCap;
	}

	private void ShowHitMaterial() {
		if (rendererSkin != null)
			rendererSkin.material = hitMaterial;
		if (rendererShirt != null)
			rendererShirt.material = hitMaterial;
		if (rendererBeard != null)
			rendererBeard.material = hitMaterial;
		if (rendererTrousersSkirt != null)
			rendererTrousersSkirt.material = hitMaterial;
		if (rendererShoeLeft != null)
			rendererShoeLeft.material = hitMaterial;
		if (rendererShoeRight != null)
			rendererShoeRight.material = hitMaterial;
		if (rendererHairCap != null)
			rendererHairCap.material = hitMaterial;
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

    void InitDamageText(string text)
    {
        GameObject temp = Instantiate(DamageTextPrefab) as GameObject;
        RectTransform tempRect = temp.GetComponent<RectTransform>();
        temp.transform.SetParent(transform.FindChild("EnemyCanvas"));
        tempRect.transform.localPosition = DamageTextPrefab.transform.localPosition;
        tempRect.transform.localScale = DamageTextPrefab.transform.localScale;
        tempRect.transform.localRotation = DamageTextPrefab.transform.localRotation;

        tempRect.GetComponent<Text>().text = text;
        Destroy(temp, 2);
    }
}
