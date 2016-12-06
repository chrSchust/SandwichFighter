using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WeaponController : MonoBehaviour {

    public float speed = 0.1F;
    public Quaternion startRotation;
    public Material material1;

    void Start()
    {
        startRotation = transform.localRotation;
        GetComponentInChildren<Renderer>().material = material1;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButton(0))
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, new Quaternion(0.3f, -0.2f, 0.1f, 0.9f), Time.deltaTime * speed);
        }
        else
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, startRotation, Time.deltaTime * speed);
        }
    }
}
