using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WeaponController : MonoBehaviour {

    public float speed = 0.1F;

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
}
