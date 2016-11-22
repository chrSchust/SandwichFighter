﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeAttack : MonoBehaviour {
	public float distance;
	public float maxHitDistance = 2.5f;
	public List<int> ingredients { get; set; }
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			RaycastHit hit;
			if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit)) {
				distance = hit.distance;
				Debug.Log (distance);
				if (distance < maxHitDistance) {
					hit.transform.SendMessage ("HitByPlayer", ingredients, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}
