using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WeaponController : MonoBehaviour {

    public float speed = 0.1F;
    public Quaternion startRotation;
	public Vector3 startPosition;
    public Material material1;
	private GuiManager guiManager = null;

    void Start()
    {
		guiManager = GameObject.FindGameObjectWithTag("GuiManager").GetComponent<GuiManager>();
		if (guiManager == null)
		{
			Debug.LogError("GuiManager is null");
		}
		startRotation = transform.localRotation;
		startPosition = transform.localPosition;
        //GetComponentInChildren<Renderer>().material = material1;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<Animator>().Play("HitAnimation");
            //transform.localRotation = Quaternion.Lerp(transform.localRotation, new Quaternion(0.3f, -0.2f, 0.1f, 0.9f), Time.deltaTime * speed);
        }
    }
}
