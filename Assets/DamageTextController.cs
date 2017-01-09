using UnityEngine;
using System.Collections;

public class DamageTextController : MonoBehaviour {

    private GameObject MainCamera;
    private Vector3 startScale;

	// Use this for initialization
	void Start () {
        MainCamera = GameObject.Find("MainCamera");
        startScale = transform.localScale;

    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt((2 * transform.position - MainCamera.transform.position));
        float distanceToCam = Vector3.Distance(transform.position, MainCamera.transform.position);
        transform.localScale = new Vector3(startScale.x * distanceToCam, startScale.y * distanceToCam, startScale.z * distanceToCam);
    }
}
