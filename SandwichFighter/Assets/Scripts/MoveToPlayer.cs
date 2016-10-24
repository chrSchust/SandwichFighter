using UnityEngine;
using System.Collections;

public class MoveToPlayer : MonoBehaviour {

    public float speed = 0.1F;
    private GameObject player;
    private Rigidbody rb;
    private Vector3 movement;


    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("FirstPersonCharacter");
        movement = player.transform.position - transform.position;
    }

    void FixedUpdate()
    {
        //movement = player.transform.position - transform.position;
        rb.AddForce(movement * speed);
    }

}
