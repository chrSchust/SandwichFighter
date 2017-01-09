using UnityEngine;
using System.Collections;

public class ArrowController : MonoBehaviour {

    public GameObject counter;
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        //Vector3 target = new Vector3(counter.transform.position.x, transform.position.y, counter.transform.position.z);
        transform.LookAt(counter.transform);
    }
}
