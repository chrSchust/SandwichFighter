using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WeaponController : MonoBehaviour {

    public float speed = 0.1F;
    public List<int> ingredients { get; set; }

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButton(0))
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, new Quaternion(0.3f, -0.2f, 0.1f, 0.9f), Time.deltaTime * speed);
        }
        else
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, new Quaternion(0f, 0f, 0f, 1f), Time.deltaTime * speed);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy" && Input.GetMouseButton(0))
        {
            col.gameObject.GetComponent<EnemyController>().HitByPlayer(ingredients);
        }
    }
}
