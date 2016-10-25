using UnityEngine;
using System.Collections;

public class MoveToPlayer : MonoBehaviour {

    public float speed = 0.07F;
    private GameObject player;
    private Rigidbody rb;
    private Vector3 movement;
    public Material hitMaterial;
    public Material defaultMaterial;
    public float health;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("FirstPersonCharacter");
        movement = player.transform.position - transform.position;
        health = 30;
    }

    void FixedUpdate()
    {
        movement = player.transform.position - transform.position;
        rb.AddForce(movement * speed);
    }

    public void HitByPlayer(float damage)
    {
        Debug.Log("hit");
        StartCoroutine(displayDamage());
        health = health - damage;
        if(health <= 0)
        {
            Destroy(this.gameObject);
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
