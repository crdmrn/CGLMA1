using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picker : MonoBehaviour {

	// variable to select the key to pick up stuff
	public KeyCode pickupKey;


	public Transform pickupTarget;

	void Update () {
		// if we press the assigned key
		if (Input.GetKeyDown(pickupKey)) {

			// log that we're checking for pickables
			Debug.Log("check");

			// create raycasthit local variable to store the outcome of the spherecast function
			RaycastHit hit;

			// if our sphere cast intersects a collider
			if (Physics.SphereCast(transform.position, 1f, transform.forward, out hit, 1f)) {
				//check if that collider has a pickable component
				Pickable p = hit.transform.GetComponent<Pickable>();
				// if it does (so the variable is not null) log something
				if (p != null) {
					Debug.Log("pickup");

					// deactivate cube rigidbody
					Rigidbody cubeRigidbody = hit.transform.GetComponent<Rigidbody>();
					cubeRigidbody.isKinematic = true;
					cubeRigidbody.useGravity = false;

					// make cube child of pickable target transform and move it to LOCAL 0,0,0
					hit.transform.parent = pickupTarget;
					hit.transform.localPosition = Vector3.zero;
				}
			}


		}

		Debug.DrawRay(transform.position, transform.forward, Color.blue);
	}

}