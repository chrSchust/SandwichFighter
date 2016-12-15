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

		if (Input.GetKey ("1")) {
			if (guiManager.GetChosenWeapon () != 1) {
				guiManager.SetChosenWeapon (1);
				transform.localRotation = Quaternion.Lerp (transform.localRotation, 
					new Quaternion (startRotation.x, startRotation.y, -4.0f, startRotation.w), Time.deltaTime * speed);
			}
		}
		if (Input.GetKey ("2")) {
			if (guiManager.GetChosenWeapon () != 2) {
				guiManager.SetChosenWeapon (2);
				transform.localRotation = Quaternion.Lerp (transform.localRotation, 
					new Quaternion (startRotation.x, startRotation.y, -4.0f, startRotation.w), Time.deltaTime * speed);
			}
		}
    }
}
